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
        public SettingWin()
        {
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
    }
}
