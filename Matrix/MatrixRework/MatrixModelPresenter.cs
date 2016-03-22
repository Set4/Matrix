using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixCore;

namespace MatrixRework
{
    class MatrixModelPresenter
    {

        private MainWindow view = null;
        private MatrixModel model = null;


        public MatrixModelPresenter(MainWindow view, MatrixModel model)
        {
            this.view = view;
            this.model = model;
        }


        public void Proverka(string[,] matrix)
        {
            
        }

        public void CreateMatrix()
        {
            string s="";
            string s1 = "";
            model.CreateMatrix(view.ReadTable().Length, view.ReadTable().Length, model.ConvertMatrixToBool(view.ReadTable()));
            int i = 0;
            foreach (Сircuit c in model.SearchСircuit())
            {
                s += i + ": ";
                foreach (Current cur in c.CurrentList)
                {
                    s += cur.StartingPoint + "-" + cur.FinalPoint;
                }
                s += "\r\n";i++;
            }
            view.vievCirkuit(s);

            foreach (KeyValuePair<Current, List<Сircuit>> cur in model.SearchTornCurrent(model.SearchСircuit()))
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

            view.vievrazrivi(s1);
        }
    }
}
