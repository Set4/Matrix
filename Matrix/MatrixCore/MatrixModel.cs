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

        // Перемножение булевых матриц
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

        // нахождение циклов
        public List<List<int>> SearchCycles()
        {
            List<List<int>> cycles = new List<List<int>>();//динамическая матрицы
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


    }
}
