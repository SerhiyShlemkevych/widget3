using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace widget3.Controls.Abstract.Common
{
    public class TileBase : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TileBase));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TileBase));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TileBase));

        public static readonly DependencyProperty SubFontSizeProperty = DependencyProperty.Register("SubFontSize", typeof(int), typeof(TileBase));

        public string Text
        {
            get
            {
                return GetValue(TextProperty) as String;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public ICommand Command
        {
            get
            {
                return GetValue(CommandProperty) as ICommand;
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        public int SubFontSize
        {
            get
            {
                return (int)GetValue(SubFontSizeProperty);
            }
            set
            {
                SetValue(SubFontSizeProperty, value);
            }
        }
    }
}
