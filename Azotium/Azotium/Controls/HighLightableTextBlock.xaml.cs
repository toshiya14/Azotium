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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;

namespace Azotium.Controls
{
    /// <summary>
    /// HighLightableTextBlock.xaml 的交互逻辑
    /// </summary>
    public partial class HighLightableTextBlock : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HighLightableTextBlock), new PropertyMetadata("Item"));



        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(HighLightableTextBlock), new PropertyMetadata(false));



        public HighLightableTextBlock()
        {
            InitializeComponent();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsSelected)
            {
                var storyboard = Resources["sbHover"] as Storyboard;
                if (storyboard != null)
                {
                    storyboard.Begin();
                }
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            var storyboard = Resources["sbHoverLeave"] as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin();
            }
        }
    }
}
