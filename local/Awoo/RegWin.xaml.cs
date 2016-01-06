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
    /// RegWin.xaml 的交互逻辑
    /// </summary>
    public partial class RegWin : Window
    {
        public RegWin()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public void move_window(object sender, MouseButtonEventArgs e)
        {
            Shared.move_window(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PwdInput.Password != PwdAgainInput.Password)
            {
                MessageBox.Show("Please input the same password.");
                return;
            }

            Reply res =
                Shared.sendrecvjson<FormRegister, Reply>
                    (Shared.HOST, "/api/register", new FormRegister(EmailInput.Text, 
                                                                 UnInput.Text, 
                                                                 PwdInput.Password));
            MessageBox.Show(res.reply, "Feedback");
            if (res.reply == "succeed")
                this.Hide();
        }
    }
}
