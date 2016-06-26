using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace widget3.Controls.Abstract.Common
{
    public class TileBase : UserControl, ICommandSource
    {
        public TileBase()
        {
            MouseUp += TileBaseMouseUp;
            SetResourceReference(TileBase.BorderBrushProperty, "SelectedBorderColor");
            Foreground = Brushes.White;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
        }

        private void TileBaseMouseUp(object sender, MouseButtonEventArgs e)
        {
            var command = Command;
            var parameter = CommandParameter;
            var target = CommandTarget;

            var routedCommand = command as RoutedCommand;
            if (routedCommand != null && routedCommand.CanExecute(parameter, target))
            {
                routedCommand.Execute(parameter, target);
            }
            else if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }


        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(TileBase), new UIPropertyMetadata(null));

        public static readonly DependencyProperty FontTransperencyProperty =
            DependencyProperty.Register("FontTransperency", typeof(double), typeof(TileBase), new UIPropertyMetadata(null));


        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TileBase));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TileBase));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TileBase));

        public static readonly DependencyProperty SubFontSizeProperty = DependencyProperty.Register("SubFontSize", typeof(int), typeof(TileBase));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        public double FontTransperency
        {
            get { return (double)GetValue(FontTransperencyProperty); }
            set { SetValue(FontTransperencyProperty, value); }
        }

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
