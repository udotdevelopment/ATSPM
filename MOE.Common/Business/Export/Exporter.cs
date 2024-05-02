using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using MOE.Common.Models;
using OfficeOpenXml;

namespace MOE.Common.Business.Export
{
    public class Exporter
    {
        public static byte[] GetCsvFile(IEnumerable records)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(streamWriter))
                {
                    //using (var csv = new CsvWriter(new StreamWriter(filePath)))
                    //{

                    csv.Configuration.RegisterClassMap<PhaseCycleAggregation.ApproachCycleAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachPcdAggregation.ApproachPcdAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachSpeedAggregation.ApproachSpeedAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachSplitFailAggregation.ApproachSplitFailAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<ApproachYellowRedActivationAggregation.ApproachYellowRedActivationAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<DetectorEventCountAggregation.DetectorEventCountAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<PreemptionAggregation.PreemptionAggregationClassMap>();
                    csv.Configuration.RegisterClassMap<PriorityAggregation.PriorityAggregationClassMap>();
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] GetCsvFileForControllerEventLogs(List<Controller_Event_Log> records)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(streamWriter))
                {
                    csv.Configuration.RegisterClassMap<Controller_Event_Log.ControllerEventLogClassMap>();
                    csv.WriteHeader<Controller_Event_Log>();
                    csv.NextRecord();
                    foreach (var record in records)
                    {
                        csv.WriteField(record.SignalID);
                        csv.WriteField(record.Timestamp.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                        csv.WriteField(record.EventCode);
                        csv.WriteField(record.EventParam);
                        csv.NextRecord();
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] ExportSignalConfig(string signalId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fileName = AppDomain.CurrentDomain.BaseDirectory + "ATSPMImport_Template.xlsx";
            var inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var repo = Models.Repositories.SignalsRepositoryFactory.Create();
            Models.Repositories.DetectorRepositoryFactory.Create();
            var signal = repo.GetLatestVersionOfSignalBySignalID(signalId);
            var detectors = signal.GetDetectorsForSignal();

            byte[] fileBytes;
            using (var package = new ExcelPackage(inStream))
            {
                ExportSignalInformation(package.Workbook, signal);
                ExportApproaches(package.Workbook, detectors);
                fileBytes = package.GetAsByteArray();
            }
            return fileBytes;
        }

        private static void ExportSignalInformation(ExcelWorkbook workbook, Signal signal)
        {
            var sheet = workbook.Worksheets["Signal Information"];
            sheet.Cells[4, 2].Value = signal.SignalID;
            sheet.Cells[4, 4].Value = signal.PrimaryName;
            sheet.Cells[4, 6].Value = signal.SecondaryName;
            switch (signal.ControllerType)
            {
                // Add custom text as needed
                default:
                    sheet.Cells[4, 8].Value = signal.ControllerType.Description;
                    break;
            }

            sheet.Cells[5, 2].Value = Convert.ToDouble(signal.Latitude);
            sheet.Cells[5, 4].Value = Convert.ToDouble(signal.Longitude);
            sheet.Cells[5, 6].Value = signal.IPAddress;
        }

        private static void ExportApproaches(ExcelWorkbook workbook, List<Models.Detector> detectors)
        {
            for (var i = 1; i < 9; i++)
            {
                var sheet = workbook.Worksheets[$"Approach {i}"];
                var approachDetectors = detectors.Where(x => x.Approach.ProtectedPhaseNumber == i).ToList();
                if (!approachDetectors.Any()) continue;
                var det = approachDetectors.First();
                sheet.Cells[4, 2].Value = det.Approach.DirectionType.Abbreviation;
                sheet.Cells[5, 2].Value = det.Approach.Description;
                sheet.Cells[4, 4].Value = det.Approach.ProtectedPhaseNumber;
                sheet.Cells[5, 4].Value = det.Approach.PermissivePhaseNumber;
                sheet.Cells[4, 6].Value = det.Approach.IsProtectedPhaseOverlap;
                sheet.Cells[5, 6].Value = det.Approach.IsPermissivePhaseOverlap;
                sheet.Cells[4, 8].Value = det.Approach.MPH;
                sheet.Cells[5, 8].Value = det.Approach.IsPedestrianPhaseOverlap;
                sheet.Cells[4, 10].Value = det.Approach.PedestrianPhaseNumber;
                sheet.Cells[5, 10].Value = det.Approach.PedestrianDetectors;

                var row = 10;
                foreach (var detector in approachDetectors)
                {
                    sheet.Cells[row, 1].Value = detector.DetChannel;
                    if (detector.DetectionTypeIDs.Contains(2))
                        sheet.Cells[row, 2].Value = true;
                    if (detector.DetectionTypeIDs.Contains(3))
                        sheet.Cells[row, 3].Value = true;
                    if (detector.DetectionTypeIDs.Contains(4))
                        sheet.Cells[row, 4].Value = true;
                    if (detector.DetectionTypeIDs.Contains(5))
                        sheet.Cells[row, 5].Value = true;
                    if (detector.DetectionTypeIDs.Contains(6))
                        sheet.Cells[row, 6].Value = true;
                    if (detector.DetectionTypeIDs.Contains(7))
                        sheet.Cells[row, 7].Value = true;
                    sheet.Cells[row, 8].Value = detector.DetectionHardware?.Name;
                    sheet.Cells[row, 9].Value = detector.LatencyCorrection;
                    sheet.Cells[row, 10].Value = detector.LaneNumber;
                    sheet.Cells[row, 11].Value = detector.MovementType?.Description;
                    sheet.Cells[row, 12].Value = detector.LaneType?.Description;
                    sheet.Cells[row, 14].Value = detector.DistanceFromStopBar;
                    sheet.Cells[row, 15].Value = detector.MinSpeedFilter;
                    sheet.Cells[row, 16].Value = detector.DecisionPoint;
                    sheet.Cells[row, 17].Value = detector.MovementDelay;
                    row++;
                }
            }
        }
    }
}