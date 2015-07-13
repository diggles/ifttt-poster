using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Ifttt_Poster
{
    class Program
    {
        private static void Main(string[] args)
        {
            #region Global config

            // Load global config 
            dynamic global;
            try
            {
                global = LoadConfig("config.txt");

                if (global.url == null || global.key == null)
                {
                    Log("Global config must have a url and key field");
                    return;
                }

            }
            catch (Exception ex)
            {
                Log("Couldn't load global config. " + ex.Message);
                return;
            }
            #endregion 

            #region Data config

            // Load data to send from file or args
            dynamic data;
            try
            {
                data = args.Length == 0 ? LoadConfig("data.txt") : JsonConvert.DeserializeObject(args[0]);

                if (data.trigger == null || data.data == null)
                {
                    Log("Data config must have a trigger and data field");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log("Couldn't load data config. " + ex.Message);
                return;
            }
            #endregion

            #region Trigger config

            // Load trigger config for this data
            dynamic trigger;
            try
            {
                trigger = LoadConfig(string.Format("{0}.txt", data.trigger));

                if (trigger.method == null || trigger.mime == null)
                {
                    Log("Trigger config must have a method and mime field");
                }
            }
            catch (Exception ex)
            {
                Log("Couldn't load trigger config. " + ex.Message);
                return;
            }
            #endregion

            string method = trigger.method;
            string content = JsonConvert.SerializeObject(data.data);
            string contentType = trigger.mime;

            // Convert the content to bytes
            byte[] b = Encoding.UTF8.GetBytes(content);

            Uri uri = new Uri(string.Format((string)global.url, data.trigger, global.key));
            
            // Build the request
            WebRequest wr = WebRequest.Create(uri);
            wr.Method = method;
            wr.ContentType = contentType;
            wr.ContentLength = b.Length;
            
            // Make the request
            Stream s = wr.GetRequestStream();
            s.Write(b, 0, b.Length);
            s.Close();

            // Get the response
            WebResponse response = wr.GetResponse();

            // Get the stream containing content returned by the server.
            s = response.GetResponseStream();
            StreamReader reader = new StreamReader(s);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Display the content.
            Log(responseFromServer);

        }

        private static void Log(string message)
        {
            Console.Out.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), message);
        }

        private static dynamic LoadConfig(string name)
        {
            StreamReader reader = File.OpenText(name);

            return JsonConvert.DeserializeObject(reader.ReadToEnd());
        }
    }
}
