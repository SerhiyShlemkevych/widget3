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
using System.Windows.Navigation;
using System.Windows.Shapes;
using widget3.Converters;

namespace widget3.Views.Settings.CreateTilePages
{
    /// <summary>
    /// Interaction logic for SelectTimePage.xaml
    /// </summary>
    public partial class SelectTimePage : Page
    {
        public SelectTimePage()
        {
            Resources.Add("SmallTimeConverter", new SmallTimeConverter());
            InitializeComponent();
        }
    }
}
