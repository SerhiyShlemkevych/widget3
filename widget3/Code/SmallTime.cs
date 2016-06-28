using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace widget3.Code
{
    public class SmallTime : TileData
    {
        private int _hours;
        private int _minutes;
        [NonSerialized]
        private bool _hasOverflov;

        public SmallTime()
        {

        }

        public SmallTime(string smallTimeSting)
        {
            if (smallTimeSting == null)
            {
                return;
            }

            Regex smallTimeStingRegEx = new Regex(@"[0-9]{1,2}:[0-9]{1,2}$");
            if (!smallTimeStingRegEx.IsMatch(smallTimeSting))
            {
                return;
            }

            var splitedData = smallTimeSting.Split(':');

            int hours;
            int minutes;

            int.TryParse(splitedData[0], out hours);
            int.TryParse(splitedData[1], out minutes);

            if (hours < 0 || minutes < 0)
            {
                _hasOverflov = true;
                return;
            }

            if (hours > 23)
            {
                _hasOverflov = true;
            }
            _hours = hours;
            _minutes = minutes;
        }

        public SmallTime(int hours, int minutes)
        {
            if (hours < 0 || minutes < 0)
            {
                return;
            }
            _hours = hours;
            _minutes = minutes;
            if (hours > 23)
            {
                _hasOverflov = true;
            }
        }

        public bool HasOverflov
        {
            get
            {
                return _hasOverflov;
            }
        }


        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
            }
        }

        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
            }
        }

        public int Milliseconds
        {
            get
            {
                return (_minutes + _hours * 60) * 60 * 1000;
            }
        }

        public static SmallTime operator +(SmallTime left, SmallTime right)
        {
            if (left == null)
            {
                left = new SmallTime();
            }
            if (right == null)
            {
                right = new SmallTime();
            }

            var hours = left.Hours + right.Hours;
            var minutes = left.Minutes + right.Minutes;

            if (minutes < 0)
            {
                hours++;
                minutes -= 60;
            }

            return new SmallTime(hours, minutes);
        }

        public static SmallTime operator -(SmallTime left, SmallTime right)
        {
            if (left == null)
            {
                left = new SmallTime();
            }
            if (right == null)
            {
                right = new SmallTime();
            }

            var hours = left.Hours - right.Hours;
            var minutes = left.Minutes - right.Minutes;

            if (minutes < 0)
            {
                hours--;
                minutes = 60 + minutes;
            }

            if (hours < 0)
            {
                hours = 0;
                minutes = 0;
            }

            return new SmallTime(hours, minutes);
        }

        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}", Hours, Minutes);
        }
    }
}
