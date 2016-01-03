using System;
using System.Collections.Generic;
using System.IO;
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
    /// DrawWin.xaml 的交互逻辑
    /// </summary>
    public partial class DrawWin : Window
    {
        enum DrawType {Dot, Line, Rect, Ellipse }
        DrawType type;
        bool mouseDown, fill;
        double thickness;
        Color sCol, fCol;
        SolidColorBrush sBrush, fBrush;
        System.Windows.Shapes.Ellipse dot;
        Line line;
        Rectangle rect;
        Ellipse ellipse;
        double x1, y1, x2, y2;
        ChatWin parent;

        private void btEllipse_Checked(object sender, RoutedEventArgs e)
        {
            type = DrawType.Ellipse;
        }
        private Color getColor(Color col)
        {
            System.Windows.Forms.ColorDialog colDlg
                = new System.Windows.Forms.ColorDialog();
            colDlg.Color = System.Drawing.Color.FromArgb(sCol.A,
                sCol.R, sCol.G, sCol.B);
            if (colDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return Color.FromArgb(colDlg.Color.A, colDlg.Color.R,
                    colDlg.Color.G, colDlg.Color.B);
            }
            return col;
        }

        private void btSColor_Click(object sender, RoutedEventArgs e)
        {
            Color col = getColor(sCol);
            if (col != sCol)
            {
                sCol = col;
                sBrush = new SolidColorBrush(sCol);
                btSColor.Background = sBrush;
            }
        }

        private void btFColor_Click(object sender, RoutedEventArgs e)
        {
            Color col = getColor(fCol);
            if (col != fCol)
            {
                fCol = col;
                fBrush = new SolidColorBrush(fCol);
                btFColor.Background = fBrush;
            }
        }

        private void btFill_Checked(object sender, RoutedEventArgs e)
        {
            fill = true;
        }

        private void btFill_Unchecked(object sender, RoutedEventArgs e)
        {
            fill = false;
        }

        private void tbThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!tbThickness.IsFocused) return;
            if (tbThickness.Text == "") return;
            thickness = Double.Parse(tbThickness.Text);
            if (thickness < 0) thickness = 0;
            tbThickness.Text = thickness.ToString();
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            x1 = e.GetPosition(canvas).X;
            y1 = e.GetPosition(canvas).Y;
            mouseDown = true;
            switch (type)
            {
                case DrawType.Line:
                    line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.StrokeThickness = 1;
                    line.X2 = line.X1 = x1;
                    line.Y2 = line.Y1 = y1;
                    canvas.Children.Add(line);
                    break;
                case DrawType.Rect:
                    rect = new Rectangle();
                    Canvas.SetLeft(rect, x1);
                    Canvas.SetTop(rect, y1);
                    rect.Height = 0;
                    rect.Width = 0;
                    rect.Stroke = Brushes.Gray;
                    rect.StrokeThickness = 1;
                    canvas.Children.Add(rect);
                    break;
                case DrawType.Ellipse:
                    ellipse = new Ellipse();
                    Canvas.SetLeft(ellipse, x1);
                    Canvas.SetTop(ellipse, y1);
                    ellipse.Height = 0;
                    ellipse.Width = 0;
                    ellipse.Stroke = Brushes.Gray;
                    ellipse.StrokeThickness = 1;
                    canvas.Children.Add(ellipse);
                    break;
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            x2 = e.GetPosition(canvas).X;
            y2 = e.GetPosition(canvas).Y;
            switch (type)
            {
                case DrawType.Line:
                    line.Stroke = sBrush;
                    line.StrokeThickness = thickness;
                    line.X2 = x2;
                    line.Y2 = y2;
                    break;
                case DrawType.Rect:
                    rect.Height = y2 >= y1 ? y2 - y1 : y1 - y2;
                    rect.Width = x2 >= x1 ? x2 - x1 : x1 - x2;
                    rect.Stroke = sBrush;
                    rect.StrokeThickness = thickness;
                    if (fill) rect.Fill = fBrush;
                    break;
                case DrawType.Ellipse:
                    ellipse.Height = y2 >= y1 ? y2 - y1 : y1 - y2;
                    ellipse.Width = x2 >= x1 ? x2 - x1 : x1 - x2;
                    ellipse.Stroke = sBrush;
                    ellipse.StrokeThickness = thickness;
                    if (fill) ellipse.Fill = fBrush;
                    break;
            }
            mouseDown = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            double h, w;
            x2 = e.GetPosition(canvas).X;
            y2 = e.GetPosition(canvas).Y;
            if (!mouseDown) return;
            switch (type)
            {
                case DrawType.Dot:
                    dot = new Ellipse();
                    Canvas.SetLeft(dot, x2);
                    Canvas.SetTop(dot, y2);
                    dot.Height = thickness;
                    dot.Width = thickness;
                    dot.Stroke = this.sBrush;
                    dot.StrokeThickness = thickness;
                    dot.Fill = this.sBrush;
                    canvas.Children.Add(dot);
                    break;
                case DrawType.Line:
                    line.X2 = x2;
                    line.Y2 = y2;
                    break;
                case DrawType.Rect:
                    h = y2 - y1;
                    if (h >= 0)
                    {
                        rect.Height = h;
                        Canvas.SetTop(rect, y1);
                    }
                    else // h < 0
                    {
                        rect.Height = -h;
                        Canvas.SetTop(rect, y2);
                    }
                    w = x2 - x1;
                    if (w >= 0)
                    {
                        rect.Width = w;
                        Canvas.SetLeft(rect, x1);
                    }
                    else // w < 0
                    {
                        rect.Width = -w;
                        Canvas.SetLeft(rect, x2);
                    }
                    break;
                case DrawType.Ellipse:
                    h = y2 - y1;
                    if (h >= 0)
                    {
                        ellipse.Height = h;
                        Canvas.SetTop(ellipse, y1);
                    }
                    else // h < 0
                    {
                        ellipse.Height = -h;
                        Canvas.SetTop(ellipse, y2);
                    }
                    w = x2 - x1;
                    if (w >= 0)
                    {
                        ellipse.Width = w;
                        Canvas.SetLeft(ellipse, x1);
                    }
                    else // w < 0
                    {
                        ellipse.Width = -w;
                        Canvas.SetLeft(ellipse, x2);
                    }
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            parent.Show();
            parent.IsEnabled = true;
        }

        private void btRect_Checked(object sender, RoutedEventArgs e)
        {
            type = DrawType.Rect;
        }

        public DrawWin(ChatWin __parent)
        {
            InitializeComponent();
            parent = __parent;

            btDot.IsChecked = true;
            btFill.IsChecked = true;
            type = DrawType.Dot;
            fill = true;
            thickness = 4;
            sCol = Colors.Red;
            fCol = Colors.Green;
            sBrush = new SolidColorBrush(sCol);
            fBrush = new SolidColorBrush(fCol);
        }

        private void btLine_Checked(object sender, RoutedEventArgs e)
        {
            type = DrawType.Line;
        }

        private void btDot_Checked(object sender, RoutedEventArgs e)
        {
            type = DrawType.Dot;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void move_window(object sender, MouseEventArgs e)
        {
            Shared.move_window(this);
        }
        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bmpren = new RenderTargetBitmap(
                (int)this.ActualWidth, (int)this.ActualHeight, 
                96, 96, PixelFormats.Pbgra32);
            bmpren.Render(canvasGrid);
            CroppedBitmap cropbmpren = new CroppedBitmap(bmpren, new Int32Rect(0, (int)canvasGrid.Margin.Top, (int)canvasGrid.ActualWidth, (int)canvasGrid.ActualHeight));
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(cropbmpren));
            encoder.Save(stream);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);
            parent.sendMsg(TypeImgBase64.header+Shared.BitmapToBase64(bitmap));

            this.Close();
        }
    }
}
