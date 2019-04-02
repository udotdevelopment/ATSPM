using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;

namespace MOE.Common.Business
{
    public class MaxTimeHDLogClient
    {
        public XmlDocument GetFull(string url)
        {
            var xml = new XmlDocument();
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 500);
                client.BaseAddress = new Uri(url + "/v1/asclog/xml/full");
                client.DefaultRequestHeaders.Accept.Clear();
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var response = new HttpResponseMessage();
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (AggregateException e)
                {
                    //stuffhere
                }

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    var dataObjects = response.Content.ReadAsStringAsync().Result;
                    xml.LoadXml(dataObjects);
                }
                return xml;
            }
        }

        public XmlDocument GetSince(string url, DateTime since)
        {
            var xml = new XmlDocument();
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 500);

                client.BaseAddress = new Uri("http://" + url + "/v1/asclog/xml/full?since=" +
                                             since.ToString("MM-dd-yyyy HH:mm:ss.f"));
                //client.BaseAddress = new Uri("http://" + url + "/v1/asclog/xml/hourly/current?type=xml&since=" + since.ToString("MM-dd-yyyy HH:mm:ss.f"));
                client.DefaultRequestHeaders.Accept.Clear();
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var response = new HttpResponseMessage();
                try
                {
                    response = client.GetAsync(client.BaseAddress).Result;
                }
                catch (AggregateException e)
                {
                    //stuffhere
                }

                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    // Parse the response body. Blocking!
                    var dataObjects = response.Content.ReadAsStringAsync().Result;
                    xml.LoadXml(dataObjects);
                }
                return xml;
            }
        }
    }
}