using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Windows.Interop;

using RestSharp;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Awoo
{
    public class FormLogin
    {
        public string username { get; set;}
        public string password { get; set;}

        public FormLogin(string un, string pwd)
        {
            this.username = un;
            this.password = pwd;
        }
    }

    public class ReplyLogin
    {
        public string reply { get; set;}
        public string token { get; set;}
    }
    public class FormUserFetchByUsername
    {
        public string username { get; set;}
        public FormUserFetchByUsername(string un) { username = un; }
    }
    public class FormUserFetchById
    {
        public string user_id { get; set;}
    }
    public class ReplyUserFetch
    {
        public string id { get; set;}
        public string avatar { get; set;}
        public string intro { get; set;}
        public string status { get; set;}
        public string username { get; set;}
        public string email { get; set;}
        public string lastlogin { get; set;}
        public string reply { get; set;}
    }





    public static class Shared { 
        // move window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg,
                int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static void move_window(System.Windows.Window window) { 
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(window).Handle,
                WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        public static string HOST = "http://localhost:5000/";
        public static T2 sendrecvjson<T1, T2>(string host, string url, T1 obj) where T2 : new() { 
            var client = new RestClient(host);
            var request = new RestRequest(url, Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(obj);

            IRestResponse<T2> response = client.Execute<T2>(request);
            return response.Data;
        }

        public static BitmapImage Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(imageBytes);
            image.EndInit();
            return image;
        }

    }
}
