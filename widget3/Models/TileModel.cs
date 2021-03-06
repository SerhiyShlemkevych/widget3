﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using widget3.Code;
using widget3.Enums;

namespace widget3.Models
{
    public class TileModel
    {
        public double Transperency
        {
            get;
            set;
        }

        public TileData Data
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

        public BackgroundModel Background
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

        public int Row
        {
            get;
            set;
        }

        public int Column
        {
            get;
            set;
        }
    }
}
