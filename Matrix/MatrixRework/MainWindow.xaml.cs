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
  public interface IView
    {
        void StatusView(string status, int step);

        void ViewСircuit(string s);
        void ViewTornCurrent(string s);

      

    } 


    public partial class MainWindow : Window, IView
    {

        decimal SizeTable = 0;

        string info = "Дипломная работа.\n\"Разработка программного обеспечения для определения оптимальной структуры жизненного цикла информационных и технических систем.\" \n Студентки группы: ТРП-1-12 \n Алины Анисимовой";

        List<Tuple<int,int>> BadCells;

         MatrixModelPresenter presenter;
      
        public MainWindow()
        {
            InitializeComponent();

            presenter = new MatrixModelPresenter(this, new MatrixCore.MatrixModel());

            BadCells = new List<Tuple<int, int>>();

         
        }

      

        public void StatusView(string status, int step)
        {
            txblock_status.Text = status;
            prgbar_status.Value = step;
        }



        public void ViewСircuit(string s)
        {
            txBlock_Сircuit.Text = s;

        }

        public void ViewTornCurrent(string s)
        {
            txBlock_TornCurrent.Text = s;
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
                
            }

        }

        public void CreateTable(int size)
        {
            numudSizeMatrix.Value = size;

           SizeTable = size;

            dgvTable.Columns.Clear(); //удаляем ранее созданые столбцы


            for (int i = 0; i < size; i++)
            {
                dgvTable.Columns.Add((i + 1).ToString(), (i+1).ToString()); //добавляем столбцы
                dgvTable.Rows.Add(); //добавляем строки
                dgvTable.Rows[i].HeaderCell.Value = (i + 1).ToString();
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

            txblock_status.Text = "created new matrix[" + size + "," + size + "]";
        }




     

         private string[,] ReadTable()
        {
            //  string[,] table = new string[4, 4] { { "0", "1", "0", "0" }, { "1", "0", "1", "0" }, { "0", "0", "0", "1" }, { "1", "0", "1", "0" } };

            dgvTable.Update();
            dgvTable.EndEdit();


            string[,] table = new string[dgvTable.Rows.Count, dgvTable.Rows.Count];
            for (int rows = 0; rows < dgvTable.Rows.Count; rows++)
            {
                for (int col = 0; col < dgvTable.Rows[rows].Cells.Count; col++)
                {

                    table[rows, col] = dgvTable[rows, col].Value != null ? dgvTable[rows, col].Value.ToString() : "0";


                }
            }

            return table;
        }


    

        private void btn_SearthAll_Click(object sender, RoutedEventArgs e)
        {
            presenter.SearthAll(ReadTable());
        }

        private void MenuItemSearchСircuit_Click(object sender, RoutedEventArgs e)
        {
            presenter.SearthSearchСircuit(ReadTable());
        }



















    



        private void dgvTable_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            ViewBadCell( e.RowIndex,e.ColumnIndex);
        }




        public void ViewBadCell(int row, int columm)
        {
            if (dgvTable.Rows[row].Cells[columm].Value.ToString() == String.Empty
                 || dgvTable.Rows[row].Cells[columm].Value.ToString() == "1"
                 || dgvTable.Rows[row].Cells[columm].Value.ToString() == "0" || dgvTable.Rows[row].Cells[columm].Value == null)
            {
                if (row == columm)
                    dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.LightGray;
                else
                    dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.White;
                BadCells.Remove(new Tuple<int, int>(row, columm));
            }
            else
            {
                dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.Red;
                BadCells.Add(new Tuple<int, int>(row, columm));

             
            }
        }




















        private void ReSizeTable(decimal size)
        {
            if (SizeTable > 0 && size!= SizeTable)
            {
                if (size > SizeTable)
                {
                  
                    for (int i = Convert.ToInt32(SizeTable); i < size; i++)
                    {
                        dgvTable.Columns.Add((i + 1).ToString(), (i + 1).ToString()); //добавляем столбцы
                        dgvTable.Rows.Add(); //добавляем строки
                        dgvTable.Rows[i].HeaderCell.Value = (i + 1).ToString();
                    }

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (i == j)
                            {
                                dgvTable[i,j].Style.BackColor = System.Drawing.Color.LightGray;

                            }

                        }
                    }



                }
                else
                {

                    for (int i = Convert.ToInt32(SizeTable) ; i > size; i--)
                    {
                        dgvTable.Columns.Remove(i.ToString());
                        dgvTable.Rows.RemoveAt(i-1);
                    }
                }
                txblock_status.Text = "размер matrix[" + SizeTable + ", " + SizeTable + "] изменен на matrix[" + numudSizeMatrix.Value + "," + numudSizeMatrix.Value + "]";
                SizeTable = size;
            }
        }


  

        private async void MenuItemOpentxtFile_Click(object sender, RoutedEventArgs e)
        {
            OpenMatrixWindow openDialog = new OpenMatrixWindow(".txt");
            if (openDialog.ShowDialog() == true)
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


                dlg.DefaultExt = ".txt";

                dlg.Filter = "Text documents (.txt)|*.txt";


                Nullable<bool> result = dlg.ShowDialog();


                if (result == true)
                {
                    List<List<string>> items = await presenter.OpentxtFile(dlg.FileName);

                    CreateTable(items.Select(x => x.Count).Max());
                    
                    for (int i = 0; i < items.Count; i++)
                    {
                        List<string> item = items[i];
                        for (int j = 0; j < items[i].Count; j++)
                            dgvTable[i, j].Value = items[i][j];
                        dgvTable.Update();
                        dgvTable.EndEdit();
                    }

                }
                else
                {
                    MessageBox.Show("При открытии файла произошла ошибка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                return;
            }
        }

        private void MenuItemOpenExcelFile_Click(object sender, RoutedEventArgs e)
        {
            OpenMatrixWindow openDialog = new OpenMatrixWindow(".excel");
            if (openDialog.ShowDialog() == true)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


                dlg.DefaultExt = ".xlsx";

                dlg.Filter = "Excel documents (.xlsx)|*.xlsx";


                Nullable<bool> result = dlg.ShowDialog();


                if (result == true)
                {
                    string[,] items = presenter.OpenExcelFile(dlg.FileName);
                    CreateTable(items.GetLength(0)> items.GetLength(1)?items.GetLength(0): items.GetLength(1));
                    for (int i = 0; i < items.GetLength(0); i++)
                        for (int j = 0; j < items.GetLength(1); j++)
                            dgvTable[i, j].Value = items[i,j];

                }
                else
                {
                    MessageBox.Show("При открытии файла произошла ошибка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                return;
            }

        }

        private void MenuItemSavetxtFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".txt";
            saveDialog.Filter= "Text documents (.txt)|*.txt";
            Nullable<bool> result = saveDialog.ShowDialog();


            if (result == true)
            {
                string matrix = String.Empty;
                string[,] items = ReadTable();
                for (int i = 0; i < items.GetLength(0); i++)
                {
                    for (int j = 0; j < items.GetLength(1); j++)
                        matrix += items[i, j] + " ";

                    matrix += "\r\n";
                }

                        presenter.SavetxtFile(matrix + "\n" + txBlock_Сircuit.Text + "\n" + txBlock_TornCurrent.Text, saveDialog.FileName);
            }
            else
            {
                MessageBox.Show("При сохранении файла произошла ошибка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void numudSizeMatrix_ValueChanged(object sender, EventArgs e)
        {
              ReSizeTable(numudSizeMatrix.Value);
           
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
