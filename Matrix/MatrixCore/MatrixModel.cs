using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    class MatrixModel
    {
        private int _sizeMatrix;

        /// <summary>
        /// Размер матрицы
        /// </summary>
        public int SizeMatrix
        {
            get { return _sizeMatrix; }
            private set
            {

                if (value <= 0)
                    _sizeMatrix = 0;//можем вызывать событие ошибки
                else
                    _sizeMatrix = value;
            }
        }

        /// <summary>
        /// Матрица
        /// </summary>
        public bool[,] Matrix { get; private set; }

        /// <summary>
        /// Создание пустой матрицы
        /// </summary>
        /// <param name="sizeMatrix">размер матрицы</param>
        public MatrixModel(int sizeMatrix)
        {
            SizeMatrix = sizeMatrix;
            Matrix = new bool[SizeMatrix, SizeMatrix];
        }

        /// <summary>
        /// Заполнение матрицы значениями
        /// </summary>
        /// <param name="matrix">входная матрица</param>
        public void FillingMatrix(bool[,] matrix)
        {
            for (int height = 0; height < SizeMatrix; height++)
                for (int width = 0; width < SizeMatrix; width++)
                    Matrix[height, width] = matrix[height, width];
        }

        /// <summary>
        /// Перемножение bool матриц
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <param name="_matrix2">матрица 2</param>
        /// <returns></returns>
        private static bool[,] MultiplicationMatrix(bool[,] _matrix1, bool[,] _matrix2)
        {
            if (_matrix1.GetLength(1) != _matrix2.GetLength(0))
                ; //событие ошибки

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
        /// Нахождение циклов
        /// </summary>
        /// <returns>List<List<int>></returns>
        public List<List<int>> SearchCycles()
        {
            List<List<int>> cycles = new List<List<int>>();//динамическая матрица
            bool[,] matrixClone = Matrix;//копия матрицы

            for (int stage = 0; stage < SizeMatrix - 1; stage++)
            {
                //Перемножение номер: " + stage+1 + " \n";
                matrixClone = MultiplicationMatrix(matrixClone, Matrix);//перемножение матриц при помощи метода Mul

                List<int> list = new List<int>();
                // Анализ матрицы, нахождение циклов
                for (int height = 0; height < SizeMatrix; height++)
                    for (int width = 0; width < SizeMatrix; width++)
                        if ((height == width) & (matrixClone[height, width] == true))
                            list.Add(height + 1);
            }
            return cycles;
        }


        /// <summary>
        /// Изменение размера матрицы с сохранением значений
        /// </summary>
        /// <param name="size">новый размер матрицы</param>
        public void CloneMatrix(int size)
        {
            bool[,] newMatrix = new bool[size, size];

            for (int height = 0; height < size; height++)
                for (int width = 0; width < size; width++)
                    newMatrix[height, width] = Matrix[height, width];

            Matrix = newMatrix;
        }

        /// <summary>
        /// Проверка введенной матрицы на наличие не 0 или 1
        /// </summary>
        /// <param name="matrix">проверяемая матрица</param>
        /// <returns>Получаем матрицы с Cells, не равных 1 или 0 </returns>
        public List<List<int[]>> GetBadCellsMatrix(string[,] matrix)
        {
            List<List<int[]>> badMatrix = new List<List<int[]>>();
            int _result;
            bool _b;

            for (int height = 0; height < SizeMatrix; height++)
            {
                List<int[]> _list = new List<int[]>();
                for (int width = 0; width < SizeMatrix; width++)
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
        /// <returns>матрица bool</returns>
        public bool[,] ConvertMatrixToBool(string[,] matrix)
        {
            bool[,] convertMatrix = new bool[SizeMatrix, SizeMatrix];

            for (int height = 0; height < SizeMatrix; height++)
            {
                for (int width = 0; width < SizeMatrix; width++)
                {
                    if (Int32.Parse(matrix[height, width]) == 1)
                        convertMatrix[height, width] = true;
                   else if (Int32.Parse(matrix[height, width]) == 0)
                        convertMatrix[height, width] = false;
                }
            }
            return convertMatrix;
        }

    }
}
