﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixCore
{
    /// <summary>
    /// Класс Сообщения о событии
    /// </summary>
    public class EventMessage : EventArgs
    {
        public string Message { get; private set; }
        public object Item { get; private set; }

        public EventMessage(string message, object obj = null)
        {
            Message = message;
            Item = obj;
        }
    }

}
