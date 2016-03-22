using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
   
  public  class MatrixModel
    {
        Matrix matrix { get; set; }


        /// <summary>
        /// Событие размер матрицы меньше или равно 0
        /// </summary>
        public event EventHandler<EventMessage> ErrorSizeMatrix = delegate { };



        /// <summary>
        /// Создание матрицы и заполнение матрицы
        /// </summary>
        public bool CreateMatrix(int weght, int height, bool[,] _matrix)
        {
            if (weght == height && weght > 0)
            {

                matrix = new Matrix(_matrix);
                return true;
            }
            else
            {

                ErrorSizeMatrix(this, new EventMessage("размер матрицы меньше или равно 0"));
                return false;
            }
        }


        /// <summary>
        /// Событие нахождения неверных значений матрицы
        /// </summary>
        public event EventHandler<EventMessage> BadMatrix = delegate { };

        /// <summary>
        /// Событие вычисление закончено
        /// </summary>
        public event EventHandler<EventMessage> СalculationСompleted = delegate { };


            // СalculationСompleted(this, new EventMessage("Нахождение контуров завершено", cycles));

            


            public bool IsBadCell(string cell)
        {
            if (cell != "1" || cell != "0" || !String.IsNullOrEmpty(cell))
                return false;
            else
                return true;

        }



        /// <summary>
        /// Проверка введенной матрицы на правильность
        /// </summary>
        /// <param name="matrix">матрица</param>
        /// <returns>true- если матрица содержит неверные элементы</returns>
        public bool IsBadMatrix(string[,] matrix)
        {
            List<List<int[]>> badMatrix = GetBadCellsMatrix(matrix);
            if (badMatrix.Count != 0)
            {
                BadMatrix(this, new EventMessage("Найдены неверные значения матрицы", badMatrix));
                return true;
            }
            else
                return false;               
        }

        /// <summary>
        /// Проверка введенной матрицы на наличие не 0 или 1
        /// </summary>
        /// <param name="matrix">проверяемая матрица</param>
        /// <returns>Получаем матрицу с индексами ячеек, не равных 1 или 0 </returns>
        private List<List<int[]>> GetBadCellsMatrix(string[,] matrix)
        {
            List<List<int[]>> badMatrix = new List<List<int[]>>();
            int _result;
            bool _b;

            for (int height = 0; height < matrix.GetLength(0); height++)
            {
                List<int[]> _list = new List<int[]>();
                for (int width = 0; width < matrix.GetLength(1); width++)
                {
                    _b = Int32.TryParse(matrix[height, width], out _result);
                    if (!_b && (_result != 0 || _result != 1))
                        _list.Add(new int[2] { height, width });
                }
                badMatrix.Add(_list);
            }
            return badMatrix;
        }

        /// <summary>
        /// Переводим string 0 и 1 в bool(true, false)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>матрица bool[,]</returns>
        public bool[,] ConvertMatrixToBool(string[,] matrix)
        {
            bool[,] convertMatrix = new bool[matrix.GetLength(0), matrix.GetLength(1)];

            for (int height = 0; height < matrix.GetLength(0); height++)
            {
                for (int width = 0; width < matrix.GetLength(1); width++)
                {
                    if (matrix[height, width] == "1")
                        convertMatrix[height, width] = true;
                    else if (matrix[height, width] == "0")
                        convertMatrix[height, width] = false;
                }
            }
            return convertMatrix;
        }









        public List<Сircuit> SearchСircuit()
        {
            return  matrix.SearchСircuit();
        }




        /// <summary>
        /// поиск разрываемых потоков
        /// </summary>
        /// <param name="circuits">список всех циклов</param>
        /// <returns>поток-список циклов которые он разрывает</returns>
        public Dictionary<Current, List<Сircuit>> SearchTornCurrent(List<Сircuit> circuits)
        {
            Dictionary<Current, List<Сircuit>> _dict = new Dictionary<Current, List<Сircuit>>();

            List<Сircuit> list;

               Сircuit generalСircuit = new Сircuit(new List<Current>());
            foreach (Сircuit circuit in circuits)
                foreach (Current item in circuit.CurrentList)
                {
                    generalСircuit.AddСircuitItem(item);
                }


            foreach (Current item in generalСircuit.CurrentList)
            {
                list = new List<Сircuit>();
                foreach (Сircuit circuit in circuits)
                    if (circuit.CurrentList.Where(i => i.StartingPoint == item.StartingPoint && i.FinalPoint == item.FinalPoint).ToList().Count != 0)
                        list.Add(circuit);

                _dict.Add(item, list);
            }



             СalculationСompleted(this,new EventMessage("Нахождение потоков завершено"));
            return _dict;
        }



       

    }

   


    


}
