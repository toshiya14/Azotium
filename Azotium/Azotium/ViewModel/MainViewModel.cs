using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Azotium.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private WindowViewModel _CurrentFloatWindow;
        private bool _IsMainWindowBusy;
        private string _ProcessingTaskMessage;
        private static readonly JsonSerializerSettings _SubWindowJsonSettings;
        private static readonly string _SubWindowConfigPath = Path.Combine(App.RootPath, "swconf.json");

        public RelayCommand CMDCreateFloatWindow { get; private set; }

        public RelayCommand CMDClosingMainWindow { get; private set; }

        public ObservableCollection<WindowViewModel> FloatWindowSet { get; set; }

        public bool IsMainWindowBusy
        {
            get => _IsMainWindowBusy;
            set
            {
                Set(ref _IsMainWindowBusy, value);
                RaisePropertyChanged("OverlayVisibility");
                RaisePropertyChanged("IsUILocked");
            }
        }

        public Visibility OverlayVisibility
        {
            get => IsMainWindowBusy ? Visibility.Visible : Visibility.Hidden;
        }

        public bool IsUILocked
        {
            get => !IsMainWindowBusy;
        }

        public string ProcessingTaskMessage
        {
            get => _ProcessingTaskMessage;
            set => Set(ref _ProcessingTaskMessage, value);
        }

        public WindowViewModel CurrentFloatWindow
        {
            get => _CurrentFloatWindow;
            set
            {
                Set(ref _CurrentFloatWindow, value);
                RaisePropertyChanged("FloatWindowSet");
                RaisePropertyChanged("CanRightPanelOperated");
            }
        }

        public bool CanRightPanelOperated
        {
            get => CurrentFloatWindow != null;
        }

        static MainViewModel()
        {
            _SubWindowJsonSettings = new JsonSerializerSettings()
            {
                CheckAdditionalContent = true,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            IsMainWindowBusy = true;
            ProcessingTaskMessage = "正在初始化浮窗...";
            InitializeFloatWindowSet();
            IsMainWindowBusy = false;

            CMDCreateFloatWindow = new RelayCommand(() => 
            {
                if (FloatWindowSet.Count >= 200)
                {
                    MessageBox.Show("浮窗总数已达到最大数量，无法创建200以上个浮窗。", "发生错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var vm = new WindowViewModel()
                {
                    Name = GetNewName(),
                    Address = "about:blank"
                };
                var wnd = new SubWindow()
                {
                    DataContext = vm
                };
                vm.BindingWindow = wnd;
                wnd.Show();
                FloatWindowSet.Add(vm);
                CurrentFloatWindow = FloatWindowSet.Last();
            });
            CMDClosingMainWindow = new RelayCommand(() => 
            {
                SaveFloatWindowSet();
                foreach (var wnd in (from vm in FloatWindowSet where vm.BindingWindow != null select vm.BindingWindow))
                {
                    wnd.Close();
                }
            });
        }

        private string GetNewName()
        {
            var newNameTemplate = "New #";
            for (var i = 1; i < 200; i++)
            {
                var name = newNameTemplate += i;
                var result = from fw in FloatWindowSet where fw.Name.Equals(name, System.StringComparison.Ordinal) select fw;
                if (result.Count() == 0)
                {
                    return name;
                }
            }
            return string.Empty;
        }

        private void InitializeFloatWindowSet()
        {
            try
            {
                if (File.Exists(_SubWindowConfigPath))
                {
                    var content = File.ReadAllText(_SubWindowConfigPath);
                    FloatWindowSet = JsonConvert.DeserializeObject<ObservableCollection<WindowViewModel>>(content, _SubWindowJsonSettings);
                    CurrentFloatWindow = FloatWindowSet.First();
                    foreach(var vm in FloatWindowSet)
                    {
                        vm.BindingWindow = new SubWindow()
                        {
                            DataContext = vm
                        };
                        vm.BindingWindow.Show();
                    }
                }
                else
                {
                    FloatWindowSet = new ObservableCollection<WindowViewModel>();
                    CurrentFloatWindow = null;
                }
            }
            catch
            {
                FloatWindowSet = new ObservableCollection<WindowViewModel>();
                CurrentFloatWindow = null;
            }
        }

        private void SaveFloatWindowSet()
        {
            try
            {
                File.WriteAllText(_SubWindowConfigPath, JsonConvert.SerializeObject(FloatWindowSet, _SubWindowJsonSettings), Encoding.UTF8);
            }
            catch { }
        }
    }
}