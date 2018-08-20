using System;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using MOE.Common.Properties;

namespace MOE.Common.Business.LogDecoder
{
    public class Asc3Decoder
    {
        public static void DecodeAsc3File(string fileName, string signalId,
            BlockingCollection<Data.MOE.Controller_Event_LogRow> rowBag, DateTime earliestAcceptableDate)
        {
            var encoding = Encoding.ASCII;
            using (var br = new BinaryReader(File.Open(fileName, FileMode.Open), encoding))
            {
                var elTable = new Data.MOE.Controller_Event_LogDataTable();
                var custUnique =
                    new UniqueConstraint(new[]
                    {
                        elTable.Columns["SignalID"],
                        elTable.Columns["Timestamp"],
                        elTable.Columns["EventCode"],
                        elTable.Columns["EventParam"]
                    });

                elTable.Constraints.Add(custUnique);

                if (br.BaseStream.Position + 21 < br.BaseStream.Length)
                {
                    //Find the start Date
                    var dateString = "";
                    for (var i = 1; i < 21; i++)
                    {
                        var c = br.ReadChar();
                        dateString += c;
                    }

                    var startTime = new DateTime();
                    if (DateTime.TryParse(dateString, out startTime) && br.BaseStream.Position < br.BaseStream.Length)
                    {
                        //find  line feed characters, that should take us to the end of the header.
                        // First line break is after Version
                        // Second LF is after FileName
                        // Third LF is after Interseciton number, which isn't used as far as I can tell
                        // Fourth LF is after IP address
                        // Fifth is after MAC Address
                        // Sixth is after "Controller data log beginning:," and then the date
                        // Seven is after "Phases in use:," and then the list of phases, seperated by commas

                        var i = 0;

                        while (i < 7 && br.BaseStream.Position < br.BaseStream.Length)
                        {
                            var c = br.ReadChar();
                            if (c == '\n')
                                i++;
                        }

                        // after that, we start reading until we reach the end 
                        while (br.BaseStream.Position + sizeof(byte) * 4 <= br.BaseStream.Length)
                        {
                            var eventTime = new DateTime();
                            var eventCode = new int();
                            var eventParam = new int();

                            for (var eventPart = 1; eventPart < 4; eventPart++)
                            {
                                //getting the EventCode
                                if (eventPart == 1)
                                    eventCode = Convert.ToInt32(br.ReadByte());

                                if (eventPart == 2)
                                    eventParam = Convert.ToInt32(br.ReadByte());

                                //getting the time offset
                                if (eventPart == 3)
                                {
                                    var rawoffset = new byte[2];
                                    rawoffset = br.ReadBytes(2);
                                    Array.Reverse(rawoffset);
                                    int offset = BitConverter.ToInt16(rawoffset, 0);
                                    var tenths = Convert.ToDouble(offset) / 10;
                                    eventTime = startTime.AddSeconds(tenths);
                                }
                            }

                            if (eventTime <= DateTime.Now && eventTime > earliestAcceptableDate)
                            {
                                try
                                {
                                    var eventrow = elTable.NewController_Event_LogRow();
                                    eventrow.Timestamp = eventTime;
                                    eventrow.SignalID = signalId;
                                    eventrow.EventCode = eventCode;
                                    eventrow.EventParam = eventParam;
                                    rowBag.Add(eventrow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    //this is what we do when the datestring doesn't parse
                }
                //this is what we do when the datestring doesn't parse
            }
        }
    }
}