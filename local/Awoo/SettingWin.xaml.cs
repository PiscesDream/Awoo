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

namespace Awoo
{
    /// <summary>
    /// SettingWin.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWin : Window
    {
        public string username;
        public string token;
        public string avatar;
        public MainWin parent;
        public SettingWin(MainWin mainwin)
        {
            parent = mainwin;
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog dlg = new System.Windows.Forms.FontDialog();
            dlg.ShowColor = true;
            dlg.ShowEffects = false;
            dlg.ShowApply = false;
            if (object.ReferenceEquals(Shared.config, null))
                Shared.config = new Config();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FontFamilyConverter ffc = new FontFamilyConverter();
                Shared.config.chatwinfontsize = dlg.Font.Size;
                Shared.config.chatwinfontfamily = dlg.Font.Name;
                Shared.config.chatwinfontcolor = Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);
                Config.save(Shared.config);
            }
        }

        private void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (password.Password != retype.Password)
            {
                MessageBox.Show("Please input the same password", "Error");
                return;
            }


            Reply res =
              Shared.sendrecvjson<FormUpdate, Reply>
              (Shared.HOST, "/api/user/update", new FormUpdate(
                  username, token, introduce.Text, password.Password, avatar)); 
            MessageBox.Show(res.reply);
            if (res.reply == "succeed")
            {
                this.Hide();
                parent.initMain();
            }
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                avatar = Shared.FileToBase64(op.FileName, true);
            this.Avatar.Source = Shared.Base64ToImage(avatar);
        }
    }
}
