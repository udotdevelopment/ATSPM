using System;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using MOE.Common.Properties;

namespace MOE.Common.Business.LogDecoder
{
    public class ASC3Decoder
    {
        //static public MOE.Common.Data.MOE.Controller_Event_LogDataTable DecodeASC3File(string FileName, string SignalId)
        public static void DecodeASC3File(string FileName, string SignalId,
            BlockingCollection<Data.MOE.Controller_Event_LogRow> RowBag)
        {
            var EarliestAcceptableDate = Settings.Default.EarliestAcceptableDate;

            var encoding = Encoding.ASCII;
            using (var BR = new BinaryReader(File.Open(FileName, FileMode.Open), encoding))
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

                if (BR.BaseStream.Position + 21 < BR.BaseStream.Length)
                {
                    //Find the start Date
                    var dateString = "";
                    for (var i = 1; i < 21; i++)
                    {
                        var c = BR.ReadChar();
                        dateString += c;
                    }

                    //Console.WriteLine(dateString);
                    var StartTime = new DateTime();
                    if (DateTime.TryParse(dateString, out StartTime) && BR.BaseStream.Position < BR.BaseStream.Length)
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

                        while (i < 7 && BR.BaseStream.Position < BR.BaseStream.Length)
                        {
                            var c = BR.ReadChar();
                            //Console.WriteLine(c.ToString());
                            if (c == '\n')
                                i++;
                        }

                        //The first record alwasy seems to be missing a timestamp.  Econolite assumes the first even occures at the same time
                        // the second event occurs.  I am going to tag the first event with secondevet.timestamp - 1/10 second
                        var firstEventCode = new int();
                        var firstEventParam = new int();


                        if (BR.BaseStream.Position + sizeof(char) < BR.BaseStream.Length)
                            firstEventCode = Convert.ToInt32(BR.ReadChar());

                        if (BR.BaseStream.Position + sizeof(char) < BR.BaseStream.Length)
                            firstEventParam = Convert.ToInt32(BR.ReadChar());

                        var firstEventEntered = false;
                        //MOE.Common.Business.ControllerEvent firstEvent = new MOE.Common.Business.ControllerEvent(SignalID, StartTime, firstEventCode, firstEventParam);


                        //After that, we can probably start reading
                        while (BR.BaseStream.Position + sizeof(byte) * 4 <= BR.BaseStream.Length
                        ) //we need ot make sure we are more that 4 characters from the end
                        {
                            var EventTime = new DateTime();
                            var EventCode = new int();
                            var EventParam = new int();

                            //MOE.Common.Business.ControllerEvent controllerEvent = null;
                            for (var EventPart = 1; EventPart < 4; EventPart++)
                            {
                                //getting the time offset
                                if (EventPart == 1)
                                {
                                    var rawoffset = new byte[2];
                                    //char[] offset = new char[2];
                                    rawoffset = BR.ReadBytes(2);
                                    Array.Reverse(rawoffset);
                                    int offset = BitConverter.ToInt16(rawoffset, 0);

                                    var tenths = Convert.ToDouble(offset) / 10;

                                    EventTime = StartTime.AddSeconds(tenths);
                                }

                                //getting the EventCode
                                if (EventPart == 2)
                                    EventCode = Convert.ToInt32(BR.ReadByte());

                                if (EventPart == 3)
                                    EventParam = Convert.ToInt32(BR.ReadByte());
                            }

                            //controllerEvent = new MOE.Common.Business.ControllerEvent(SignalId, EventTime, EventCode, EventParam);

                            if (EventTime <= DateTime.Now && EventTime > EarliestAcceptableDate)
                                if (!firstEventEntered)
                                {
                                    try
                                    {
                                        var eventrow = elTable.NewController_Event_LogRow();
                                        eventrow.Timestamp = EventTime.AddMilliseconds(-1);
                                        eventrow.SignalID = SignalId;
                                        eventrow.EventCode = firstEventCode;
                                        eventrow.EventParam = firstEventParam;
                                        if (!RowBag.Contains(eventrow))
                                            RowBag.Add(eventrow);
                                    }
                                    catch
                                    {
                                    }

                                    firstEventEntered = true;
                                }
                                else
                                {
                                    try
                                    {
                                        var eventrow = elTable.NewController_Event_LogRow();
                                        eventrow.Timestamp = EventTime;
                                        eventrow.SignalID = SignalId;
                                        eventrow.EventCode = EventCode;
                                        eventrow.EventParam = EventParam;

                                        if (!RowBag.Contains(eventrow))
                                            RowBag.Add(eventrow);
                                    }
                                    catch
                                    {
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