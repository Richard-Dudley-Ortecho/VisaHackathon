using System;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GP
{
    public class GoogleAPIClient
    {
        private void LogRequest(string url, string requestBody)
        {
            UnityEngine.Debug.Log("Logging request");
            UnityEngine.Debug.Log(url);
            UnityEngine.Debug.Log(requestBody);
        }

        private void LogResponse(string info, HttpWebResponse response)
        {
            string responseBody;
            UnityEngine.Debug.Log("Logging response");
            UnityEngine.Debug.Log(info);
            UnityEngine.Debug.Log("Response Status: \n" + response.StatusCode);
            UnityEngine.Debug.Log("Response Headers: \n" + response.Headers.ToString());

            using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                responseBody = reader.ReadToEnd();
            }

            UnityEngine.Debug.Log("Response Body: \n" + responseBody);
        }

        public Tuple<string, string> DoMutualAuthCall(string path, string method, string testInfo, string requestBodyString, Dictionary<string, string> headers = null)
        {
            string requestURL = "https://maps.googleapis.com" + path;
            string APIkey = "Place API Key Here";
            string statusCode = "";
            LogRequest(requestURL, requestBodyString);

            // Create the request object 
            HttpWebRequest request = WebRequest.Create(requestURL) as HttpWebRequest;
            request.Method = method;
            if (method.Equals("POST") || method.Equals("PUT"))
            {
                request.ContentType = "application/json";
                request.Accept = "application/json";
                // Load the body for the post request
                var requestStringBytes = Encoding.UTF8.GetBytes(requestBodyString);
                request.GetRequestStream().Write(requestStringBytes, 0, requestStringBytes.Length);
            }

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            try
            {
                // Make the call
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //LogResponse(testInfo, response);
                    statusCode = response.StatusCode.ToString();
                    using (var sr = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseBody = sr.ReadToEnd();
                        UnityEngine.Debug.Log("Response body: " + responseBody);
                        return new Tuple<string, string>(statusCode, responseBody);
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse)
                {
                    HttpWebResponse response = (HttpWebResponse)e.Response;
                    LogResponse(testInfo, response);
                    statusCode = response.StatusCode.ToString();

                    return new Tuple<string, string>(statusCode, null);
                }
            }

            return null;
        }
    }
}
