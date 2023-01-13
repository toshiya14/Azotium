using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azotium.ViewModel
{
    public class ConfigViewModel : ViewModelBase
    {
        private ConfigItem _SelectedItem;

        public ObservableCollection<ConfigItem> ConfigItems { get; set; }
        public ConfigItem SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }

        public RelayCommand<string> CMDSetSelectedItem { get; set; }

        public ConfigViewModel()
        {
            this.ConfigItems = new ObservableCollection<ConfigItem>();
            this.ConfigItems.Add(new ConfigItem() { Name = "index", DisplayName = "首选项" });
            this.ConfigItems.Add(new ConfigItem() { Name = "global", DisplayName = "全局设定" });
            this.ConfigItems.Add(new ConfigItem() { Name = "browser", DisplayName = "浏览器" });
            this.ConfigItems.Add(new ConfigItem() { Name = "customize", DisplayName = "个性化" });

            this.CMDSetSelectedItem = new RelayCommand<string>(name =>
            {
                var result = from i in ConfigItems where i.Name.Equals(name, StringComparison.Ordinal) select i;
                if (result.Count() > 0)
                {
                    if (this.SelectedItem != null)
                    {
                        this.SelectedItem.IsSelected = false;
                    }
                    this.SelectedItem = result.First();
                    this.SelectedItem.IsSelected = true;
                }
                else
                {
                    this.SelectedItem = null;
                }
            });
        }
    }

    public class ConfigItem : ViewModelBase
    {
        private string _Name;
        private string _DisplayName;
        private bool _IsSelected;
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        public string DisplayName
        {
            get => _DisplayName;
            set => Set(ref _DisplayName, value);
        }
        public bool IsSelected
        {
            get => _IsSelected;
            set => Set(ref _IsSelected, value);
        }
    }
}
