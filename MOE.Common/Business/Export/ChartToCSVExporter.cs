using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;

namespace MOE.Common.Business.Export
{
    public class ChartToCSVExporter
    {
        public static List<byte[]> ExportChartDataToCSVForDownload(Chart chart)
        {
            try
            {
                List<byte[]> csvList = new List<byte[]>();
                XmlDocument doc = ConvertChartToXML(chart);
                if (doc != null)
                {
                    var seriesCollection = GetSeriesListFromChartXML(doc);

                    csvList.AddRange(GetByteArrayListBySeries(seriesCollection));

                    //List<byte[]> csvList = new List<byte[]>();
                }
                return csvList;
            }
            catch(Exception e)
            {
                throw new Exception("Error on chart: " + chart.Titles[0].Text);
            }

        }

        internal static XmlDocument GetXMLFromChart(Chart chart)
        {
            XmlDocument doc = ConvertChartToXML(chart);
            return doc;
        }

        public static XmlNodeList GetSeriesListFromChartXML(XmlDocument ChartDoc)
        {
            var seriesCollection = ChartDoc.SelectNodes("//Chart/Series/Series");

            return seriesCollection;


        }

        

        public static XmlNodeList GetPointsListFromSeriesNode(XmlNode series)
        {
            var pointsCollection = series.FirstChild;

            return pointsCollection.SelectNodes("DataPoint");


        }

        public static List<byte[]> GetByteArrayListBySeries(XmlNodeList seriesCollection)
        {
            List<byte[]> csvList = new List<byte[]>();

            foreach (XmlNode s in seriesCollection)
            {
                var points = GetPointsListFromSeriesNode(s);
                if (points != null)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(s.Attributes.GetNamedItem("Name").Value.ToString());
                    sb.AppendLine("XValue,YValue");

                    foreach (XmlNode p in points)
                    {
                        try
                        {
                            string XValue = "";
                            string YValue = "";
                            if (p.Attributes.GetNamedItem("XValue") != null)
                            {
                                XValue = p.Attributes.GetNamedItem("XValue").Value.ToString();
                             }

                            if (p.Attributes.GetNamedItem("YValues") != null)
                                {
                                YValue = p.Attributes.GetNamedItem("YValues").Value.ToString();
                            }

                            sb.AppendLine(string.Join(",", XValue, YValue.ToString()));
                        }
                        catch(Exception e)
                        {
                            throw new Exception("Problem getting X or Y values from the XML");
                        }


                    }
                    string x = sb.ToString();

                    csvList.Add(Encoding.UTF8.GetBytes(x));
                }


            }
            return csvList;
        }

        public static string SerializeChart(Chart chart)
        {
            string convertedChart = Newtonsoft.Json.JsonConvert.SerializeObject(chart);
            return convertedChart;
        }

        public static XmlDocument ConvertChartToXML(Chart chart)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                using (var streamWriter = new System.IO.StreamWriter(memoryStream))
                {
                    chart.Serializer.Save(streamWriter);

                   
                    string s = System.Text.UTF8Encoding.UTF8.GetString(memoryStream.ToArray());
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(s);
                    //XDocument doc = XDocument.Parse(s);
                    return doc;
                }
            }
        }


    }


}
