using Microsoft.Xaml.Behaviors.Core;
using System;
using WebCrawler.Navigation;
using WebCrawler.ViewModels;
using WebCrawler.Views;

namespace WebCrawler {
    public class MainwindowViewModel : ViewModelBase {
        public MainwindowViewModel() {
            LaunchCommand = new ActionCommand(LaunchAction);
        }

        private void LaunchAction() {
            Navigator.INSTANCE.RequestNavigate(new CrawlerView());
        }

        private object _content;
        public object Content {
            get => _content;
            set {
                _content = value;
                RaisePropertyChanged();
            }
        }


        public ActionCommand LaunchCommand { get; }

    }
}
