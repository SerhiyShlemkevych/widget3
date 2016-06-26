using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace widget3.Views.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        private Window _fakeWindow;

        public MainWindow()
        {
            InitializeComponent();
            HideIcon();
        }

        private void HideIcon() //hide from alt+tab page
        {
            _fakeWindow = new Window();
            _fakeWindow.Top = -100;
            _fakeWindow.Left = -100;
            _fakeWindow.Width = 1;
            _fakeWindow.Height = 1;
            _fakeWindow.WindowStyle = WindowStyle.ToolWindow;
            _fakeWindow.ShowInTaskbar = false;
            _fakeWindow.Show();
            this.Owner = _fakeWindow;
            _fakeWindow.Hide();
        }
    }
}
