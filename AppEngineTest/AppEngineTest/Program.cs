using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // send request
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8888/_ah/api/menuendpoint/v1/menu");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://lunch-viewer.appspot.com/_ah/api/menuendpoint/v1/menu");
            //request.Method = "GET";
            //request.ContentType = "text/xml; encoding=utf-8";

            //// get response
            //WebResponse response = request.GetResponse();
            //using (var responseStream = response.GetResponseStream())
            //using (var responseReader = new StreamReader(responseStream))
            //{
            //    string data = responseReader.ReadToEnd();
            //    Console.WriteLine(data);
            //    //File.WriteAllText(@"C:\Private\GitHub\Projects\AppEngineTest\AppEngineTest\bin\Debug\text.txt", data);
            //}

            
            //http://stackoverflow.com/questions/4535840/deserialize-json-object-into-dynamic-object-using-json-net
            //http://stackoverflow.com/questions/14850465/error-when-trying-to-deserialize-json-string-with-json-net
            //String text = File.ReadAllText(@"C:\Private\GitHub\Projects\AppEngineTest\AppEngineTest\bin\Debug\text.txt");
            //dynamic menus = JObject.Parse(text);
            
            //foreach (var menu in menus.items)
            //{
            //    Console.WriteLine(menu.year + " " + menu.week);
            //    foreach (var item in menu.items)
            //    {
            //        Console.WriteLine("   " + item.date + " " + item.text + " " + item.subText);
            //    }
            //}

            String text = File.ReadAllText(@"C:\Private\GitHub\Projects\AppEngineTest\AppEngineTest\bin\Debug\text.txt");
            var session = (Session)JsonConvert.DeserializeObject(text, typeof(Session));

            foreach (var menu in session.items)
            {
                Console.WriteLine(string.Format("Year {0}, week {1}", menu.year, menu.week));
                foreach (var menu_item in menu.items)
                {
                    Console.WriteLine(string.Format("{0} - {1} {2}", menu_item.date.ToShortDateString(), menu_item.text, menu_item.subText));
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
