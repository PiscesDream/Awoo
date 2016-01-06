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
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Awoo
{
    /// <summary>
    /// MainWin.xaml 的交互逻辑
    /// </summary>
    public partial class MainWin : Window
    {
        // #FFC3D6FF
        Color blinkcolor = new Color();
        bool blinkbool = false;

        public string id;
        public string avatar;
        public string intro;
        public string status;
        public string username;
        public string email;
        public string lastlogin;
        public string token;
        public List<string> friends_username = new List<string>();
        public List<Grid> friends_grid = new List<Grid>();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWin(string un, string tk)
        {
            username = un;
            token = tk;

            InitializeComponent();

            initMain();
            initFriends();

            this.Topmost = true;

            blinkcolor = Color.FromArgb(0xFF, 0xC3, 0xD6, 0xff);


            Shared.configpath = @"./" + username + ".xml";
            try { Shared.config = Config.load(); } catch { Shared.config = null; }


            dispatcherTimer.Tick += blinking;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Start();
        }
        public void blinking(object source, EventArgs e)
        {
            Brush brush;
            if (blinkbool)
                brush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            else
                brush = new SolidColorBrush(blinkcolor);
            blinkbool = !blinkbool;

            ReplyMsgNotice res = 
                Shared.sendrecvjson<FormMsgNotice, ReplyMsgNotice>
                (Shared.HOST, "/api/msg/fetch/notice", new FormMsgNotice(username, token));
            try
            {
                for (int i = 0; i < friends_username.Count; ++i)
                {
                    friends_grid[i].Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    foreach (var username in res.usernames)
                        if (username == friends_username[i])
                        {
                            friends_grid[i].Background = brush;
                            break;
                        }
                }
            }
            catch
            {

            }

        }

        public void initMain()
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
            Username.Content = username;
            Intro.Content = "Introduction:" + intro;
        }
        public void initFriends()
        {
            ReplyUserFetchFriends res = 
                Shared.sendrecvjson<FormUserFetchFriends, ReplyUserFetchFriends>
                (Shared.HOST, "/api/user/fetch/friends", new FormUserFetchFriends(username, token));
            if (res.reply != "succeed") { MessageBox.Show(res.reply, "Error"); this.Close(); }

            List.Items.Clear();
            friends_username.Clear();
            friends_grid.Clear();
            foreach (var friend in res.friends)
            {
                /*
                <Grid Height="57" Width="268">
                    <Image Margin="9,9,0,9" HorizontalAlignment="Left" Width="39"/>
                    <Label Margin="55,0,10,30" Content="asdf" FontSize="13"/>
                </Grid>
                */

                ReplyUserFetch friendres = 
                    Shared.sendrecvjson<FormUserFetchByUsername, ReplyUserFetch>
                    (Shared.HOST, "/api/user/fetch/username", new FormUserFetchByUsername(friend));

                Border border = new Border();
                border.BorderBrush = Brushes.Gray; border.BorderThickness = new Thickness(0, 0, 1, 1);
                Grid grid = new Grid();
                grid.Height = 57; grid.Width = this.Width-10;
                Image image = new Image();
                image.Source = Shared.Base64ToImage(friendres.avatar);
                image.Width = image.Height = 39; image.Margin = new Thickness(0, 9, 9, 9); image.HorizontalAlignment = HorizontalAlignment.Left;
                Label label = new Label();
                label.Content = friendres.username;
                label.Margin = new Thickness(55,0,10,30); label.HorizontalAlignment = HorizontalAlignment.Left;label.FontSize = 13;
                grid.Children.Add(image);
                grid.Children.Add(label);
                border.Child = grid;
                List.Items.Add(border);
                grid.MouseRightButtonUp += (MouseButtonEventHandler)popup;

                friends_grid.Add(grid);
                friends_username.Add(friendres.username);
            }
        }


        private void popup(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("friendPopup") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            Application.Current.Shutdown();
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(List.SelectedIndex.ToString());
            ChatWin chatwin = new ChatWin(username, token, friends_username[List.SelectedIndex]);
            chatwin.Show();            
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            //locationtext.Content = "Location: (" + this.Left.ToString() + ", " + this.Top.ToString() + ")";
            if (this.Top <= 5) this.Top = 5;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        /*
                public struct WINDOWPOS
                {
                    public IntPtr hwnd;
                    public IntPtr hwndInsertAfter;
                    public int x;
                    public int y;
                    public int cx;
                    public int cy;
                    public UInt32 flags;
                };

                private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
                {
                    switch (msg)
                    {
                        case 0x46://WM_WINDOWPOSCHANGING
                            if (Mouse.LeftButton != MouseButtonState.Pressed)
                            {
                                WINDOWPOS wp = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                                wp.flags = wp.flags | 2; //SWP_NOMOVE
                                Marshal.StructureToPtr(wp, lParam, false);
                            }
                            break;
                    }
                    return IntPtr.Zero;
                }

                private void Window_Loaded(object sender, RoutedEventArgs e)
                {
                    HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
                    source.AddHook(new HwndSourceHook(WndProc));
                }
         */

        public double original_height = 0.0;
        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.Top <= 10)
            {
                original_height = this.Height;
                this.Height = 15;
                //locationtext.Content = "hidden";
            }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Top <= 10)
            {
                this.Height = original_height;
                //locationtext.Content = "recover";
            }
        }


        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
             ReplyUserAll res = 
                Shared.sendrecvjson<FormUserQuery, ReplyUserAll>
                (Shared.HOST, "/api/user/query", new FormUserQuery(username, token, fquery.Text));

            if (res.reply != "succeed")
            { MessageBox.Show(res.reply, "Error");return; }
            else
            {
                string s = "Result:\n";
                foreach (var str in res.usernames) s += str + '\n';
                MessageBox.Show(s, "Searched username");
                return;
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
             Reply res = 
                Shared.sendrecvjson<FormUserQuery, Reply>
                (Shared.HOST, "/api/user/makefriend", new FormUserQuery(username, token, fadd.Text));
            if (res.reply != "succeed")
            { MessageBox.Show(res.reply, "Error");return; }
            else
            {
                MessageBox.Show(res.reply, "Success");
                dispatcherTimer.Stop();
                initFriends();
                dispatcherTimer.Start();
                return;
            }
        }

        private void sendmessage(object sender, RoutedEventArgs e)
        {
            ChatWin chatwin = new ChatWin(username, token, friends_username[List.SelectedIndex]);
            chatwin.Show();            
        }

        private void deleteFriend(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete friend [" + friends_username[List.SelectedIndex] + "] ?", 
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
                == MessageBoxResult.No)
                return;

             Reply res = 
                Shared.sendrecvjson<FormUserQuery, Reply>
                (Shared.HOST, "/api/user/deletefriend", new FormUserQuery(username, token, friends_username[List.SelectedIndex]));
            if (res.reply != "succeed")
            { MessageBox.Show(res.reply, "Error");return; }
            else
            {
                MessageBox.Show(res.reply, "Success");
                dispatcherTimer.Stop();
                initFriends();
                dispatcherTimer.Start();
                return;
            }
        }

        public SettingWin settingwin = new SettingWin();
        private void button_Click(object sender, RoutedEventArgs e)
        {
            settingwin.Show();
        }

    }
}
