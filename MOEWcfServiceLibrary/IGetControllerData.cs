using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGetControllerData" in both code and config file together.
    [ServiceContract]
    public interface IGetControllerData
    {
        [OperationContract]
        void UploadControllerData(string IPAddress, string SignalID, string UserName, string Password, string LocalDir, string RemoteDir, bool DeleteFiles, int SNMPRetry, int SNMPTimeout, int SNMPPort, bool ImportAfterFTP, bool ActiveMode, int WaitBetweenRecords,MOE.Common.Business.BulkCopyOptions Options);
    }
}
