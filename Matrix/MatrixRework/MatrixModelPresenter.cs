﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixCore;

namespace MatrixRework
{
    class MatrixModelPresenter
    {

        private IView view = null;
        private IMatrixModel model = null;

        /// <summary>
        /// конструктор класса Presenter
        /// </summary>
        /// <param name="view">представление</param>
        /// <param name="model">модель</param>
        public MatrixModelPresenter(IView view, IMatrixModel model)
        {
            this.view = view;
            this.model = model;



            model.EndSearchTornCurrent += Model_EndSearchTornCurrent;
            model.EndSearchСircuit += Model_EndSearchСircuit;

            model.StartSearchTornCurrent += Model_StartSearchTornCurrent;
            model.StartSearchСircuit += Model_StartSearchСircuit;

            model.StepSearchTornCurrent += Model_StepSearchTornCurrent;
            model.StepSearchСircuit += Model_StepSearchСircuit;
        }

        public event EventHandler<EventMessage> StatusEvent = delegate { };


        /// <summary>
        /// отображения шага вычисления контура
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_StepSearchСircuit(object sender, EventMessage e)
        {
            StatusEvent(this, e);
           // view.StatusView(e.Message, Convert.ToInt32(e.Item));
        }

        /// <summary>
        /// отображения шага вычисления разрыва контура
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_StepSearchTornCurrent(object sender, EventMessage e)
        {
            StatusEvent(this, e);
            //view.StatusView(e.Message, Convert.ToInt32(e.Item));
        }

        /// <summary>
        /// начало вычисления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_StartSearchСircuit(object sender, EventMessage e)
        {
           // view.StatusView(e.Message,0);
        }

        /// <summary>
        /// начало вычисления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_StartSearchTornCurrent(object sender, EventMessage e)
        {
           // view.StatusView(e.Message,0);
        }


        /// <summary>
        /// обработчик события завершения вычисления контура
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_EndSearchСircuit(object sender, EventMessage e)
        {
           // view.StatusView(e.Message, 100);


            string s="";
            int i = 0;
            foreach (Сircuit c in e.Item as List<Сircuit>)
            {
                s += i + ": ";
                foreach (Current cur in c.CurrentList)
                {
                    s += cur.StartingPoint + "-" + cur.FinalPoint;
                }
                s += "\r\n"; i++;
            }
            view.ViewСircuit(s);
        }

        /// <summary>
        /// обработчик события завершения вычисления разрыва контура
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_EndSearchTornCurrent(object sender, EventMessage e)
        {
           // view.StatusView(e.Message, 100);

            string s1 = "";
          
            foreach (KeyValuePair<Current, List<Сircuit>> cur in e.Item as Dictionary<Current, List<Сircuit>>)
            {
                s1 += cur.Key.StartingPoint + "-" + cur.Key.FinalPoint + ";\n";
                foreach (Сircuit c in cur.Value)
                {
                    foreach (Current c1 in c.CurrentList)
                        s1 += c1.StartingPoint + "-" + c1.FinalPoint + " ";

                    s1 += "; ";
                }

                s1 += "\n";
            }

            view.ViewTornCurrent(s1);
        }





        /// <summary>
        /// запуск вычисления контура и разрыва
        /// </summary>
        /// <param name="matrix"></param>
        public async void SearthAll(string[,] matrix)
        {
            model.CreateMatrix(matrix.GetLength(0), matrix.GetLength(0), model.ConvertMatrixToBool(matrix));

            List<Сircuit> _circ = await model.SearchСircuitAsync();
            await model.SearchTornCurrentAsync(_circ);
        }

        /// <summary>
        /// запуск вычисления контура
        /// </summary>
        /// <param name="matrix"></param>
        public async void SearthSearchСircuit(string[,] matrix)
        {
            model.CreateMatrix(matrix.GetLength(0), matrix.GetLength(0), model.ConvertMatrixToBool(matrix));
            await model.SearchСircuitAsync();
            
        }


    /// <summary>
    /// открытие файла 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
        public async Task<List<List<string>>> OpentxtFile(string path)
        {
          return await FileProvider.ReadFile(path);
        }


        /// <summary>
        /// открытие файла 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[,] OpenExcelFile(string path)
        {
            return  ExelProvider.ReadExcelFile(path);
        }

        /// <summary>
        /// сохранение в файл
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        public void SavetxtFile(string file, string path)
        {
            FileProvider.SaveFileHow(file, path);
        }

    }
}
