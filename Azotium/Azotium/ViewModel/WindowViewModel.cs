using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Azotium.ViewModel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WindowViewModel : ViewModelBase
    {
        private string _Address;
        private string _Name;
        private int _Opacity;
        private bool _IsMouseThrough;
        private bool _IsPosLocked;
        private bool _IsTopMost;
        private bool _IsAltTabHidden;
        private bool _IsActivated;
        private int _X;
        private int _Y;
        private int _Width;
        private int _Height;
        private SubWindow _BindingWindow;

        [JsonProperty(PropertyName = "_id")]
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        [JsonProperty(PropertyName = "url")]
        public string Address
        {
            get => _Address;
            set => Set(ref _Address, value);
        }

        [JsonProperty(PropertyName = "_x")]
        public int X
        {
            get => _X;
            set => Set(ref _X, value);
        }

        [JsonProperty(PropertyName = "_y")]
        public int Y
        {
            get => _Y;
            set => Set(ref _Y, value);
        }

        [JsonProperty(PropertyName = "width")]
        public int Width
        {
            get => _Width;
            set => Set(ref _Width, value);
        }

        [JsonProperty(PropertyName = "height")]
        public int Height
        {
            get => _Height;
            set => Set(ref _Height, value);
        }

        [JsonProperty(PropertyName = "opacity")]
        public int Opacity
        {
            get => _Opacity;
            set
            {
                if (value < 0)
                {
                    Set(ref _Opacity, 0);
                }
                else if (value > 100)
                {
                    Set(ref _Opacity, 100);
                }
                else
                {
                    Set(ref _Opacity, value);
                }
                RaisePropertyChanged("OpacityFloat");
            }
        }

        [JsonIgnore]
        public float OpacityFloat
        {
            get => _Opacity / 100f;
        }

        [JsonProperty(PropertyName = "mouseThrough")]
        public bool IsMouseThrough
        {
            get => _IsMouseThrough;
            set
            {
                Set(ref _IsMouseThrough, value);
                if (value && BindingWindow != null)
                {
                    WindowUtils.SetWindowClickThrough(BindingWindow);
                }
                else if (!value && BindingWindow != null)
                {
                    WindowUtils.SetWindowNotClickThrough(BindingWindow);
                }
            }
        }

        [JsonProperty(PropertyName = "activated")]
        public bool IsActivated
        {
            get => _IsActivated;
            set
            {
                Set(ref _IsActivated, value);
                if (value && this.BindingWindow == null)
                {
                    this.BindingWindow = new SubWindow();
                    this.BindingWindow.DataContext = this;
                    this.BindingWindow.Loaded += BindingWindowLoaded;
                    this.BindingWindow.TitleChanged += BindingWindowTitleChanged;
                    this.BindingWindow.Show();
                }
                else if(!value && this.BindingWindow != null)
                {
                    this.BindingWindow.Close();
                    this.BindingWindow = null;
                }
            }
        }

        [JsonProperty(PropertyName = "hideAltTab")]
        public bool IsAltTabHidden
        {
            get => _IsAltTabHidden;
            set
            {
                Set(ref _IsAltTabHidden, value);
                if (value && BindingWindow != null)
                {
                    WindowUtils.HideAltTabShown(BindingWindow);
                }
                else if (!value && BindingWindow != null)
                {
                    WindowUtils.RecoverAltTabShown(BindingWindow);
                }
            }
        }

        [JsonProperty(PropertyName = "posLocked")]
        public bool IsPosLocked
        {
            get => _IsPosLocked;
            set
            {
                Set(ref _IsPosLocked, value);
                RaisePropertyChanged("HandleVisibility");

                if (!value && BindingWindow != null)
                {
                    BindingWindow.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
                    BindingWindow.PosLock = false;
                    BindingWindow.UpdateLayout();
                }
                else if (value && BindingWindow != null)
                {
                    BindingWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                    BindingWindow.PosLock = true;
                    BindingWindow.UpdateLayout();
                }
            }
        }

        [JsonProperty(PropertyName = "topMost")]
        public bool IsTopMost
        {
            get => _IsTopMost;
            set
            {
                Set(ref _IsTopMost, value);

                if (BindingWindow != null)
                {
                    BindingWindow.Topmost = value;
                }
            }
        }

        public RelayCommand ShowDevToolsCommand { get; set; }


        public SubWindow BindingWindow
        {
            get => _BindingWindow;
            set
            {
                Set(ref _BindingWindow, value);
                if (BindingWindow != null && !BindingWindow.IsLoaded && BindingWindow.Visibility != Visibility.Visible)
                {
                    BindingWindow.Loaded += BindingWindowLoaded;
                }
            }
        }

        private void BindingWindowLoaded(object sender, RoutedEventArgs args)
        {
            IsMouseThrough = IsMouseThrough;
            IsPosLocked = IsPosLocked;
            IsTopMost = IsTopMost;
            IsAltTabHidden = IsAltTabHidden;
            IsActivated = IsActivated;
        }

        private void BindingWindowTitleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            this.BindingWindow.Title = "Azotium - " + args.NewValue.ToString();
        }

        public Visibility HandleVisibility
        {
            get => IsPosLocked ? Visibility.Hidden : Visibility.Visible;
        }

        public WindowViewModel()
        {
            this.Address = "about:blank;";
            this.IsAltTabHidden = true;
            this.IsMouseThrough = false;
            this.IsPosLocked = false;
            this.IsTopMost = true;
            this.Opacity = 100;
            ShowDevToolsCommand = new RelayCommand(ShowDevTools);
        }

        public void ShowDevTools()
        {
            this.BindingWindow.ShowDevTools();
        }

        public static Rect GetDefaultPosSize(Screen cws)
        {
            var defaultHeight = 300;
            var defaultWidth = 500;

            var _CWSCenter = new Point()
            {
                X = cws.WorkingArea.Left + cws.WorkingArea.Width / 2,
                Y = cws.WorkingArea.Top + cws.WorkingArea.Height / 2
            };

            var X = Convert.ToInt32(_CWSCenter.X - defaultWidth / 2);
            var Y = Convert.ToInt32(_CWSCenter.Y - defaultHeight / 2);

            return new Rect()
            {
                X = X,
                Y = Y,
                Height = defaultHeight,
                Width = defaultWidth
            };
        }


    }
}
