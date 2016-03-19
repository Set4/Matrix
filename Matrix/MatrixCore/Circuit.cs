using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    /// <summary>
    /// Контур
    /// </summary>
    public class Сircuit
    {
        private List<Current> _currentList;
        public List<Current> CurrentList
        {
            get
            {
                // return _currentList.OrderBy(x => x.Position).ToList();
                _currentList.Sort();
                return _currentList;
            }

        }

        public void AddСircuitItem(Current item)
        {
            int index = _currentList.IndexOf(item);
            if (index != -1)
                _currentList[index].Amount++;
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
            List<Current> _currentList = new List<Current>();

            foreach (int item in _сircuit)
                for (int width = 0; width < _adjacencyMatrix.Length; width++)
                    if (_adjacencyMatrix[item, width] == true && _сircuit.Contains(width))
                        _currentList.Add(new Current(item, item, width));

        }
    
    }
    
}
