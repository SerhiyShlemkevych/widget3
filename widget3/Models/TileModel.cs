﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Enums;

namespace widget3.Models
{
    public class TileModel
    {
        public object Data
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public string BackgroundColor
        {
            get;
            set;
        }

        public TileType Type
        {
            get;
            set;
        }

        public bool[] Days
        {
            get;
            set;
        } 
    }
}