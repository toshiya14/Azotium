using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Azotium.Controls
{
    /// <summary>
    /// WebBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class WebBrowser : UserControl
    {
        public Action<object, MouseButtonEventArgs> MouseDownOnHandler { get; set; }
        public Action<object, MouseButtonEventArgs> MouseUpOnHandler { get; set; }

        public DependencyPropertyChangedEventHandler TitleChanged;

        public WebBrowser()
        {
            InitializeComponent();
            this.ctrlBrowser.TitleChanged += TitleChanged;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseDownOnHandler != null)
            {
                MouseDownOnHandler.Invoke(sender, e);
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseUpOnHandler != null)
            {
                MouseUpOnHandler.Invoke(sender, e);
            }
        }

        public void ShowDevTools()
        {
            this.ctrlBrowser?.GetBrowser()?.GetHost()?.ShowDevTools();
        }
    }
}
