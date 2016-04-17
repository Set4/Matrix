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

        void ViewTable(int[,] matrix);

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
                numudSizeMatrix.Value = inputDialog.Answer;
            }

        }

        public void CreateTable(int size)
        {
            SizeTable = size;

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

            txblock_status.Text = "created new matrix[" + size + "," + size + "]";
        }




     

         private string[,] ReadTable()
        {
            //  string[,] table = new string[4, 4] { { "0", "1", "0", "0" }, { "1", "0", "1", "0" }, { "0", "0", "0", "1" }, { "1", "0", "1", "0" } };

            dgvTable.Update();
            dgvTable.EndEdit();


            string[,] table = new string[4, 4];
            for (int rows = 0; rows < dgvTable.Rows.Count; rows++)
            {
                for (int col = 0; col < dgvTable.Rows[rows].Cells.Count; col++)
               {
                    table[rows,col] = dgvTable.Rows[rows].Cells[col].Value.ToString();

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
            if (dgvTable.Rows[row].Cells[columm].Value.ToString() != String.Empty
                 || dgvTable.Rows[row].Cells[columm].Value.ToString() != "1"
                 || dgvTable.Rows[row].Cells[columm].Value.ToString() != "0")
            {
                dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.Red;
                BadCells.Add(new Tuple<int, int>(row, columm));
            }
            else
            {
                if(row==columm)
                dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.LightGray;
                else
                    dgvTable.Rows[row].Cells[columm].Style.BackColor = System.Drawing.Color.White;
                BadCells.Remove(new Tuple<int, int>(row, columm));
            }
        }


















    public void ViewTable(int[,] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    dgvTable.Rows[j].Cells[i].Value = matrix[i, j];
                }
            }
        }


        private void ReSizeTable(decimal size)
        {
            if (SizeTable > 0&& size!= SizeTable)
            {
                if (size > SizeTable)
                {
                    int n = 1;
                    for (int i = Convert.ToInt32(SizeTable) - 1; i < size; i++)
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



                }
                else
                {

                    for (int i = Convert.ToInt32(SizeTable) - 1; i < size; i--)
                    {
                        dgvTable.Columns.RemoveAt(i);
                        dgvTable.Rows.RemoveAt(i);
                    }
                }
                txblock_status.Text = "размер matrix[" + SizeTable + ", " + SizeTable + "] изменен на matrix[" + numudSizeMatrix.Value + "," + numudSizeMatrix.Value + "]";
                SizeTable = size;
            }
        }


        private void numudSizeMatrix_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
           

        }

        private async void MenuItemOpentxtFile_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> items = await presenter.OpentxtFile();
            CreateTable(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                List<string> item = items[i];
                for (int j = 0; j < items.Count || j < items[i].Count; j++)
                    dgvTable.Rows[i].Cells[j].Value = items[i][j];
            }
        }

        private void MenuItemOpenExcelFile_Click(object sender, RoutedEventArgs e)
        {
            string[,] items = presenter.OpenExcelFile();
            CreateTable(items.GetLength(0));
            for (int i = 0; i < items.GetLength(0); i++)
                for (int j = 0; j < items.GetLength(1); j++)
                    dgvTable.Rows[i].Cells[j].Value = items[i,j];
            
        }

        private void MenuItemSavetxtFile_Click(object sender, RoutedEventArgs e)
        {
            presenter.SavetxtFile(ReadTable() + "\n" + txBlock_Сircuit.Text + "\n" + txBlock_TornCurrent.Text);
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
