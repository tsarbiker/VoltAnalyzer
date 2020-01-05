using VoltAnalyzer.ViewModel;
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
using VoltAnalyzer.Utils.Controls;

namespace VoltAnalyzer.Views.Home
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Window, IWindow
    {
        public HomeView()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        public void Expand()
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        public void Minimize()
        {
            this.WindowState = WindowState.Minimized;
        }

        public void Move()
        {
                this.DragMove();
        }
    }
}
