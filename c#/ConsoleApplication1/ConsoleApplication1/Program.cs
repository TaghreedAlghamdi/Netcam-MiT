using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace WeMoSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            string IPADDR = "10.68.68.101";
            string PORT = "49153";
            string TARGETSTATUS = "0";

            HttpWebRequest req = WebRequest.Create("http://" + IPADDR + ":" + PORT + "/upnp/control/basicevent1") as HttpWebRequest;
            string reqContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            reqContent += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
            reqContent += "<s:Body>";
            reqContent += "<u:SetBinaryState xmlns:u=\"urn:Belkin:service:basicevent:1\">";
            reqContent += "<BinaryState>"+TARGETSTATUS+"</BinaryState>";
            reqContent += "</u:SetBinaryState>";
            reqContent += "</s:Body>";
            reqContent += "</s:Envelope>";
            UTF8Encoding encoding = new UTF8Encoding();

            req.ContentType = "text/xml; charset=\"utf-8\"";
            req.Headers.Add("SOAPACTION:\"urn:Belkin:service:basicevent:1#SetBinaryState\"");
            req.Method = "POST";

            using (Stream requestStream = req.GetRequestStream())
            {
                requestStream.Write(encoding.GetBytes(reqContent), 0,
                    encoding.GetByteCount(reqContent));
            }

            try
            {
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                using (Stream rspStm = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(rspStm))
                    {
                        Console.WriteLine("Response Description: " + response.StatusDescription);
                        Console.WriteLine("Response Status Code: " + response.StatusCode);
                        Console.WriteLine("responseBody: " + reader.ReadToEnd());
                    }
                }
                Console.WriteLine("Success: " + response.StatusCode.ToString());
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine("Exception message: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.Status);
                
            }
            while (!Console.KeyAvailable) System.Threading.Thread.Sleep(500);



        }
    }
}
