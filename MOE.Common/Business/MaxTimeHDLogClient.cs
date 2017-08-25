using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;

namespace MOE.Common.Business
{
    public class MaxTimeHDLogClient
    {
        public  MaxTimeHDLogClient()
        {

        }

        public  XmlDocument GetFull(string url)
        {
            XmlDocument xml = new XmlDocument();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 500);
                client.BaseAddress = new Uri(url + "/v1/asclog/xml/full");
                client.DefaultRequestHeaders.Accept.Clear();
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = new HttpResponseMessage();
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
                    string dataObjects = response.Content.ReadAsStringAsync().Result;
                    xml.LoadXml(dataObjects);
                }
                return xml;
            }
        }

            public  XmlDocument GetSince(string url, DateTime since)
        {
            XmlDocument xml = new XmlDocument();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0,0,500);

                client.BaseAddress = new Uri("http://" + url + "/v1/asclog/xml/full?since=" + since.ToString("MM-dd-yyyy HH:mm:ss.f"));
                //client.BaseAddress = new Uri("http://" + url + "/v1/asclog/xml/hourly/current?type=xml&since=" + since.ToString("MM-dd-yyyy HH:mm:ss.f"));
                client.DefaultRequestHeaders.Accept.Clear();
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = new HttpResponseMessage();
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
                    string dataObjects = response.Content.ReadAsStringAsync().Result;
                    xml.LoadXml(dataObjects);
                }
                return xml;
            }
    }
}
}
