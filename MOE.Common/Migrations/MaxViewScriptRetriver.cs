using MOE.Common.Models;
using System;

namespace MOE.Common.Migrations
{
    public static class MaxViewScriptRetriver
    {
        public static string GetImportScript()
        {
            var importSignalsFromMaxView = @"
                Insert into Signals(signalid, latitude, longitude, primaryname, secondaryname, ipaddress, RegionID, controllertypeid,enabled, versionactionid, note, start)

                SELECT      GroupableElements.ID as SignalID, 
			                GroupableElements_Device.CenterLatitude AS Longitude, 
			                GroupableElements_Device.CenterLongitude AS Latitude,           
			                GroupableElements_IntersectionController.Intersection_MainStreetName AS PrimaryName,			
			                GroupableElements_IntersectionController.Intersection_SideStreetName AS ScondaryName,             
			                DeviceConnectionProperties_IpDeviceSettings.HostAddress AS IPADDRESS, 
			                1 AS RegionId,
			                4 As ControllerTypeID, 
                            1 As Enabled,
			                10 AS VersionActionID, 
			                'Import From MaxView' AS Note, 
			                SYSDATETIME() AS Start

                FROM            [dbo].GroupableElements INNER JOIN
                                [dbo].GroupableElements_IntersectionController ON GroupableElements.ID = GroupableElements_IntersectionController.ID INNER JOIN
                                [dbo].GroupableElements_Device ON GroupableElements_IntersectionController.ID = GroupableElements_Device.ID INNER JOIN
                                [dbo].DeviceConnectionProperties ON GroupableElements_Device.DeviceConnectionPropertiesID = DeviceConnectionProperties.ID INNER JOIN
                                [dbo].DeviceConnectionProperties_IpDeviceSettings ON GroupableElements_Device.DeviceConnectivityTypeID = DeviceConnectionProperties_IpDeviceSettings.ID ";


            return importSignalsFromMaxView;
        }

        public static string GetDropTriggersScript()
        {
            var dropTriggers = @"IF OBJECT_ID ('[insertMVSignalFromAtspmTrigger] ', 'TR') IS NOT NULL
                DROP TRIGGER [insertMVSignalFromAtspmTrigger];


                IF OBJECT_ID ('[UpdateMVSignalInAtspmTrigger] ', 'TR') IS NOT NULL
                DROP TRIGGER [UpdateMVSignalInAtspmTrigger];

            IF OBJECT_ID ('[deleteMVSignalFromAtspmTrigger] ', 'TR') IS NOT NULL
            DROP TRIGGER [deleteMVSignalFromAtspmTrigger];";


            return dropTriggers;


        }

        public static string GetInsertTriggerScript()
        {
            var insertMVSignalFromAtspmTrigger = @"
                CREATE TRIGGER insertMVSignalFromAtspmTrigger  
                ON GroupableElements_IntersectionController
                AFTER INSERT
                AS
                Insert into signals(signalid, latitude, longitude, primaryname, secondaryname, ipaddress, RegionID, controllertypeid, versionactionid,enabled, note, start)
                Select      GroupableElements.ID as SignalID, 
			                GroupableElements_Device.CenterLatitude AS Longitude, 
			                GroupableElements_Device.CenterLongitude AS Latitude,           
			                GroupableElements_IntersectionController.Intersection_MainStreetName AS PrimaryName,			
			                GroupableElements_IntersectionController.Intersection_SideStreetName AS ScondaryName,             
			                DeviceConnectionProperties_IpDeviceSettings.HostAddress AS IPADDRESS, 
			                1 AS RegionId,
			                4 As ControllerTypeID, 
                            1 as enabled,
			                1 AS VersionActionID, 
			                'Import From MaxView' AS Note, 
			                SYSDATETIME() AS Start

                FROM             [dbo].GroupableElements INNER JOIN
                                [dbo].GroupableElements_IntersectionController ON GroupableElements.ID = GroupableElements_IntersectionController.ID INNER JOIN
                                [dbo].GroupableElements_Device ON GroupableElements_IntersectionController.ID = GroupableElements_Device.ID INNER JOIN
                                [dbo].DeviceConnectionProperties ON GroupableElements_Device.DeviceConnectionPropertiesID = DeviceConnectionProperties.ID INNER JOIN
                                [dbo].DeviceConnectionProperties_IpDeviceSettings ON GroupableElements_Device.DeviceConnectivityTypeID = DeviceConnectionProperties_IpDeviceSettings.ID 

                Where  GroupableElements.id in (select ID from inserted)

            ";

            return insertMVSignalFromAtspmTrigger;
        }

        public static string GetUpdateTriggerScript()
        {
                var updateMVSignalFromAtspmTrigger = @"
                CREATE TRIGGER UpdateMVSignalInAtspmTrigger  
                ON GroupableElements_IntersectionController
                for update
                AS
                Insert into signals(signalid, latitude, longitude, primaryname, secondaryname, ipaddress, RegionID, controllertypeid, versionactionid, enabled, note, start)
                Select      GroupableElements.ID as SignalID, 
			                GroupableElements_Device.CenterLatitude AS Longitude, 
			                GroupableElements_Device.CenterLongitude AS Latitude,           
			                GroupableElements_IntersectionController.Intersection_MainStreetName AS PrimaryName,			
			                GroupableElements_IntersectionController.Intersection_SideStreetName AS ScondaryName,             
			                DeviceConnectionProperties_IpDeviceSettings.HostAddress AS IPADDRESS, 
			                1 AS RegionId,
			                4 As ControllerTypeID, 
			                2 AS VersionActionID, 
                            1 as enabled,
			                'Update From MaxView' AS Note, 
			                SYSDATETIME() AS Start

                FROM             [dbo].GroupableElements INNER JOIN
                                [dbo].GroupableElements_IntersectionController ON GroupableElements.ID = GroupableElements_IntersectionController.ID INNER JOIN
                                [dbo].GroupableElements_Device ON GroupableElements_IntersectionController.ID = GroupableElements_Device.ID INNER JOIN
                                [dbo].DeviceConnectionProperties ON GroupableElements_Device.DeviceConnectionPropertiesID = DeviceConnectionProperties.ID INNER JOIN
                                [dbo].DeviceConnectionProperties_IpDeviceSettings ON GroupableElements_Device.DeviceConnectivityTypeID = DeviceConnectionProperties_IpDeviceSettings.ID 

                Where  GroupableElements.id in (select ID from inserted)

            ";

            return updateMVSignalFromAtspmTrigger;

        }

        public static string GetDeleteTriggerScript()
        {
            var DeleteTriggerScript = @"

      
            CREATE TRIGGER deleteMVSignalFromAtspmTrigger  
                ON GroupableElements_IntersectionController
                AFTER delete
                AS
                
                update signals set versionactionid = 3, enabled = 0 Where SignalId in (select ID from deleted)

                

            ";

        return DeleteTriggerScript;
        }

        public static string GetCreateCELView()
        {
            var script = @"
            CREATE VIEW [dbo].[Controller_Event_Log]
            AS
            SELECT        TimeStamp AS Timestamp, Cast(DeviceId AS nvarchar(10)) AS SignalID, Cast(EventId as int) AS EventCode, cast(Parameter as int) AS EventParam
            FROM            dbo.ASCEvents;";
             return script;

        }
}
}
