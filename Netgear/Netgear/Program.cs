using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace Netgear
{
    class Program
    {  
        //public static String ProxyString = "";
        public static string HttpGet(string URI)
        {
            
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
       
        public static string httpget(string url)
        {
            // Create a new webrequest to the mentioned URL.
            System.Net.WebRequest myWebRequest = System.Net.WebRequest.Create(url);

            // Set 'Preauthenticate'  property to true.  Credentials will be sent with the request.
            myWebRequest.PreAuthenticate = true;
     
            //Console.WriteLine("\nPlease Enter ur credentials for the requested Url");
            //Console.WriteLine("UserName");
            string UserName = "admin"; //Console.ReadLine();
            //Console.WriteLine("Password");
            string Password = "capablanca";// Console.ReadLine();

            // Create a New 'NetworkCredential' object.
            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(UserName, Password);

            // Associate the 'NetworkCredential' object with the 'WebRequest' object.
            myWebRequest.Credentials = networkCredential;

            // Assign the response object of 'WebRequest' to a 'WebResponse' variable.
            System.Net.WebResponse myWebResponse = myWebRequest.GetResponse();

            if (myWebResponse == null) return null;

            /* Trying to load the shit into a browser*/
            System.Windows.Forms.WebBrowser wb = new WebBrowser();
            wb.DocumentStream = myWebResponse.GetResponseStream();
            
            System.IO.StreamReader sr = new System.IO.StreamReader(myWebResponse.GetResponseStream());
           
            return wb.DocumentText; //sr.ReadToEnd().Trim();
        }

        /* Trying to use a webbrowser instead Check out: http://awesomium.com/#about*/
        public static void webbrowsermethod(string url) {
            System.Windows.Forms.WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(url);
            while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }
            wb.Document.DomDocument.ToString();

            /* HttpWebRequest - Fake Javascript Enabled */
            //var responseData = "";
            //var strUrl = this.QuerySelector(item, "a[class='url']").Attributes["href"].Value;

            //request = (HttpWebRequest) WebRequest.Create(strUrl);
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = 0;
            //request.CookieContainer = cookies;
            //request.Timeout = System.Threading.Timeout.Infinite;
            //request.UserAgent = this.RefreshUserAgent();
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            //request.KeepAlive = true;
            //request.AllowAutoRedirect = false;
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            //response = (HttpWebResponse)request.GetResponse();
            //response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            //var encoding = new System.Text.UTF8Encoding();
            //var responseReader = new StreamReader(response.GetResponseStream(), encoding, true);

            //responseData = responseReader.ReadToEnd();
            //response.Close();
            //responseReader.Close();
        }

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            
            
            string output = httpget("http://192.168.1.1/");
            Console.WriteLine(output);
            //"Basic YWRtaW46Y2FwYWJsYW5jQ=="
            //HttpPost("http://192.168.1.1/start.htm", "capablanca");
           // Console.WriteLine(HttpGet("http://192.168.1.1/start.htm"));
            
            Console.WriteLine("Hello World!");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
