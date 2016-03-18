using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    public static class Methods
    {

        /// <summary>
        /// Перемножение bool матриц
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <param name="_matrix2">матрица 2</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public static bool[,] SummirovanieMatrix(bool[,] _matrix1, bool[,] _matrix2)
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
        /// Transponirovamie bool матрицы 
        /// </summary>
        /// <param name="_matrix1">матрица 1</param>
        /// <param name="_matrix2">матрица 2</param>
        /// <returns></returns>
        public static bool[,] Transponirovanie(bool[,] _matrix)
        {
            var result = new bool[_matrix.GetLength(0), _matrix.GetLength(1)];
            for (int height = 0; height < result.GetLength(0); height++)
                for (int width = 0; width < result.GetLength(1); width++)
                {
                    result[width, height] = _matrix[width, height];
                }

            return result;
        }


        /// <summary>
        /// nahojdenie kontyra bool матрицы 
        /// </summary>
        /// <param name="_matrix">матрица 1</param>
        /// <returns></returns>
        public static List<int> NahojdenieKontyra(bool[,] _matrix)
        {
            List<int> list = new List<int>();
            // Анализ матрицы, нахождение циклов
            for (int height = 0; height < _matrix.GetLength(0); height++)
                for (int width = 0; width < _matrix.GetLength(1); width++)
                    if ((height == width) & (_matrix[height, width] == true))
                        list.Add(height + 1);

            return list;
        }



      

    }


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
        /// Матрица smejnosti
        /// </summary>
        public bool[,] MatrixSmejnosti { get; private set; }

        /// <summary>
        /// Матрица dostijimosti
        /// </summary>
        public bool[,] MatrixDostijimosti { get; private set; }

        /// <summary>
        /// MatrixSvazannosti
        /// </summary>
        public bool[,] MatrixSvazannosti { get
            {
                return Methods.MultiplicationElementsMatrix(MatrixDostijimosti, Methods.Transponirovanie(MatrixDostijimosti));
            }
        }

        /// <summary>
        /// Создание пустой матрицы
        /// </summary>
        /// <param name="sizeMatrix">размер матрицы</param>
        public MatrixModel(int sizeMatrix)
        {
            SizeMatrix = sizeMatrix;
            MatrixSmejnosti = new bool[SizeMatrix, SizeMatrix];
            MatrixDostijimosti = new bool[SizeMatrix, SizeMatrix];
        }




        private List<int> Nahojdeniebloka(int _start, int stage)
        {
            List<int> kontyr = new List<int>();

            for (int a = _start - 1; a < stage; a++)
            {
                for (int b = -1; b < stage; b++)
                    if (MatrixSvazannosti[a, b] != true)
                        return null;
                kontyr.Add(a + 1);
            }

            return kontyr;
        }

        private List<List<int>> YtochninieContyra(int stage, List<int> kontyr)
        {
            List<List<int>> koliakontyrov = new List<List<int>>();
            foreach(int i in kontyr)
            {
                List<int> item = Nahojdeniebloka(i, stage);
                if (item != null)
                    koliakontyrov.Add(item);
            }
            return koliakontyrov;
        }

        /// <summary>
        /// Нахождение kontyrov
        /// </summary>
        /// <returns>List<List<int>></returns>
        public List<List<int>> SearchCycles()
        {
            List<List<int>> cycles = new List<List<int>>();//динамическая матрица
            bool[,] matrixtekysheistepeni =MatrixDostijimosti= MatrixSmejnosti;//копия матрицы

            for (int stage = 0; stage < SizeMatrix - 1; stage++)
            {
                //Перемножение номер: " + stage+1 + " \n";
                matrixtekysheistepeni = Methods.MultiplicationMatrix(matrixtekysheistepeni, MatrixSmejnosti);//перемножение матриц при помощи метода Mul
                MatrixDostijimosti = Methods.SummirovanieMatrix(MatrixDostijimosti, matrixtekysheistepeni);

                List<int> list = Methods.NahojdenieKontyra(matrixtekysheistepeni);
                if (!cycles.Contains(list) && list.Count != 0)
                {
                    if (list.Count <= stage + 1)
                        cycles.Add(list);
                    else
                    cycles.AddRange( YtochninieContyra(stage, list));//ytochnenie kontyra
                }
            }
            return cycles;
        }


        public Dictionary<int, int[]>  YporadochivanieContyra(List<int> contyr)
        {
            Dictionary<int, int[]> kont = new Dictionary<int, int[]>();
            foreach (int i in contyr)
                for (int j = 0; j < MatrixSmejnosti.Length; j++)
                    if (MatrixSmejnosti[i, j] == true && contyr.Contains(j))
                        kont.Add(i, new int[] { i, j });
            return kont;
        }



        public List<List<Dictionary<int, int[]>>> SearchRazrili(List<Dictionary<int, int[]>> contyri)
        {
            List<int[]> vsepotoki=new List<int[]>();
            foreach (Dictionary<int, int[]> item in contyri)
                foreach (KeyValuePair<int, int[]> i in item)
                    if (vsepotoki.BinarySearch(i.Value) == -1)
                        vsepotoki.Add(i.Value);


            Dictionary<int, int[]> potokichactotaego = new Dictionary<int, int[]>();
           
                foreach (Dictionary<int, int[]> item in contyri)
                    foreach (KeyValuePair<int, int[]> i1 in item)
                        if (vsepotoki.BinarySearch(i1.Value) !=-1)
                            
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
                   else if (Int32.Parse(matrix[height, width]) == 0 || matrix[height, width]==String.Empty)
                        convertMatrix[height, width] = false;
                }
            }
            return convertMatrix;
        }

    }
}
