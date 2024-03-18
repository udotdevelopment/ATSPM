//using MOE.Common.Business;
//using AtspmApi.Models;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Custom;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace AtspmApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class PowerToolsController : ApiController
    {
        /// <summary>
        /// Returns a list of Split Monitor details based on a date range and comma separated string list of signal Ids
        /// </summary>
        /// <param name="signalIds">List of signal Ids separated by commas</param>
        /// <param name="startDate">Start date for date range</param>
        /// <param name="endDate">End date for date range</param>
        /// <returns>CSV file of Split Monitor data</returns>
        [HttpGet]
        [Route("GetSplitMonitor")]
        public IHttpActionResult GetSplitMonitor(string signalIds, DateTime startDate, DateTime endDate)
        {
            string[] signals = signalIds.Split(',');
            if (signals.Length < 1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Invalid Signals range. Please enter valid Signal Ids separated by commas.")
                });
            }
            if (startDate > endDate)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Invalid date range. Start date cannot be after the end date.")
                });
            }
            //Default to the very end of the date selected.
            endDate = endDate.AddDays(1).AddSeconds(-1);
            try
            {
                List<SplitMonitorSummary> splitMonitorSummaries = new List<SplitMonitorSummary>();
                foreach (string signal in signals)
                {
                    var splitMonitorOptions = new SplitMonitorOptions();
                    splitMonitorOptions.SignalID = signal.Trim();
                    splitMonitorOptions.StartDate = startDate;
                    splitMonitorOptions.EndDate = endDate;
                    splitMonitorOptions.MetricTypeID = 35;
                    splitMonitorSummaries.AddRange(splitMonitorOptions.CreateMeticDataWithoutGraph());
                }

                if (!splitMonitorSummaries.Any())
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No data found using parameters entered.")
                    });
                }

                splitMonitorSummaries.OrderBy(m => m.SignalId).ThenBy(m => m.Date).ThenBy(m => m.Phase).ThenBy(m => m.Plan);

                var response = ExportModelToCSV(splitMonitorSummaries, "SplitMonitorData.csv");
                return response;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Unable to return Split Monitor data based on parameters.")
                });
            }
        }

        /// <summary>
        /// Returns a list of Purdue Split Failure details based on a date range and comma separated string list of signal Ids
        /// </summary>
        /// <param name="signalIds">List of signal Ids separated by commas</param>
        /// <param name="startDate">Start date for date range</param>
        /// <param name="endDate">End date for date range</param>
        /// <returns>CSV file of Purdue Split Failure data</returns>
        [HttpGet]
        [Route("GetPurdueSplitFailure")]
        public IHttpActionResult GetPurdueSplitFailure(string signalIds, DateTime startDate, DateTime endDate)
        {
            string[] signals = signalIds.Split(',');
            if (signals.Length < 1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Invalid Signals range. Please enter valid Signal Ids separated by commas.")
                });
            }

            if (startDate > endDate)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Invalid date range. Start date cannot be after the end date.")
                });
            }
            //Default to the very end of the date selected.
            endDate = endDate.AddDays(1).AddSeconds(-1);
            try
            {
                List<SplitFailureSummary> splitFailureSummaries = new List<SplitFailureSummary>();
                foreach (var signal in signals)
                {
                    var splitFailOptions = new SplitFailOptions();
                    splitFailOptions.SignalID = signal.Trim();
                    splitFailOptions.StartDate = startDate;
                    splitFailOptions.EndDate = endDate;
                    splitFailureSummaries.AddRange(splitFailOptions.CreateMetricWithoutGraph());
                }

                if (!splitFailureSummaries.Any())
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No data found using parameters entered.")
                    });
                }

                splitFailureSummaries.OrderBy(m => m.SignalId).ThenBy(m => m.Date).ThenBy(m => m.Phase).ThenBy(m => m.Plan);

                var response = ExportModelToCSV(splitFailureSummaries, "PurdueSplitFailureData.csv");
                return response;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Unable to return Purdue Split Failure data based on parameters.")
                });
            }
        }

        /// <summary>
        /// Returns a list of Purdue Coordination Diagram details based on a date range and comma separated string list of signal Ids
        /// </summary>
        /// <param name="signalIds">List of signal Ids separated by commas</param>
        /// <param name="startDate">Start date for date range</param>
        /// <param name="endDate">End date for date range</param>
        /// <returns>CSV file of Purdue Coordination Diagram data</returns>
        [HttpGet]
        [Route("GetPurdueCoordinationDiagram")]
        public IHttpActionResult GetPurdueCoordinationDiagram(string signalIds, DateTime startDate, DateTime endDate)
        {
            string[] signals = signalIds.Split(',');
            if (signals.Length < 1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Invalid Signals range. Please enter valid Signal Ids separated by commas.")
                });
            }

            if (startDate > endDate)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Invalid date range. Start date cannot be after the end date.")
                });
            }
            //Default to the very end of the date selected.
            endDate = endDate.AddDays(1).AddSeconds(-1);
            try
            {
                List<PCDSummary> pcdSummaries = new List<PCDSummary>();
                foreach (var signal in signals)
                {
                    var pcd = new PCDOptions();
                    pcd.SignalID = signal.Trim();
                    pcd.StartDate = startDate;
                    pcd.EndDate = endDate;
                    pcdSummaries.AddRange(pcd.CreateMetricWithoutGraph());
                }

                if (!pcdSummaries.Any())
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No data found using parameters entered.")
                    });
                }

                pcdSummaries.OrderBy(m => m.SignalId).ThenBy(m => m.Date).ThenBy(m => m.Phase).ThenBy(m => m.Plan);

                var response = ExportModelToCSV(pcdSummaries, "PurdueCoordinationDiagramData.csv");
                return response;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Unable to return Purdue Coordination Diagram data based on parameters.")
                });
            }
        }

        private ResponseMessageResult ExportModelToCSV<T>(List<T> model, string fileName)
        {
            var csv = CsvSerializer.SerializeToCsv<T>(model);

            byte[] bytes = Encoding.ASCII.GetBytes(csv);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = ResponseMessage(result);

            return response;
        }
    }
}