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

namespace MatrixRework
{
    /// <summary>
    /// Логика взаимодействия для OpenMatrixWindow.xaml
    /// </summary>
    public partial class OpenMatrixWindow : Window
    {
        public OpenMatrixWindow(string format_file)
        {
            InitializeComponent();
            switch(format_file)
            {
                case ".txt":
                    lbl_file.Content = "Открытие файла расширения .txt";
                    lbl_Inst.Content = "Инструкция открытия файла: /n";
                    img_Inst.Source = new BitmapImage(new Uri(@"Resources/txt.PNG", UriKind.Relative));
                    break;
                case ".excel":
                    lbl_file.Content = "Открытие файла расширения Excel";
                    lbl_Inst.Content = "Инструкция открытия файла: /n";
                    img_Inst.Source = new BitmapImage(new Uri(@"Resources/excel.PNG", UriKind.Relative));
                    break;
                default:break;
            }

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
