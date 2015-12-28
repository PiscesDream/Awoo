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
using System.IO;


namespace Awoo
{
    /// <summary>
    /// MainWin.xaml 的交互逻辑
    /// </summary>
    public partial class MainWin : Window
    {
        public string id;
        public string avatar;
        public string intro;
        public string status;
        public string username;
        public string email;
        public string lastlogin;
        public string token;
        public MainWin(string un, string tk)
        {
            username = un;
            token = tk;

            InitializeComponent();
            __init__();
        }
        public void __init__()
        {
            ReplyUserFetch res = 
                Shared.sendrecvjson<FormUserFetchByUsername, ReplyUserFetch>
                (Shared.HOST, "/api/user/fetch/username", new FormUserFetchByUsername(username));
            if (res.reply != "succeed") { MessageBox.Show(res.reply, "Error"); this.Close(); }

            id = res.id;
            avatar = res.avatar;
            intro = res.intro;
            status = res.status;
            email = res.email;
            lastlogin = res.lastlogin;

            Avatar.Source = Shared.Base64ToImage(avatar);
        }

        private void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
