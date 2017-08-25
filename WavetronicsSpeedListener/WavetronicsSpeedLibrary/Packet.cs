using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MOE.Common;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;



namespace WavetronicsSpeedLibrary
{
    // ----------------
    // Packet Structure
    // ----------------


    // Description  ->  |dataIdentifier|DeviceID|Header|SpeedMPH|SpeedKPH|6Digittag|ClosingFlags|
    // Size In Bytes -> |       2      |    4   |  2   |   1    |    1   |     6   |       3    | = 19 Bytes
    //
    //I am unclear of the purpose of the data identifier bits
    //
    //Device ID is a 4 digit number that usually corresponds with the last four digits of the Advance serial number
    //  It IS user configurable, so it could be set to the signal id or some other identifier
    //  The Device ID will only be sent if the "Remove Multi-drop Identifier" toggle on the Advance sensor is tunred off.
    //
    //Header is supposed to be a message type indentifier.  If the Multi-drop Identifier is turned off, it will actually come at the begining of the message
    //      
    //  (SpeedMPH * 1.609344) should just about = SpeedKPH
    //
    //The  6-digit tag can and should be configured to be the i2 signal id + the detector channel for this detector
    //
    //The closing tags are "7e 0d 0d", or "~\r\r"

    

    public class Packet
    {
        #region Private Members
        private string sensorId;
        private int mph;
        private int kph;
        private int header;
        private DateTime date;
        private string senderIPAddress;
        private bool goodChecksum;

        #endregion

        #region Public Properties

        public string SenderIPAddress
        {
            get { return senderIPAddress; }
            set { senderIPAddress = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string SensorId
        {
            get { return sensorId; }
            set { sensorId = value; }
        }

        public int Mph
        {
            get { return mph; }
            set { mph = value; }
        }

        public int Kph
        {
            get { return kph; }
            set { kph = value; }
        }

        public bool GoodChecksum
        {
            get { return goodChecksum; }
            set { goodChecksum = value; }
        }

        #endregion

        #region Methods

        // Default Constructor
        public Packet()
        {
            this.senderIPAddress = "";
            this.sensorId = null;
            this.mph = -1;
            this.kph = -1;
        }

        public Packet(byte[] dataStream, string senderipaddress)
        {
            date = DateTime.Now;
            senderIPAddress = senderipaddress;
            try
            {
                char[] tempSensorId = new char[6] {Convert.ToChar(dataStream[10]), Convert.ToChar(dataStream[11]),
                Convert.ToChar(dataStream[12]), Convert.ToChar(dataStream[13]), Convert.ToChar(dataStream[14]), Convert.ToChar(dataStream[15]) };
                this.sensorId = new string(tempSensorId);
                this.mph = dataStream[8];
                this.kph = dataStream[9];
                this.header = dataStream[7];

            }
            catch 
            {

            }

            if (this.kph == Math.Round(this.mph * 1.609344))
            {
            goodChecksum = true;
            this.Save();
            }
            else if (header == 83)
            {

            }
            
        }

        private void Save()
        {
                try
                {
                    MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
                    {
                        MOE.Common.Models.Speed_Events se = new MOE.Common.Models.Speed_Events();
                        se.DetectorID = this.sensorId;
                        se.timestamp = this.date;
                        se.MPH = this.mph;
                        se.KPH = this.kph;
                        Regex r = new Regex("^[a-zA-Z0-9]*$");
                        if (r.IsMatch(se.DetectorID))
                        {
                            db.Speed_Events.Add(se);
                            db.SaveChanges();
                        }  
                    }
                }
                catch (DbUpdateException ex)
                {
                    SqlException innerException = ex.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                    }
                    else
                    {
                        MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                        eventRepository.QuickAdd("SpeedListener", this.GetType().ToString(), "Save",
                            MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.HResult.ToString() + ex.Message + ex.InnerException);
                    }
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    eventRepository.QuickAdd("SpeedListener", this.GetType().ToString(), "Save",
                        MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.HResult.ToString() + ex.Message + ex.InnerException);
                }
            

        }
        #endregion
    }
}
