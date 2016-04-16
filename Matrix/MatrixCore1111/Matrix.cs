using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MatrixCore
{
   public class Matrix
    {
        /// <summary>
        /// Размер матрицы
        /// </summary>
        public int SizeMatrix
        {
            get { return AdjacencyMatrix.GetLength(0); } 
        }



        /// <summary>
        /// Матрица смежности
        /// </summary>
        public bool[,] AdjacencyMatrix { get; private set; }

        /// <summary>
        /// Матрица достижимости
        /// </summary>
        public bool[,] ReachabilityMatrix { get; private set; }


        /// <summary>
        /// Matrix высокой связанности
        /// </summary>
        public bool[,] СonnectivityMatrix
        {
            get
            {
                return MatrixMethods.MultiplicationElementsMatrix(ReachabilityMatrix, MatrixMethods.TranspositionMatrix(ReachabilityMatrix));
            }
        }


        /// <summary>
        /// Создание матрицы
        /// </summary>
        /// <param name="sizeMatrix">размер матрицы</param>
        public Matrix(bool[,] matrix)
        {
            AdjacencyMatrix = ReachabilityMatrix = matrix;
        }





        /// <summary>
        /// Изменение размера матрицы смежности с сохранением значений
        /// </summary>
        /// <param name="size">новый размер матрицы</param>
        public void CloneMatrix(int size)
        {
            bool[,] newMatrix = new bool[size, size];

            for (int height = 0; height < size; height++)
                for (int width = 0; width < size; width++)
                    newMatrix[height, width] = AdjacencyMatrix[height, width];

            AdjacencyMatrix = newMatrix;
        }





    
        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> StepSearchСircuit = delegate { };

       

        /// <summary>
        /// Нахождение контуров
        /// </summary>
        /// <returns>контуры List<Сircuit></returns>
        public async Task<List<Сircuit>> SearchСircuit()
        {
           

            List<Сircuit> _сircuit = new List<Сircuit>();

            List<List<int>> _items = new List<List<int>>();

            bool[,] _matrix = ReachabilityMatrix = AdjacencyMatrix;// матрица хранящая возведенную в степень матрицу



            return await Task.Run(() =>
            {





                for (int stage = 2; stage < SizeMatrix + 1; stage++)
            {
                StepSearchСircuit(this, new EventMessage("Step: " + stage.ToString(), stage));

                //Перемножение номер: " + stage+1 + " \n";
                _matrix = MatrixMethods.MultiplicationMatrix(_matrix, AdjacencyMatrix);//перемножение матриц при помощи метода Mul
                ReachabilityMatrix = MatrixMethods.SummationMatrix(ReachabilityMatrix, _matrix);
                //
                List<int> _cir = MatrixMethods.SearchСircuitVertices(_matrix);
                if (!_items.Contains(_cir) && _cir.Count != 0)
                {


                    if (_cir.Count <= stage)
                    {
                        _сircuit.Add(new Сircuit(_cir, AdjacencyMatrix));
                        _items.Add(_cir);
                    }
                    else
                    {
                        List<List<int>> _it= FragmentationCircuit(stage, _cir);
                        _items.AddRange(_it);

                        foreach(List<int> i in _it)
                        _сircuit.Add(new Сircuit(i, AdjacencyMatrix));
                    }
                }
            }


           
            return _сircuit;

            });
        }






        /// <summary>
        /// Поиск блока true значений в матрице связанности
        /// </summary>
        /// <param name="stage">степень матрицы смежности</param> 
        /// /// <param name="_start">вершина контура</param>
        /// <returns>точненные вершина контура</returns>
        private bool SearchBlock(int stage, int _start, List<int> _сir)
        {
            for (int height =_start;height<stage+_start ; height++)
                for (int width =_start; width< stage + _start; width++)
                    if (СonnectivityMatrix[height, width] != true&&_сir.Contains(height) && _сir.Contains(width))
                        return false;
                
           

            return true;
        }

        /// <summary>
        /// "Уточнение" контура(разбиение полученных вершин на несколько контуров)
        /// </summary>
        /// <param name="stage">степень матрицы смежности</param>
        /// <param name="_сir">список вершин контура</param>
        /// <returns>полученных вершин на несколько контуров</returns>
        private List<List<int>> FragmentationCircuit(int stage, List<int> _сir)
        {
            List<List<int>> _сircuits = new List<List<int>>();
            for (int index = 0;index < _сir.Count && _сir[index] - 1 <= SizeMatrix - stage  ;)
            {
                List<int> item = new List<int>();

                if (SearchBlock(stage, _сir[index] - 1, _сir))
                {

                    for (int i = index; i < stage + index; i++)
                        item.Add(_сir[i]);

                    _сircuits.Add(item);
                    index += stage;
                }
                else
                    index++;

            }
            return _сircuits;
        }


    }



    /// <summary>
    /// Методы пр работе с матрицей
    /// </summary>
    internal static class MatrixMethods
    {

        /// <summary>
        /// Нахождение вершин контура bool матрицы
        /// </summary>
        /// <param name="_matrix">матрица bool[,]</param>
        /// <returns>List<int> вершин контура</int></returns>
        public static List<int> SearchСircuitVertices(bool[,] _matrix)
        {
            List<int> list = new List<int>();
            // Анализ матрицы, нахождение контуров
            for (int height = 0; height < _matrix.GetLength(0); height++)
                for (int width = 0; width < _matrix.GetLength(1); width++)
                    if ((height == width) & (_matrix[height, width] == true))
                        list.Add(height + 1);

            return list;
        }



        /// <summary>
        /// Перемножение bool матриц
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <param name="_matrix2">матрица 2</param>
        /// <returns>матрицу bool[,]</returns>
        public static bool[,] MultiplicationMatrix(bool[,] _matrix1, bool[,] _matrix2)
        {
            if (_matrix1.GetLength(1) != _matrix2.GetLength(0))
                return null;

            var result = new bool[_matrix1.GetLength(0), _matrix2.GetLength(1)];
            for (int height = 0; height < result.GetLength(0); height++)
                for (int width = 0; width < result.GetLength(1); width++)
                {
                    result[width, height] = false;
                    for (var k = 0; k < _matrix1.GetLength(1); k++)
                        result[width, height] |= _matrix1[width, k] && _matrix2[k, height];
                }

            return result;
        }

        /// <summary>
        /// Перемножение bool матриц по элементно
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <param name="_matrix2">матрица 2</param>
        /// <returns>матрицу bool[,]</returns>
        public static bool[,] MultiplicationElementsMatrix(bool[,] _matrix1, bool[,] _matrix2)
        {
            if (_matrix1.GetLength(1) != _matrix2.GetLength(0))
                return null;

            var result = new bool[_matrix1.GetLength(0), _matrix2.GetLength(1)];
            for (int height = 0; height < result.GetLength(0); height++)
                for (int width = 0; width < result.GetLength(1); width++)
                {
                    result[width, height] = _matrix1[width, height] && _matrix2[width, height];
                }

            return result;
        }


        /// <summary>
        /// Суммирование bool матриц по элементно
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <param name="_matrix2">матрица 2</param>
        /// <returns>матрицу bool[,]</returns>
        public static bool[,] SummationMatrix(bool[,] _matrix1, bool[,] _matrix2)
        {
            if (_matrix1.GetLength(1) != _matrix2.GetLength(0))
                return null;

            var result = new bool[_matrix1.GetLength(0), _matrix2.GetLength(1)];
            for (int height = 0; height < result.GetLength(0); height++)
                for (int width = 0; width < result.GetLength(1); width++)
                {
                    result[width, height] = _matrix1[width, height] || _matrix2[width, height];
                }

            return result;
        }



        /// <summary>
        /// транспонирование bool матрицы 
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <returns>транспонированную матрицу bool[,]</returns>
        public static bool[,] TranspositionMatrix(bool[,] _matrix)
        {
            var result = new bool[_matrix.GetLength(0), _matrix.GetLength(1)];
            for (int height = 0; height < result.GetLength(0); height++)
                for (int width = 0; width < result.GetLength(1); width++)
                {
                    result[width, height] = _matrix[width, height];
                }

            return result;
        }


    }
}
