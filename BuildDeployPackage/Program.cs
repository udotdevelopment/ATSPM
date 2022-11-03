using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildDeployPackage
{

    class Program
    {
        public static string ParentFolderLocation { get; set; }
        public static string ProjectFolderLocation { get; set; }
        public static string ReleaseBinDirectory { get; set; }

        public static string DebugBinDirectory { get; set; }
        /// <summary>
        /// You may need to give your project folder control access to everyone to run this
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            ParentFolderLocation = appSettings["ParentFolderLocation"];
            ProjectFolderLocation = appSettings["ProjectFolderLocation"];
            DebugBinDirectory = appSettings["DeployDebug"];
            ReleaseBinDirectory = appSettings["DeployRelease"];
            ModifyConnectionStrings();
            CreateParentFolder();
            CreateAsyncGetMaxTimeRecordsDeploy();
            CreateGetMaxTimeRecordsDeploy();
            CreateATSPMAPIDeploy();
            CreateDecodePeekLogsDeploy();
            CreateDecodeSiemensLogsDeploy();
            CreateFTPFromAllControllersDeploy();
            CreateTrafficwareLogsDeploy();
            CreateGenerateAddDataScriptDeploy();
            CreateImportCheckerDeploy();
            CreateMOEWcfServiceLibraryDeploy();
            CreateNewDecodeandImportASC3LogsDeploy();
            CreateSPMDeploy();
            CreateWatchDogDeploy();
            CreateWavetronicsSpeedListenerDeploy();
            CreateAggregateDeploy();
            CreateInstallerDeploy();
            CreateConvertDBForHistoricalConfigurationsDeploy();
        }

        private static void CreateFTPFromAllControllersDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["FTPFromAllControllersProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }


        private static void ModifyConnectionStrings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string connectionStringReplacement = appSettings["ConnectionStringReplacement"];
            string filePath = string.Empty;
            //if (isConsoleApp)
            //{
            //    filePath = ProjectFolderLocation + filePathAndName + "\\App.config";
            //}
            //else
            //{
            //    filePath= ProjectFolderLocation + filePathAndName + "\\Web.config";
            //}

            foreach (var key in appSettings.AllKeys)
            {
                if (key.Contains("ProjectFolderLocation"))
                {
                    if (File.Exists(ProjectFolderLocation + appSettings[key] + "\\App.config"))
                    {
                        filePath = ProjectFolderLocation + appSettings[key] + "\\App.config";
                    }
                    else
                    {
                        filePath = ProjectFolderLocation + appSettings[key] + "\\Web.config";
                    }

                    if (File.Exists(filePath))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(filePath);
                        XmlNodeList elements = xml.SelectNodes(@"/configuration/connectionStrings/add");
                        foreach (XmlNode element in elements)
                        {
                            if (!element.OuterXml.Contains("ADConnectionString"))
                                element.Attributes["connectionString"].InnerText = connectionStringReplacement;
                        }

                        xml.Save(filePath);
                    }
                }
            }
        }


        private static void CreateInstallerDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["InstallerProjectFolderLocation"];
            CopyBinFiles(appLocation, false);
        }

        private static void CreateWavetronicsSpeedListenerDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["WavetronicsSpeedListenerProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }


        private static void CreateWatchDogDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["WatchDogProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateSPMDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["SPMProjectFolderLocation"];
            CopyBinFiles(appLocation, false);
        }

        private static void CreateNewDecodeandImportASC3LogsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["NewDecodeandImportASC3LogsProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateMOEWcfServiceLibraryDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["MOEWcfServiceLibraryProjectFolderLocation"];
            CopyBinFiles(appLocation, false);
        }

        private static void CreateImportCheckerDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["ImportCheckerProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateGenerateAddDataScriptDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["GenerateAddDataScriptProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateTrafficwareLogsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["DecodeTrafficwareLogsProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateDecodeSiemensLogsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["DecodeSiemensLogsProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateDecodePeekLogsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["DecodePeekLogsProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateATSPMAPIDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["ATSPMAPIProjectFolderLocation"];
            CopyBinFiles(appLocation, false);
        }

        private static void CreateParentFolder()
        {
            if (Directory.Exists(ParentFolderLocation))
            {
                Directory.Delete(ParentFolderLocation,true);
            }

            Directory.CreateDirectory(ParentFolderLocation);
        }

        private static void CreateAsyncGetMaxTimeRecordsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["AsyncGetMaxTimeRecordsProjectFolderLocation"];
            CopyBinFiles(appLocation,true);
        }

        private static void CreateGetMaxTimeRecordsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["GetMaxTimeRecordsProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateConvertDBForHistoricalConfigurationsDeploy()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string appLocation = appSettings["ConvertDBForHistoricalConfigurationsProjectFolderLocation"];
            CopyBinFiles(appLocation, true);
        }

        private static void CreateAggregateDeploy()
        {
            //NameValueCollection appSettings = ConfigurationManager.AppSettings;
            //string appLocation = appSettings["AggregateApproachEventProjectLocation"];
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key.StartsWith("Aggregate"))
                {
                    string appLocation = ConfigurationManager.AppSettings[key];
                    CopyBinFiles(appLocation, true);
                }

            }
            
        }

        private static void CopyBinFiles(string appLocation, bool isConsoleApp)
        {
            string tempPath = string.Empty;
            if (isConsoleApp)
            {
#if DEBUG
                tempPath = appLocation + DebugBinDirectory;
#else
                tempPath = appLocation + ReleaseBinDirectory;
#endif
            }
            else
            {
                tempPath = appLocation+"\\";
            }
            Directory.CreateDirectory(ParentFolderLocation + tempPath);
            DirectoryInfo dir = new DirectoryInfo(ProjectFolderLocation + tempPath);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(ProjectFolderLocation + tempPath, ParentFolderLocation + tempPath + file.Name), false);
            }
        }

        private static void BuildApplication(string appParentFolder)
        {
            string[] projects = { ProjectFolderLocation + appParentFolder };
            var p = new Project(ProjectFolderLocation + appParentFolder);
            p.SetGlobalProperty("Configuration", "Release");
            var logger = new List<MySimpleLogger>();
            bool result = p.Build(projects, logger);
        }
    }

    public class MySimpleLogger : Logger
    {
        public override void Initialize(Microsoft.Build.Framework.IEventSource eventSource)
        {
            //Register for the ProjectStarted, TargetStarted, and ProjectFinished events
            eventSource.ProjectStarted += new ProjectStartedEventHandler(eventSource_ProjectStarted);
            eventSource.TargetStarted += new TargetStartedEventHandler(eventSource_TargetStarted);
            eventSource.ProjectFinished += new ProjectFinishedEventHandler(eventSource_ProjectFinished);
        }

        void eventSource_ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            Console.WriteLine("Project Started: " + e.ProjectFile);
        }

        void eventSource_ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            Console.WriteLine("Project Finished: " + e.ProjectFile);
        }
        void eventSource_TargetStarted(object sender, TargetStartedEventArgs e)
        {
            if (Verbosity == LoggerVerbosity.Detailed)
            {
                Console.WriteLine("Target Started: " + e.TargetName);
            }
        }
    }
}
