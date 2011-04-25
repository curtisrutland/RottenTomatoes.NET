﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RottenTomatoes.NET.SL {
    public class ResultEventArgs<T> : EventArgs{
        public T Result { get; set; }

        public ResultEventArgs(T result) {
            Result = result;
        }
    }
}
