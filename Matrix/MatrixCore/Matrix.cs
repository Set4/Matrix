using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    class Matrix
    {
        /// <summary>
        /// Событие размер матрицы меньше или равно 0
        /// </summary>
        public event EventHandler<EventMessage> ErrorSizeMatrix = delegate { };


        private int _sizeMatrix=0;

        /// <summary>
        /// Размер матрицы
        /// </summary>
        public int SizeMatrix
        {
            get { return _sizeMatrix; }
            private set
            {

                if (value <= 0)
                    ErrorSizeMatrix(this, new EventMessage("размер матрицы меньше или равно 0"));
                else
                    _sizeMatrix = value;
            }
        }



        /// <summary>
        /// Матрица смежности
        /// </summary>
        public bool[,] AdjacencyMatrix { get; private set; }

        /// <summary>
        /// Матрица достижимости
        /// </summary>
        public bool[,] ReachabilityMatrix { get; set; }


        /// <summary>
        /// Matrix высокой связанности
        /// </summary>
        public bool[,] СonnectivityMatrix
        {
            get
            {
                return MultiplicationElementsMatrix(ReachabilityMatrix, TranspositionMatrix(ReachabilityMatrix));
            }
        }


        /// <summary>
        /// Создание пустой матрицы
        /// </summary>
        /// <param name="sizeMatrix">размер матрицы</param>
        public Matrix(int sizeMatrix)
        {
            SizeMatrix = sizeMatrix;
            AdjacencyMatrix = new bool[SizeMatrix, SizeMatrix];
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
        /// Поиск блока true значений в матрице связанности
        /// </summary>
        /// <param name="stage">степень матрицы смежности</param> 
        /// /// <param name="_start">вершина контура</param>
        /// <returns>точненные вершина контура</returns>
        private List<int> SearchBlock(int stage, int _start)
        {
            List<int> _item = new List<int>();

            for (int height = _start - 1; height < stage; height++)
            {
                for (int width = -1; width < stage; width++)
                    if (СonnectivityMatrix[height, width] != true)
                        return null;
                _item.Add(height + 1);
            }

            return _item;
        }

        /// <summary>
        /// "Уточнение" контура(разбиение полученных вершин на несколько контуров)
        /// </summary>
        /// <param name="stage">степень матрицы смежности</param>
        /// <param name="_сircuit">список вершин контура</param>
        /// <returns>полученных вершин на несколько контуров</returns>
        private List<List<int>> FragmentationCircuit(int stage, List<int> _сircuit)
        {
            List<List<int>> _сircuits = new List<List<int>>();
            foreach (int i in _сircuit)
            {
                List<int> item = SearchBlock(i, stage);
                if (item != null)
                    _сircuits.Add(item);
            }
            return _сircuits;
        }



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
        private static bool[,] MultiplicationElementsMatrix(bool[,] _matrix1, bool[,] _matrix2)
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
        private static bool[,] TranspositionMatrix(bool[,] _matrix)
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
