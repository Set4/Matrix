using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    /// <summary>
    /// Поток
    /// </summary>
    public class Current
    {
        /// <summary>
        /// позиция потока в контуре
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// начало потока
        /// </summary>
        public int StartingPoint { get; set; }

        /// <summary>
        /// конец потока
        /// </summary>
        public int FinalPoint { get; set; }

        /// <summary>
        /// кол-во одинаковых потоков в цикле
        /// </summary>
        public int Amount { get; set; }

        public Current(int position, int startpoint, int finalpoint, int amount=1)
        {
            Position = position;
            StartingPoint = startpoint;
            FinalPoint = finalpoint;
            Amount = amount;
        }
    }
}
