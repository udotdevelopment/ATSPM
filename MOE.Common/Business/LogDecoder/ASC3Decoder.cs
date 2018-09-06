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
        //static public MOE.Common.Data.MOE.Controller_Event_LogDataTable DecodeASC3File(string FileName, string SignalId)
        public static void DecodeAsc3File(string fileName, string signalId,
            BlockingCollection<Data.MOE.Controller_Event_LogRow> rowBag, DateTime earliestAcceptableDate)
        {
            var encoding = Encoding.ASCII;
            using (var br = new BinaryReader(File.Open(fileName, FileMode.Open), encoding))
            {
                var elTable = new Data.MOE.Controller_Event_LogDataTable();
                //var custUnique =
                //    new UniqueConstraint(new[]
                //    {
                //        elTable.Columns["SignalID"],
                //        elTable.Columns["Timestamp"],
                //        elTable.Columns["EventCode"],
                //        elTable.Columns["EventParam"]
                //    });

                //elTable.Constraints.Add(custUnique);

                if (br.BaseStream.Position + 21 < br.BaseStream.Length)
                {
                    //Find the start Date
                    var dateString = "";
                    for (var i = 1; i < 21; i++)
                    {
                        var c = br.ReadChar();
                        dateString += c;
                    }

                    //Console.WriteLine(dateString);
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
                            //Console.WriteLine(c.ToString());
                            if (c == '\n')
                                i++;
                        }

                        //The first record alwasy seems to be missing a timestamp.  Econolite assumes the first even occures at the same time
                        // the second event occurs.  I am going to tag the first event with secondevet.timestamp - 1/10 second
                        var firstEventCode = new int();
                        var firstEventParam = new int();


                        if (br.BaseStream.Position + sizeof(char) < br.BaseStream.Length)
                            firstEventCode = Convert.ToInt32(br.ReadChar());

                        if (br.BaseStream.Position + sizeof(char) < br.BaseStream.Length)
                            firstEventParam = Convert.ToInt32(br.ReadChar());

                        var firstEventEntered = false;
                        //MOE.Common.Business.ControllerEvent firstEvent = new MOE.Common.Business.ControllerEvent(SignalID, StartTime, firstEventCode, firstEventParam);


                        //After that, we can probably start reading
                        while (br.BaseStream.Position + sizeof(byte) * 4 <= br.BaseStream.Length
                        ) //we need ot make sure we are more that 4 characters from the end
                        {
                            var eventTime = new DateTime();
                            var eventCode = new int();
                            var eventParam = new int();

                            //MOE.Common.Business.ControllerEvent controllerEvent = null;
                            for (var eventPart = 1; eventPart < 4; eventPart++)
                            {
                                //getting the time offset
                                if (eventPart == 1)
                                {
                                    var rawoffset = new byte[2];
                                    //char[] offset = new char[2];
                                    rawoffset = br.ReadBytes(2);
                                    Array.Reverse(rawoffset);
                                    int offset = BitConverter.ToInt16(rawoffset, 0);

                                    var tenths = Convert.ToDouble(offset) / 10;

                                    eventTime = startTime.AddSeconds(tenths);
                                }

                                //getting the EventCode
                                if (eventPart == 2)
                                    eventCode = Convert.ToInt32(br.ReadByte());

                                if (eventPart == 3)
                                    eventParam = Convert.ToInt32(br.ReadByte());
                            }

                            //controllerEvent = new MOE.Common.Business.ControllerEvent(SignalId, EventTime, EventCode, EventParam);

                            if (eventTime <= DateTime.Now && eventTime > earliestAcceptableDate)
                                if (!firstEventEntered)
                                {
                                    try
                                    {
                                        var eventrow = elTable.NewController_Event_LogRow();
                                        eventrow.Timestamp = eventTime.AddMilliseconds(-1);
                                        eventrow.SignalID = signalId;
                                        eventrow.EventCode = firstEventCode;
                                        eventrow.EventParam = firstEventParam;
                                        rowBag.Add(eventrow);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }

                                    firstEventEntered = true;
                                }
                                else
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


                //return true;
            }
        }
    }
}