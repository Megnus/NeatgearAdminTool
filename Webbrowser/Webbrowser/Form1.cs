using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace Webbrowser
{
    public partial class Form1 : Form
    {
        const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        const int SET_FEATURE_ON_PROCESS = 0x00000002;

        [System.Runtime.InteropServices.DllImport("urlmon.dll")]
        [System.Runtime.InteropServices.PreserveSig]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(
            int FeatureEntry,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] int dwFlags,
            bool fEnable);

        static void DisableClickSounds()
        {
            CoInternetSetFeatureEnabled(
                FEATURE_DISABLE_NAVIGATION_SOUNDS,
                SET_FEATURE_ON_PROCESS,
                true);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DisableClickSounds();
            webBrowser1.Document.GetElementById("action_Refresh").InvokeMember("click");
           // webBrowser1.Update();
            //System.Threading.Thread.Sleep(5000);
           
            //HtmlElement tb = webBrowser1.Document.GetElementById("TextBox1");
            //tb.InnerText = "QWERTY";
            //webBrowser1.DocumentText;
            //textBox1.Text = webBrowser1.DocumentText;
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                //MessageBox.Show(ip.ToString());
                
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().Contains("192.168.1"))
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            return localIP;

        }

        public List<String> ListNetworkComputers()
        {
            List<String> _ComputerNames = new List<String>();
            String _ComputerSchema = "Computer";
            System.DirectoryServices.DirectoryEntry _WinNTDirectoryEntries = new System.DirectoryServices.DirectoryEntry("WinNT:");
            foreach (System.DirectoryServices.DirectoryEntry _AvailDomains in _WinNTDirectoryEntries.Children)
            {
                foreach (System.DirectoryServices.DirectoryEntry _PCNameEntry in _AvailDomains.Children)
                {
                    if (_PCNameEntry.SchemaClassName.ToLower().Contains(_ComputerSchema.ToLower()))
                    {
                        _ComputerNames.Add(_PCNameEntry.Name);
                    }
                }
            }
            return _ComputerNames;
        }

        public void getHostNames() 
        {
            // Get host name from current network
           /* String strHostName = Dns.GetHostName();
            List<string> machinesName = new List<string>();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // Enumerate IP addresses from current network
            int nIP = 0;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                var host = Dns.GetHostByAddress(ipaddress);//ip Address is to be specified machineName
                //Add machine name into the list        
                machinesName.Add(host.HostName);
                //Form1.textBox1.ScrollToCaret();
                textBox2.Text += host.HostName;
            }
            */

            List<string> list = new List<string>();
            using (System.DirectoryServices.DirectoryEntry root = new System.DirectoryServices.DirectoryEntry("WinNT:"))
            {
                foreach (System.DirectoryServices.DirectoryEntry computers in root.Children)
                {
                    foreach (System.DirectoryServices.DirectoryEntry computer in computers.Children)
                    {
                        if ((computer.Name != "Schema"))
                        {
                            list.Add(computer.Name);
                            textBox2.Text += computer.Name;
                        }
                    }
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(LocalIPAddress());
            // Something...
            getHostNames();
        }

        public static string RemoveFirstLines(string text, int linesCount)
        {
            // Better select(x => x.Contains("ALLOW"));
            
            var lines = System.Text.RegularExpressions.Regex.Split(text, "\r\n|\r|\n").Skip(linesCount);

            int num = lines.Count(x => x.IndexOf("ALLOW") >= 0);

            string localIpAddr = LocalIPAddress();

            lines = System.Text.RegularExpressions.Regex.Split(text, "\r\n|\r|\n").Take(num);// Skip(linesCount);

            lines = lines.Where(x => !x.Contains(localIpAddr));

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox1.Text = webBrowser1.DocumentText;
            string sub = textBox1.Text.Substring(textBox1.Text.IndexOf("ALLOW") - 1);//("[ALLOW"));
            string subb = RemoveFirstLines(sub, 0);
            //string sub = RemoveFirstLines(textBox1.Text, 78);//.Substring(textBox1.Text.IndexOf("TEXTAREA NAME="));
            textBox1.Text = subb;
            textBox1.SelectionStart = textBox1.TextLength;
            //scroll to the caret
            textBox1.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(new Uri("http://admin:capablanca@192.168.1.1/FW_log.htm"));
            //textBox1.Text = webBrowser1.DocumentText;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Navigate(new Uri("http://admin:capablanca@192.168.1.1/FW_log.htm"));
            //timer1.Enabled = false;
            textBox1.SelectionStart = textBox1.TextLength;
            //scroll to the caret
            textBox1.ScrollToCaret();
            //Console.Beep();
            //webBrowser1.Update();
            //webBrowser1.Document.GetElementById("action_Refresh").InvokeMember("click");
        }


    }
}
