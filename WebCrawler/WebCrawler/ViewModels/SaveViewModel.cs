
using Microsoft.Xaml.Behaviors.Core;
using System;

namespace WebCrawler.ViewModels {
    public class SaveViewModel : ViewModelBase {
        public SaveViewModel() {
            SaveCommand = new ActionCommand(SaveAction);
        }

        private void SaveAction() {
            Close?.Invoke(SiteName);
        }

        private string _steName;
        public string SiteName {
            get => _steName;
            set {
                _steName = value;
                RaisePropertyChanged();
            }
        }

        internal event Action<string> Close;
        public ActionCommand SaveCommand { get; }
    }
}
