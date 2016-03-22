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
    /// Логика взаимодействия для NewMatrixWindow.xaml
    /// </summary>
    public partial class NewMatrixWindow : Window
    {
        public NewMatrixWindow()
        {
            InitializeComponent();
      
        }

       

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        
        }

        public int Answer
        {
            get { return (int)numudSizeMatrix.Value; }
        }

    }
}
