using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatrixRework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string info = "Дипломная работа.\n\"Разработка программного обеспечения для определения оптимальной структуры жизненного цикла информационных и технических систем.\" \n Студентки группы: ТРП-1-12 \n Алины Анисимовой";

      public string Status { get; set; }

         MatrixModelPresenter presenter;
      
        public MainWindow()
        {
            InitializeComponent();

            presenter = new MatrixModelPresenter(this, new MatrixCore.MatrixModel());

        }

      

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из программы?", "Выход", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    break;
               
            }
        }

        private void MenuItemInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(info, "Информация");
        }




        private void MenuItemNewFile_Click(object sender, RoutedEventArgs e)
        {
            NewMatrixWindow inputDialog = new NewMatrixWindow();
            if (inputDialog.ShowDialog() == true)
            {
                CreateTable(inputDialog.Answer);
                numudSizeMatrix.Value = inputDialog.Answer;
            }

        }

        public void CreateTable(int size)
        {
            dgvTable.Columns.Clear(); //удаляем ранее созданые столбцы

            int n = 1;
            for (int i = 0; i < size; i++)
            {
                dgvTable.Columns.Add("" + n, "" + n); //добавляем столбцы
                dgvTable.Rows.Add(); //добавляем строки
                dgvTable.Rows[i].HeaderCell.Value = "" + n;
                n++;
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        dgvTable.Rows[j].Cells[i].Style.BackColor = System.Drawing.Color.LightGray;

                    }

                }
            }

            Status = "created new matrix[" + size + "," + size + "]";
        }




        private void dgvTable_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {

        }




         public string[,] ReadTable()
        {
            string[,] table = new string[4, 4] { { "0", "1", "0", "0" }, { "1", "0", "1", "0" }, { "0", "0", "0", "1" }, { "1", "0", "1", "0" } };

            //string[,] table = new string[4, 4];
            //for (int rows = 0; rows < dgvTable.Rows.Count; rows++)
            //{
            //    for (int col = 0; col < dgvTable.Rows[rows].Cells.Count; col++)
            //    {
            //        table[rows,col] = dgvTable.Rows[rows].Cells[col].Value.ToString();

            //    }
            //}

            return table;
        }



        public void ReSizeTable(int size)
        {

        }


        public void ViewBadCell(int row, int columm)
        {
            dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.Red;
        }

        
      public void vievCirkuit(string s)
        {
            textBlock.Text = s;
        }

        public void vievrazrivi(string s)
        {
            textBlock1.Text = s;
        }

        private void ViewTable(int[,] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    dgvTable.Rows[j].Cells[i].Value = matrix[i, j];
                }
            }
        }













        private void numudSizeMatrix_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            CreateTable((int)numudSizeMatrix.Value);

        }

        private void MenuItemVichislenieVsego_Click(object sender, RoutedEventArgs e)
        {
            presenter.CreateMatrix();
        }

        //private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    string s1 = e.EditingElement.ToString();
        //    string s2 = e.Column.DisplayIndex.ToString();
        //    string s3 = e.Row.GetIndex().ToString();

        //    DataGridRow firstRow = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.Items[0]) as DataGridRow;
        //    DataGridCell firstColumnInFirstRow = dataGrid.Columns[0].GetCellContent(firstRow).Parent as DataGridCell;
        //    //set background
        //    firstColumnInFirstRow.Background = Brushes.Red;
        //}


    }
}
