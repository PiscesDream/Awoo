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
using System.Windows.Media;

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
    public class FormRegister
    {
        public string username { get; set;}
        public string password { get; set;}
        public string email { get; set;}
        public FormRegister(string em, string un, string psw) { email = em; username = un; password = psw; }
    }
    public class FormUpdate
    {
        public string username { get; set;}
        public string token { get; set;}
        public string intro { get; set;}
        public string password { get; set;}
        public string avatar { get; set;}
        public FormUpdate(string un, string tk, string into, string pwd, string av) { username = un; token = tk; intro = into; password = pwd; avatar = av; }
    }








    public static class TypeText
    {
        public const string header = "";
        public static Label parse(String s, bool rightalign=false)
        {
            Label label = new Label();
            label.Content = s; 
            label.Content = /*"RawText:"+*/s.Substring(header.Length);
            if (rightalign) label.HorizontalContentAlignment = HorizontalAlignment.Right;
            Shared.renderLabel(label);
            return label;
        }
    }
    public static class TypeTextRaw
    {
        public const string header = "{$Text.Raw}";
        public static Label parse(String s, bool rightalign=false)
        {
            Label label = new Label();
            label.Content = /*"RawText:"+*/s.Substring(header.Length);
            if (rightalign) label.HorizontalContentAlignment = HorizontalAlignment.Right;
            Shared.renderLabel(label);
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
                Shared.renderLabel(label);
                return label; 
            }
        }
    }



    public static class Shared {
        public static Config config;
        public static string configpath;
        public static GlobalConfig globalconfig;
        public static string globalconfigpath = @"./global_conf.xml";
        public static void renderLabel(Label label)
        {
            if (!object.ReferenceEquals(Shared.config, null))
            {
                label.Foreground = new SolidColorBrush(Shared.config.chatwinfontcolor);
                label.FontSize = Shared.config.chatwinfontsize;
                FontFamilyConverter ffc = new FontFamilyConverter();
                label.FontFamily = (FontFamily)ffc.ConvertFromString(Shared.config.chatwinfontfamily);
            }
        }



        // move window
        public const int WM_NCLBUTTONDOWN = 0xA1; // 消息编号
        public const int HT_CAPTION = 0x2; // 参数
        [DllImportAttribute("user32.dll")] // 加载DLL
        public static extern int SendMessage(IntPtr hWnd, int Msg,
                int wParam, int lParam); // 引用外部符号
        [DllImportAttribute("user32.dll")] // 加载DLL
        public static extern bool ReleaseCapture(); // 引用外部符号

        public static void move_window(System.Windows.Window window) { // 处理函数
            ReleaseCapture(); // 释放对鼠标的捕捉
            SendMessage(new WindowInteropHelper(window).Handle,
                WM_NCLBUTTONDOWN, HT_CAPTION, 0); // 发送一个消息给窗体的句柄
        }

        public static string HOST = "http://localhost:5000/";
        //public static string HOST = "http://awoo.hapd.info";

        public static T2 sendrecvjson<T1, T2>(string host, string url, T1 obj) where T2 : new() { 
            // 建立连接
            var client = new RestClient(host);
            // 设置传送格式
            var request = new RestRequest(url, Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            // 添加传送内容
            request.AddBody(obj);
            // 开始传送
            IRestResponse<T2> response = client.Execute<T2>(request);
            // 返回结果
            return response.Data;
        }


        private static System.Drawing.Bitmap ResizeBitmap(System.Drawing.Bitmap sourceBMP, int width, int height)
        {
            System.Drawing.Bitmap result = new System.Drawing.Bitmap(width, height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }

        public static BitmapImage Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            // 将字符串转换成字节流
            byte[] imageBytes = Convert.FromBase64String(base64String);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            // 将字节流导入内存，再赋值给图片
            image.StreamSource = new MemoryStream(imageBytes);
            image.EndInit();
            return image;
        }
        public static String FileToBase64(string filename, bool resize=false)
        {
            string base64;
            // 读入文件并压缩大小
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(filename);
            if (resize) bm = ResizeBitmap(bm, 64, 64); {
                using (MemoryStream ms = new MemoryStream()) {
                    // 将bitmap中的信息保存到内存流里
                    bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    // 将内存流转化成字符串
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




    public class GlobalConfig
    {
        public string lastHOST;
        public string lastUser;
        static public void save(GlobalConfig config)
        {
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(GlobalConfig));
            var wfile = new System.IO.StreamWriter(Shared.globalconfigpath);
            writer.Serialize(wfile, config);
            wfile.Close();
        }
        static public GlobalConfig load()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(GlobalConfig));
            System.IO.StreamReader file = new System.IO.StreamReader(Shared.globalconfigpath);
            GlobalConfig config = (GlobalConfig)reader.Deserialize(file);
            file.Close();
            return config;
        }
    }
    public class Config
    {
        public Color chatwinfontcolor;
        public float chatwinfontsize;
        public string chatwinfontfamily;
        static public void save(Config config)
        {
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            var wfile = new System.IO.StreamWriter(Shared.configpath);
            writer.Serialize(wfile, config);
            wfile.Close();
        }
        static public Config load()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Config));
            System.IO.StreamReader file = new System.IO.StreamReader(Shared.configpath);
            Config config = (Config)reader.Deserialize(file);
            file.Close();
            return config;
        }
    }



}
