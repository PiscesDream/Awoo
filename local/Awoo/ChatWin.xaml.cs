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
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        bool blinking = false;
        public ChatWin(string un, string tk, string fun )
        {
            fusername = fun;
            username = un;
            token = tk;

            InitializeComponent();

            initMain();
            fetchMsg();

            dispatcherTimer.Tick += OnTimedEvent;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        public void initMain()
        {
            ReplyUserFetch res =
                Shared.sendrecvjson<FormUserFetchByUsername, ReplyUserFetch>
                (Shared.HOST, "/api/user/fetch/username", new FormUserFetchByUsername(fusername));

            try
            {
                Fusername.Content = res.username;
                Fintro.Content = res.intro;
                Favatar.Source = Shared.Base64ToImage(res.avatar);
            }
            catch
            {
                MessageBox.Show(res.reply, "Error");
            }
        }

        public void fetchMsg()
        {
            ReplyMsgFetch res =
                Shared.sendrecvjson<FormMsgFetch, ReplyMsgFetch>
                (Shared.HOST, "/api/msg/fetch", new FormMsgFetch(username, token, fusername));


            try
            {
                foreach (var msg in res.messages)
                {
                    ListBoxItem listboxitem = new ListBoxItem();
                    listboxitem.Content = msg.timestamp + "  " + msg.sender + "\n" + msg.content;
                    Flist.Items.Add(listboxitem);
                }
            }
            catch
            {
                MessageBox.Show(res.reply, "Error");
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

            try
            {
                var msg = res.messages[0];
                ListBoxItem listboxitem = new ListBoxItem();
                listboxitem.Content = msg.timestamp + "  " + msg.sender + "\n" + msg.content;
                Flist.Items.Add(listboxitem);
                Fmessage.Text = "";
            }
            catch
            {
                MessageBox.Show(res.reply, "Error");
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            fetchMsg();
        }

        public void OnTimedEvent(object source, EventArgs e)
        {
            fetchMsg();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void Fmessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    send_Click(sender, null);
        }
    }
}
