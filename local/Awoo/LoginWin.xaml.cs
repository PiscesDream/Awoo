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

using RestSharp;

namespace Awoo
{
    /// <summary>
    /// LoginWin.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWin : Window
    {
        public LoginWin()
        {
            InitializeComponent();
            hostinput.Text = Shared.HOST;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            ReplyLogin res = new ReplyLogin();
            try
            {
                Shared.HOST = hostinput.Text;
                res =
                    Shared.sendrecvjson<FormLogin, ReplyLogin>
                    (Shared.HOST, "/api/login", new FormLogin(UnInput.Text, PwdInput.Password));
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot login, please connect with your service provider.", "Error");
            }


            MessageBox.Show(res.reply);
            if (res.reply == "logged in") {
                MainWin mainwin = new MainWin(UnInput.Text, res.token);
                mainwin.Show();
                this.Close();
            } 
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://Awoo.hapd.info");
        }
    }
}
