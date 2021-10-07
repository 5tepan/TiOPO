using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace Syte
{
    class Program
    {
        static List<string> GetHref(string html)
        {
            string[] d = html.Split('\n');
            List<string> HREFS = new List<string>();
            string buf = "";
            foreach (string str in d)

            {
                for (int I = 0; I < str.Length - 4; I++)

                {
                    if (str.Substring(I, 4) == "href")

                    {
                        buf = "";
                        I += 6;
                        while (str.Substring(I, 1) != @"""")

                        {
                            buf += str.Substring(I, 1);
                            I++;
                        }
                    }

                    if (Add(HREFS, buf))
                    {
                        HREFS.Add(buf);
                    }
                }
            }
            return HREFS;

        }
        static bool Add(List<string> LIST, string Str)
        {
            bool a = true;
            if (Str.Length > 1)
            {
                string k = Str.Substring(0, 1);
                if (k == "#") return false;

            }
            if (Str != "")
            {
                for (int s = 0; s < LIST.Count; s++)

                    if (LIST[s] == Str) { a = false; break; }
            }

            else
                a = false;
            return a;
        }

        public static string IsPageExists(string url, out string Status)
        {
            Status = "";
            try
            {
                WebClient client = new WebClient();
                client.DownloadString(url);

                return client.DownloadString(url);
            }
            catch (WebException ex)
            {
                Status = ex.Status.ToString();
                return "";
            }
        }

        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("Rezult.txt", false);
            StreamWriter swError = new StreamWriter("RezultError.txt", false);

            int sw_line = 0;
            int swError_line = 0;

            //string url = "https://www.opengl.org/sdk/docs/man2/xhtml/";
            //string url = "https://api.ipify.org/";
            //string url = "http://192.168.30.198/broken-links/";
            // string url = "https://yandex.ru/";
            string url = "http://91.210.252.240/broken-links/";

            string a = IsPageExists(url, out string Status);

            if (!(a == ""))
            {
                sw.WriteLine(url);
                ++sw_line;
                List<string> R = GetHref(a);
                foreach (string s in R)
                {

                    if (!(IsPageExists(s, out Status) == ""))
                    {
                        sw.WriteLine(s);
                        ++sw_line;
                        Console.WriteLine(s);
                    }
                    else
                    {
                        swError.WriteLine("{0}, status {1}", s, Status);
                        ++swError_line;
                        Console.WriteLine("NOT {0}", s);
                    }

                }
            }
            else
            {
                swError.WriteLine("{0}, status {1}", url, Status);
                ++swError_line;
            }
            sw.WriteLine("quantity = {0}, date = {1:D}", sw_line, DateTime.Now);
            swError.WriteLine("quantity = {0}, date = {1}", swError_line, DateTime.Now);
            sw.Close();
            swError.Close();
        }

    }
}
