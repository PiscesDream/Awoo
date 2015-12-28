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
            ReplyLogin response = 
                Shared.sendrecvjson<FormLogin, ReplyLogin>
                (Shared.HOST, "/api/login", new FormLogin(UnInput.Text, PwdInput.Password));

            MessageBox.Show(response.reply);
            if (response.reply == "logged in") {
                MainWin mainwin = new MainWin(UnInput.Text, response.token);
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
