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
using System.Timers;

namespace Awoo
{
    /// <summary>
    /// ChatWin.xaml 的交互逻辑
    /// </summary>
    public partial class ChatWin : Window
    {
        public string fusername;
        public string username;
        public string token;
        public ChatWin(string un, string tk, string fun )
        {
            fusername = fun;
            username = un;
            token = tk;

            InitializeComponent();

            initMain();
            fetchMsg();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += OnTimedEvent;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        public void initMain()
        {
            ReplyUserFetch res =
                Shared.sendrecvjson<FormUserFetchByUsername, ReplyUserFetch>
                (Shared.HOST, "/api/user/fetch/username", new FormUserFetchByUsername(fusername));

            Fusername.Content = res.username;
            Fintro.Content = res.intro;
        }

        public void fetchMsg()
        {
            ReplyMsgFetch res =
                Shared.sendrecvjson<FormMsgFetch, ReplyMsgFetch>
                (Shared.HOST, "/api/msg/fetch", new FormMsgFetch(username, token, fusername));


            foreach (var msg in res.messages)
            {
                ListBoxItem listboxitem = new ListBoxItem();
                listboxitem.Content = msg.timestamp + "  " + msg.sender + "\n" + msg.content;
                Flist.Items.Add(listboxitem);
            }

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            ReplyMsgFetch res =
                Shared.sendrecvjson<FormMsgSend, ReplyMsgFetch>
                (Shared.HOST, "/api/msg/send", new FormMsgSend(username, token, fusername, Fmessage.Text));

            if (res.reply != "succeed") { MessageBox.Show(res.reply, "Error"); this.Close(); }

            var msg = res.messages[0];
            ListBoxItem listboxitem = new ListBoxItem();
            listboxitem.Content = msg.timestamp + "  " + msg.sender + "\n" + msg.content;
            Flist.Items.Add(listboxitem);
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            fetchMsg();
        }

        public void OnTimedEvent(object source, EventArgs e)
        {
            fetchMsg();
        }


    }
}
