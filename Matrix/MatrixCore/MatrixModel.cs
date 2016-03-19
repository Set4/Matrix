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
        /// Создание матрицы и заполнение матрицы
        /// </summary>
        /// <param name="size">размер матрицы</param>
        public void CreateMatrix(int size,)
        {
            /////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1 ивсе другие действия с начальной матрицей
            matrix = new Matrix(size);
        }


        /// <summary>
        /// Событие нахождения неверных значений матрицы
        /// </summary>
        public event EventHandler<EventMessage> BadMatrix = delegate { };

        /// <summary>
        /// Событие вычисление закончено
        /// </summary>
        public event EventHandler<EventMessage> СalculationСompleted = delegate { };




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
                    if (Int32.Parse(matrix[height, width]) == 1)
                        convertMatrix[height, width] = true;
                    else if (Int32.Parse(matrix[height, width]) == 0 || matrix[height, width] == String.Empty)
                        convertMatrix[height, width] = false;
                }
            }
            return convertMatrix;
        }







        /// <summary>
        /// Нахождение контуров
        /// </summary>
        /// <returns>контуры List<Сircuit></returns>
        public List<Сircuit> SearchСircuit()
        {
            List<Сircuit> _сircuit = new List<Сircuit>();
            bool[,] _matrix =matrix.ReachabilityMatrix= matrix.AdjacencyMatrix;// матрица хранящая возведенную в степень матрицу

            for (int stage = 0; stage < matrix.SizeMatrix - 1; stage++)
            {
                //Перемножение номер: " + stage+1 + " \n";
                _matrix = Matrix.MultiplicationMatrix(_matrix, matrix.AdjacencyMatrix);//перемножение матриц при помощи метода Mul
                matrix.ReachabilityMatrix = Matrix.SummationMatrix(matrix.ReachabilityMatrix, _matrix);
//
                List<int> _cir =Matrix.SearchСircuitVertices(_matrix);
                if (!_сircuit.Contains(_cir) && _cir.Count != 0)
                {
                    if (_cir.Count <= stage + 1)
                        _сircuit.Add(_cir);
                    else
                        _сircuit.AddRange( YtochninieContyra(stage, _cir));//ytochnenie kontyra
                }
            }

            СalculationСompleted(this, new EventMessage("Нахождение контуров завершено", cycles));

            return cycles;
        }



     

        /// <summary>
        /// поиск разрываемых потоков
        /// </summary>
        /// <param name="circuits">список всех циклов</param>
        /// <returns>поток-список циклов которые он разрывает</returns>
        public Dictionary<Current, List<Сircuit>> SearchTornCurrent(List<Сircuit> circuits)
        {
            Dictionary<Current, List<Сircuit>> _dict = new Dictionary<Current, List<Сircuit>>();

            List<Сircuit> _circuits = circuits;
            Сircuit generalСircuit = new Сircuit(new List<Current>());
            foreach (Сircuit circuit in circuits)
                foreach (Current item in circuit.CurrentList)
                {
                    generalСircuit.AddСircuitItem(item);
                }

            while (_circuits.Count != 0)
            {

                List<Сircuit> _item = new List<Сircuit>();


                Current maxitem= generalСircuit.CurrentList.Where(obj => obj.Amount == generalСircuit.CurrentList.Max(i => i.Amount)).First();


                foreach (Сircuit circuit in _circuits)
                {
                    if (circuit.CurrentList.IndexOf(maxitem) != -1)
                    {
                        _item.Add(circuit);
                        _circuits.Remove(circuit);
                    }
                }

                _dict.Add(maxitem, _item);

              generalСircuit.CurrentList.Remove(maxitem);
            }

            СalculationСompleted(this, new EventMessage("Нахождение разрываемых потоков завершено", _dict));
            return _dict;
        }





    }

   


    


}
