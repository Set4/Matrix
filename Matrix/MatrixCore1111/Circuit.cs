using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    /// <summary>
    /// Контур
    /// </summary>
  public  class Сircuit
    {
        private List<Current> _currentList;
        public List<Current> CurrentList
        {
            get
            {
                 return _currentList.OrderBy(x => x.Position).ToList();

            }

        }

        public void AddСircuitItem(Current item)
        {
            Current cur = _currentList.Where(i => i.StartingPoint == item.StartingPoint && i.FinalPoint == item.FinalPoint).FirstOrDefault();
            if (cur != null)
                _currentList[_currentList.IndexOf(cur)].Amount++;
            else
                _currentList.Add(item);
        }

     
        /// <summary>
        /// создание контура не требующего уточнения/упорядочивания
        /// </summary>
        public Сircuit(List<Current> currents)
        {
            _currentList = currents;
        }

        /// <summary>
        /// создание упорядоченного контура
        /// </summary>
        /// <param name="_сircuit">вершины контура</param>
        /// <param name="_adjacencyMatrix">матрица смежности</param>
        public Сircuit(List<int> _сircuit, bool[,] _adjacencyMatrix)
        {
         //neobhodim graf gentq

          

            _currentList = new List<Current>();

            for (int i = 0; i < _сircuit.Count; i++)
            {
                if(i+1== _сircuit.Count)
                _currentList.Add(new Current(i, _сircuit[i], _сircuit[0]));
                else
                    _currentList.Add(new Current(i, _сircuit[i], _сircuit[i+1]));
            }


        }
    
    }
    
}
