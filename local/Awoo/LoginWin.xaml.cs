using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.Windows.Interop;

using RestSharp;

namespace Awoo
{
    public class LoginForm
    {
        public string username { get; set;}
        public string password { get; set;}

        public LoginForm(string un, string pwd)
        {
            this.username = un;
            this.password = pwd;
        }
    }
    public class LoginRespond
    {
        public string reply { get; set;}
        public string token { get; set;}
    }



    /// <summary>
    /// LoginWin.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWin : Window
    {
        public LoginWin()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // move window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg,
                int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public void move_window(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle,
                WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("http://localhost:5000/");
            var request = new RestRequest("/api/login", Method.POST);

            //request.AddHeader("Content-type", "application/json; charset=utf-8");
            request.RequestFormat = RestSharp.DataFormat.Json;
            LoginForm loginform = new LoginForm(UnInput.Text, PwdInput.Password);
            request.AddBody(loginform);

            IRestResponse<LoginRespond> response = client.Execute<LoginRespond>(request);
            MessageBox.Show(response.Data.reply);
            if (response.Data.reply == "logged in")
            {

            } 
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://Awoo.hapd.info");
        }
    }
}
