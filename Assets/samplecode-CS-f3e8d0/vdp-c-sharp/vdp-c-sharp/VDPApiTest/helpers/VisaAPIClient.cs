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

namespace Vdp
{
    public class VisaAPIClient
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

        //Correlation Id ( ex-correlation-id ) is an optional header while making an API call. You can skip passing the header while calling the API's.
        private string GetCorrelationId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray()) + "_SC";

        }

        private string GetBasicAuthHeader(string userId, string password)
        {
            string authString = userId + ":" + password;
            var authStringBytes = Encoding.UTF8.GetBytes(authString);
            string authHeaderString = Convert.ToBase64String(authStringBytes);
            return "Basic " + authHeaderString;
        }

        public Tuple<string, string> DoMutualAuthCall(string path, string method, string testInfo, string requestBodyString, Dictionary<string, string> headers = null)
        {
            string requestURL = "https://sandbox.api.visa.com/" + path;
            string userId = "1QF5U0CK7H8PAIZRTOS721MIJS6xUplNOqh0IOrw8mZ-ZCDMg";
            string password = "x3U9OSADgN37IAEcK1ul";
            string certificatePath = "./Assets/samplecode-CS-f3e8d0/security/p12certfile.p12";
            string certificatePassword = "abc";
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
            
            // Add headers
            request.Headers["Authorization"] = GetBasicAuthHeader(userId, password);
            request.Headers["ex-correlation-id"] = GetCorrelationId();
            // Add certificate
            var certificate = new X509Certificate2(certificatePath, certificatePassword);
            request.ClientCertificates.Add(certificate);

            try
            {
                // Make the call
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    statusCode = response.StatusCode.ToString();
                    using (var sr = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII)) {
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
                    statusCode = response.StatusCode.ToString();

                    return new Tuple<string, string>(statusCode, null);
                }
            }

            return null;
        }
    }
}
