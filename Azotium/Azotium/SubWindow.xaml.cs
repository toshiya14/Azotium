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

namespace Azotium
{
    /// <summary>
    /// SubWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SubWindow : Window
    {
        public bool PosLock { get; set; }
        public SubWindow()
        {
            InitializeComponent();

            wb.MouseDownOnHandler = (sender, args) =>
            {
                if (!PosLock)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        this.WindowState = WindowState.Normal;
                    }
                    if (this.ResizeMode != ResizeMode.NoResize)
                    {
                        this.ResizeMode = ResizeMode.NoResize;
                        this.UpdateLayout();
                    }
                    DragMove();
                }
            };
            wb.MouseUpOnHandler = (sender, args) =>
            {
                if (!PosLock)
                {
                    if (this.ResizeMode != ResizeMode.CanResizeWithGrip)
                    {
                        this.ResizeMode = ResizeMode.CanResizeWithGrip;
                        this.UpdateLayout();
                    }
                }
            };
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowUtils.DisableAltF4(this);
            WindowUtils.DisableSystemContextMenu(this);
        }
    }
}
