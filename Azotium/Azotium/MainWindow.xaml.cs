using Azotium.ViewModel;
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

namespace Azotium
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var dialog = new ConfigWindow();
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ShowDialog();
        }

        private void ShowFloatingWindowDevTools(object sender, RoutedEventArgs e)
        {
            var mvm = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            var fw = mvm.CurrentFloatWindow;
            fw.ShowDevTools();
        }
    }
}
