using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;


using Excel = Microsoft.Office.Interop.Excel;


namespace MatrixRework
{
    public  class FileProvider
    {

        public List<List<string>> ReadFile(string path)
        {
            List<List<string>> _matrix = new List<List<string>>();

            Regex newReg = new Regex("\\d+");//только числа

            try
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    List<string> s = new List<string>();
                    MatchCollection matches = newReg.Matches(line);
                    foreach (Match m in matches)
                    {
                        s.Add(m.ToString());
                    }
                    _matrix.Add(s);

                }
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Ошибка чтения файла.\n" + ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                //              if (MessageBox.Show("Do you want to close this window?",
                //"Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //              {
                //                  // Close the window
                //              }
                //              else
                //              {
                //                  // Do not close the window
                //              }

                //+++Событие

                return null;
            }


            return _matrix;

        }

     
    }

  public  class ExelProvider
    {
        //excel oledb
        /*
        private Task InitDataGridFromExcel(string filePath, string listName)
        {
            return Task.Run(() =>
            {
                if (!File.Exists(filePath))
                {
                    throw new Exception(string.Format("File {0} wasn't found.", filePath));
                }
                var connectionString =
                    string.Format(
                        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};" +
                        " Extended Properties=Excel 12.0;",
                        filePath);
                var query = string.Format("SELECT * FROM[{0}$]", listName);
                var adapter = new OleDbDataAdapter(query, connectionString);
                var dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    var newRow = dataTable.Rows[i].ItemArray
                        .Where(k => k.ToString() != string.Empty);
                    if (!newRow.Any())
                        dataTable.Rows[i].Delete();
                }

                if (dgv_Excel.InvokeRequired)
                    dgv_Excel.Invoke(new Action(() =>
                    {
                        dgv_Excel.DataSource = dataTable;
                    }));
                else
                    dgv_Excel.DataSource = dataTable;
            });
        }
        */



        public string[,] ReadExcelFile(string path, int size)
        {
            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(path, 
                Type.Missing, Type.Missing, Type.Missing, 
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing); //открыть файл

            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист

            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
            string[,] list = new string[size, size]; // массив значений с листа равен по размеру листу

            for (int i = 1; i < lastCell.Column; i++) //по всем колонкам
                for (int j = 1; j < lastCell.Row; j++) // по всем строкам
                    list[i, j] = ObjWorkSheet.Cells[j + 1, i + 1].Text.ToString();//считываем текст в строку

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из экселя
            GC.Collect(); // убрать за собой


            return list;
        }


    }
}
