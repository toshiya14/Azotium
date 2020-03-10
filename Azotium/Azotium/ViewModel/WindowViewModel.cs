using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Azotium.ViewModel
{
    public class WindowViewModel : ViewModelBase
    {
        private string _Address;
        private string _Name;
        private int _Opacity;
        private bool _IsMouseThrough;
        private bool _IsPosLocked;
        private bool _IsTopMost;
        private bool _IsAltTabHidden;
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

        [JsonProperty(PropertyName = "opacity")]
        public int Opacity
        {
            get => _Opacity;
            set
            {
                Set(ref _Opacity, value);
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
                else if(!value && BindingWindow != null)
                {
                    WindowUtils.SetWindowNotClickThrough(BindingWindow);
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
                    WindowUtils.RecoverAltTabShown(BindingWindow);
                }
                else if (!value && BindingWindow != null)
                {
                    WindowUtils.HideAltTabShown(BindingWindow);
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
                else if(value && BindingWindow != null)
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

        [JsonIgnore]
        public SubWindow BindingWindow
        {
            get => _BindingWindow;
            set { 
                Set(ref _BindingWindow, value);
                IsMouseThrough = IsMouseThrough;
                IsAltTabHidden = IsAltTabHidden;
                IsPosLocked = IsPosLocked;
                IsTopMost = IsTopMost;
            }
        }

        [JsonIgnore]
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
        }
    }
}
