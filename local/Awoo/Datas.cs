using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

    public class FormUserFetchFriends
    {
        public string username { get; set;}
        public string token { get; set;}
        public FormUserFetchFriends(string un, string tk) { username = un; token = tk; }
    }
    public class ReplyUserFetchFriends
    {
        public string reply { get; set; }
        public List<string>friends {get;set;}
    }

    public class FormMsgFetch
    {
        public string username { get; set;}
        public string fusername { get; set;}
        public string token { get; set;}
        public FormMsgFetch(string un, string tk, string fun) { username = un; token = tk; fusername = fun; }
    }
    public class ReplyMsgFetchUnit
    {
        public string sender_id { get; set; }
        public string sender { get; set; }
        public string timestamp { get; set; }
        public string content { get; set; }
    }
    public class ReplyMsgFetch
    {
        public string reply { get; set; }
        public List<ReplyMsgFetchUnit> messages{get;set;}
    }

    public class FormMsgSend
    {
        public string username { get; set;}
        public string token { get; set;}
        public string receiver { get; set;}
        public string message { get; set;}
        public FormMsgSend(string un, string tk, string recv, string msg) { username = un; token = tk; receiver = recv; message = msg; }
    }
    public class Reply
    {
        public string reply { get; set; }
    }

    public class FormMsgNotice
    {
        public string username { get; set;}
        public string token { get; set;}
        public FormMsgNotice(string un, string tk) { username = un; token = tk; }
    }
    public class ReplyMsgNotice: Reply
    {
        public List<String> usernames{ get; set; }
    }

    public class FormUserQuery
    {
        public string username { get; set;}
        public string token { get; set;}
        public string query { get; set;}
        public FormUserQuery(string un, string tk, string qu) { username = un; token = tk; query = qu; }
    }
    public class ReplyUserAll: Reply
    {
        public List<String> usernames{ get; set; }
    }






    public static class TypeTextRaw
    {
        public const string header = "{$Text.Raw}";
        public static Label parse(String s)
        {
            Label label = new Label();
            label.Content = "RawText:"+s.Substring(header.Length);
            return label;
        }
    }
    public static class TypeImgBase64
    {
        public const string header = "{$Img.Base64}";
        public static UIElement parse(String s)
        {
            try
            {
                Image img = new Image();
                img.Source = Shared.Base64ToImage(s.Substring(header.Length));
                return img;
            }
            catch
            {
                MessageBox.Show("Image Base64 parse error.");
                Label label = new Label();
                label.Content = "Image Base64 parse error.";
                return label; 
            }
        }
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

        //public static string HOST = "http://localhost:5000/";
        public static string HOST = "http://awoo.hapd.info";
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

        public static String FileToBase64(string filename)
        {
            string base64;
            using (System.Drawing.Bitmap bm = new System.Drawing.Bitmap(filename))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    base64 = Convert.ToBase64String(ms.ToArray());
                }
            }
            return base64;
        }

        public static String BitmapToBase64(System.Drawing.Bitmap bm)
        {
            string base64;
            using (MemoryStream ms = new MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                base64 = Convert.ToBase64String(ms.ToArray());
            }
            return base64;
        }


    }
}
