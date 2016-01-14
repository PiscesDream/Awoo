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
using Microsoft.Win32;

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
        //System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

    //  Timer =====================================================================================
        delegate void UIDelegate();
        static ReplyMsgFetch msgbox = new ReplyMsgFetch();
        void updatemsglist()
        {
            statuscircle.Fill = new SolidColorBrush(Color.FromArgb(128, 64, 255, 64));
            foreach (var msg in msgbox.messages)
                plotMsg(msg);
            msgbox.messages.Clear();
        }
        void updatestatuswarning()
        {
            statuscircle.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 64, 64));
        }
        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            msgbox =
                Shared.sendrecvjson<FormMsgFetch, ReplyMsgFetch>
                (Shared.HOST, "/api/msg/fetch", new FormMsgFetch(username, token, fusername));
            try
            {
                string a = msgbox.reply;
                UIDelegate UIhandler = new UIDelegate(updatemsglist);
                Application.Current.Dispatcher.BeginInvoke(UIhandler);
            }
            catch
            {
                UIDelegate UIwarning = new UIDelegate(updatestatuswarning);
                Application.Current.Dispatcher.BeginInvoke(UIwarning);
            }
        }
        System.Timers.Timer timer;


        public ChatWin(string un, string tk, string fun )
        {
            fusername = fun;
            username = un;
            token = tk;

            InitializeComponent();

            initMain();

            timer = new Timer(300);
            timer.Elapsed += t_Elapsed;
            timer.Start();
            //dispatcherTimer.Tick += OnTimedEvent;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //dispatcherTimer.Start();
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

       public void sendMsg(string msg)
        {
            ReplyMsgFetch res =
                Shared.sendrecvjson<FormMsgSend, ReplyMsgFetch>
                (Shared.HOST, "/api/msg/send", new FormMsgSend(username, token, fusername, msg));

            if (res.reply != "succeed") { MessageBox.Show(res.reply, "Error"); this.Close(); }

            plotMsg(res.messages[0]);
        }


        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //this.Close();
        }

        public void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            if (!Fmessage.Text.StartsWith("{$"))
                Fmessage.Text = TypeTextRaw.header + Fmessage.Text;
            sendMsg(Fmessage.Text);
            Fmessage.Text = "";
        }
        private void plotMsg(ReplyMsgFetchUnit msg)
        {
            Grid grid = new Grid();
            //do this for each row
            for (int _ = 0; _ < 2; ++_)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                grid.RowDefinitions.Add(rowdef);
            }

            Label title = new Label();

            //            Exception e = new Exception();
            //            throw (e);

            //            if (msg.sender == username)
            title.Content = msg.timestamp + "  " + msg.sender;
            grid.Children.Add(title);
            Grid.SetRow(title, 0);
            if (msg.sender == username)
            {
                grid.HorizontalAlignment = HorizontalAlignment.Right;
                title.HorizontalContentAlignment = HorizontalAlignment.Right;
            } 

            UIElement obj = null;
            if (msg.content.StartsWith(TypeImgBase64.header))
                obj = TypeImgBase64.parse(msg.content);
            else if (msg.content.StartsWith(TypeTextRaw.header))
                obj = TypeTextRaw.parse(msg.content, username==msg.sender);
            else
                obj = TypeText.parse(msg.content, username==msg.sender);

            if (!object.ReferenceEquals(obj, null))
            {
                grid.Children.Add(obj);
                Grid.SetRow(obj, 1);
            }

            Flist.Items.Add(grid);
            Flist.ScrollIntoView(grid);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            //dispatcherTimer.Stop();
        }

        private void Fmessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    send_Click(sender, null);
        }

        private void picbutton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            String base64 = TypeImgBase64.header;
            if (op.ShowDialog() == true)
            {
                base64 += Shared.FileToBase64(op.FileName);
                sendMsg(base64);
            }
        }

        private void drawbutton_Click(object sender, RoutedEventArgs e)
        {
            DrawWin drawwin = new DrawWin(this);
            drawwin.Show();
            this.IsEnabled = false;
        }

        private void hisbutton_Click(object sender, RoutedEventArgs e)
        {
            ReplyMsgFetch res =
                Shared.sendrecvjson<FormMsgFetch, ReplyMsgFetch>
                (Shared.HOST, "/api/msg/fetch/history", new FormMsgFetch(username, token, fusername));


            try
            {
                Flist.Items.Clear();
                foreach (var msg in res.messages)
                    plotMsg(msg);
            }
            catch
            {
                MessageBox.Show(res.reply, "Error");
            }
        }
    }
}
