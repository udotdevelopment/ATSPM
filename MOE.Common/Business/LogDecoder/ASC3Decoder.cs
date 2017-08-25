using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using AlexPilotti;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.ComponentModel;
using System.Timers;

namespace MOE.Common.Business.LogDecoder
{
    public class ASC3Decoder
    {
        //static public MOE.Common.Data.MOE.Controller_Event_LogDataTable DecodeASC3File(string FileName, string SignalId)
            static public void DecodeASC3File(string FileName, string SignalId, BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow> RowBag)
        {
            DateTime EarliestAcceptableDate = Properties.Settings.Default.EarliestAcceptableDate;

            System.Text.Encoding encoding = System.Text.Encoding.ASCII;
            using (BinaryReader BR = new BinaryReader(File.Open(FileName, FileMode.Open), encoding))
            {


                MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = new MOE.Common.Data.MOE.Controller_Event_LogDataTable();
                UniqueConstraint custUnique =
                new UniqueConstraint(new DataColumn[] { elTable.Columns["SignalID"],
                                                    elTable.Columns["Timestamp"], 
                                                    elTable.Columns["EventCode"],
                                                    elTable.Columns["EventParam"]
                                            });

                elTable.Constraints.Add(custUnique);

                if (BR.BaseStream.Position + 21 < BR.BaseStream.Length)
                {
                    //Find the start Date
                    String dateString = "";
                    for (int i = 1; i < 21; i++)
                    {

                        Char c = BR.ReadChar();
                        dateString += c;
                    }

                    //Console.WriteLine(dateString);
                    DateTime StartTime = new DateTime();
                    if (DateTime.TryParse(dateString, out StartTime) && (BR.BaseStream.Position < BR.BaseStream.Length))
                    {

                        //find  line feed characters, that should take us to the end of the header.
                        // First line break is after Version
                        // Second LF is after FileName
                        // Third LF is after Interseciton number, which isn't used as far as I can tell
                        // Fourth LF is after IP address
                        // Fifth is after MAC Address
                        // Sixth is after "Controller data log beginning:," and then the date
                        // Seven is after "Phases in use:," and then the list of phases, seperated by commas

                        int i = 0;

                        while (i < 7 && (BR.BaseStream.Position < BR.BaseStream.Length))
                        {
                            Char c = BR.ReadChar();
                            //Console.WriteLine(c.ToString());
                            if (c == '\n')
                            {
                                i++;

                            }

                        }

                        //The first record alwasy seems to be missing a timestamp.  Econolite assumes the first even occures at the same time
                        // the second event occurs.  I am going to tag the first event with secondevet.timestamp - 1/10 second
                        int firstEventCode = new Int32();
                        int firstEventParam = new Int32();


                        if (BR.BaseStream.Position + sizeof(char) < BR.BaseStream.Length)
                        {
                            firstEventCode = Convert.ToInt32(BR.ReadChar());
                        }

                        if (BR.BaseStream.Position + sizeof(char) < BR.BaseStream.Length)
                        {
                            firstEventParam = Convert.ToInt32(BR.ReadChar());
                        }

                        bool firstEventEntered = false;
                        //MOE.Common.Business.ControllerEvent firstEvent = new MOE.Common.Business.ControllerEvent(SignalID, StartTime, firstEventCode, firstEventParam);


                        //After that, we can probably start reading
                        while ((BR.BaseStream.Position + (sizeof(byte) * 4)) <= BR.BaseStream.Length)  //we need ot make sure we are more that 4 characters from the end
                        {
                            DateTime EventTime = new DateTime();
                            int EventCode = new Int32();
                            int EventParam = new Int32();

                            //MOE.Common.Business.ControllerEvent controllerEvent = null;
                            for (int EventPart = 1; EventPart < 4; EventPart++)
                            {

                                //getting the time offset
                                if (EventPart == 1)
                                {
                                    byte[] rawoffset = new byte[2];
                                    //char[] offset = new char[2];
                                    rawoffset = BR.ReadBytes(2);
                                    Array.Reverse(rawoffset);
                                    int offset = BitConverter.ToInt16(rawoffset, 0);

                                    double tenths = Convert.ToDouble(offset) / 10;

                                    EventTime = StartTime.AddSeconds(tenths);
                                }

                                //getting the EventCode
                                if (EventPart == 2)
                                {
                                    //Char EventCodeChar = BR.ReadChar();
                                    //EventCode = Convert.ToInt32(BR.ReadChar());
                                    EventCode = Convert.ToInt32(BR.ReadByte());
                                }

                                if (EventPart == 3)
                                {
                                    //Char EventParamChar = BR.ReadChar();
                                    //EventParam = Convert.ToInt32(BR.ReadChar());
                                    EventParam = Convert.ToInt32(BR.ReadByte());
                                }


                            }

                            //controllerEvent = new MOE.Common.Business.ControllerEvent(SignalID, EventTime, EventCode, EventParam);

                            if ((EventTime) <= DateTime.Now && (EventTime > EarliestAcceptableDate))
                            {
                               
                                
                                if (!firstEventEntered)
                                {
                                    try
                                    {

                                        MOE.Common.Data.MOE.Controller_Event_LogRow eventrow = elTable.NewController_Event_LogRow();
                                        eventrow.Timestamp = EventTime.AddMilliseconds(-1);
                                        eventrow.SignalID = SignalId;
                                        eventrow.EventCode = firstEventCode;
                                        eventrow.EventParam = firstEventParam;
                                        if (!RowBag.Contains(eventrow))
                                        {
                                            RowBag.Add(eventrow);
                                        }
                                        //elTable.AddController_Event_LogRow(SignalID, EventTime.AddMilliseconds(-1), firstEventCode, firstEventParam);
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
                                        MOE.Common.Data.MOE.Controller_Event_LogRow eventrow = elTable.NewController_Event_LogRow();
                                        eventrow.Timestamp = EventTime;
                                        eventrow.SignalID = SignalId;
                                        eventrow.EventCode = EventCode;
                                        eventrow.EventParam = EventParam;

                                        if (!RowBag.Contains(eventrow))
                                        {
                                            RowBag.Add(eventrow);
                                        }
                                        //elTable.AddController_Event_LogRow(Convert.ToInt32(SignalID), EventTime, EventCode, EventParam);
                                    }
                                    catch
                                    {

                                    }
                                }


                            }
                        }


                    }
                    //this is what we do when the datestring doesn't parse
                    else
                    {
                       // return false;
                        
                    }



                }

                    //this is what we do when the datestring doesn't parse
                else
                {
                    //return false;
                }




                //return true;



            }

        }
    }
}
