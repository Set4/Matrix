using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCore
{
   
    public interface IMatrixModel
    {
        /// <summary>
        /// Событие размер матрицы меньше или равно 0
        /// </summary>
        event EventHandler<EventMessage> ErrorSizeMatrix;

        /// <summary>
        /// Создание матрицы и заполнение матрицы
        /// </summary>
        bool CreateMatrix(int weght, int height, bool[,] _matrix);

        /// <summary>
        /// Переводим string 0 и 1 в bool(true, false)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>матрица bool[,]</returns>
         bool[,] ConvertMatrixToBool(string[,] matrix);




        /// <summary>
        /// Событие старт поиска циклов
        /// </summary>
        event EventHandler<EventMessage> StartSearchСircuit;

        /// <summary>
        /// Событие шаг поиска циклов
        /// </summary>
        event EventHandler<EventMessage> StepSearchСircuit;


        /// <summary>
        /// Событие завершение поиска циклов
        /// </summary>
        event EventHandler<EventMessage> EndSearchСircuit;

        /// <summary>
        /// Асинхронный поиск циклов
        /// </summary>
        Task<List<Сircuit>> SearchСircuitAsync();




        /// <summary>
        /// Событие старт поиска разрываемых потоков
        /// </summary>
        event EventHandler<EventMessage> StartSearchTornCurrent;

        /// <summary>
        /// Событие шаг поиска разрываемых потоков
        /// </summary>
        event EventHandler<EventMessage> StepSearchTornCurrent;


        /// <summary>
        /// Событие завершение поиска разрываемых потоков
        /// </summary>
        event EventHandler<EventMessage> EndSearchTornCurrent;



        /// <summary>
        /// Асинхронный поиск разрываемых потоков
        /// </summary>
        /// <param name="circuits">список всех циклов</param>
        /// <returns>поток-список циклов которые он разрывает</returns>
        Task<Dictionary<Current, List<Сircuit>>> SearchTornCurrentAsync(List<Сircuit> circuits);
    }

  public  class MatrixModel:IMatrixModel
    {
        Matrix matrix { get; set; }


        public MatrixModel()
        {
           
            
        }

      


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
                matrix.StepSearchСircuit += Matrix_StepSearchСircuit;
                return true;
            }
            else
            {

                ErrorSizeMatrix(this, new EventMessage("размер матрицы меньше или равно 0"));
                return false;
            }
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






        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> StartSearchСircuit = delegate { };

        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> StepSearchСircuit = delegate { };


        /// <summary>
        /// обработчик события шаг вычисления контуров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Matrix_StepSearchСircuit(object sender, EventMessage e)
        {
            StepSearchСircuit(this, new EventMessage(e.Message, e.Item));
        }

        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> EndSearchСircuit = delegate { };


        public async Task<List<Сircuit>> SearchСircuitAsync()
        {
            StartSearchСircuit(this, new EventMessage("Нахождение контуров начато", matrix.SizeMatrix));

            List< Сircuit> _сircuit=await matrix.SearchСircuit();

            EndSearchСircuit(this, new EventMessage("Нахождение контуров завершено", _сircuit));

            return _сircuit;
        }






        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> StartSearchTornCurrent = delegate { };

        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> StepSearchTornCurrent = delegate { };


        /// <summary>
        /// Событие 
        /// </summary>
        public event EventHandler<EventMessage> EndSearchTornCurrent = delegate { };



        /// <summary>
        /// поиск разрываемых потоков
        /// </summary>
        /// <param name="circuits">список всех циклов</param>
        /// <returns>поток-список циклов которые он разрывает</returns>
        public async Task<Dictionary<Current, List<Сircuit>>> SearchTornCurrentAsync(List<Сircuit> circuits)
        {
            Dictionary<Current, List<Сircuit>> _dict = new Dictionary<Current, List<Сircuit>>();

            List<Сircuit> list;



            return await Task.Run(() =>
            {

            Сircuit generalСircuit = new Сircuit(new List<Current>());
            foreach (Сircuit circuit in circuits)
                foreach (Current item in circuit.CurrentList)
                {
                    generalСircuit.AddСircuitItem(item);
                }



            StartSearchTornCurrent(this, new EventMessage("поиск разрываемых потоков начат", generalСircuit.CurrentList.Count));

            for (int i = 0; i < generalСircuit.CurrentList.Count; i++)
            {

                    StepSearchTornCurrent(this, new EventMessage("Step: " + i.ToString(), i*100/ generalСircuit.CurrentList.Count));

                        Current item = generalСircuit.CurrentList[i];

                    list = new List<Сircuit>();
                    foreach (Сircuit circuit in circuits)
                        if (circuit.CurrentList.Where(obj => obj.StartingPoint == item.StartingPoint && obj.FinalPoint == item.FinalPoint).ToList().Count != 0)
                            list.Add(circuit);

                    _dict.Add(item, list);
                }



                EndSearchTornCurrent(this, new EventMessage("поиск разрываемых потоков завершен", _dict));
                return _dict;

            });
        }



























        /// <summary>
        /// Событие нахождения неверных значений матрицы
        /// </summary>
        public event EventHandler<EventMessage> BadMatrix = delegate { };


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


    }







}
