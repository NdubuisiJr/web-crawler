using Microsoft.Xaml.Behaviors.Core;
using WebCrawler.Core.Utils;
using WebCrawler.Interfaces;
using WebCrawler.Navigation;
using WebCrawler.ViewModels;
using WebCrawler.Views;
using System.Windows;

namespace WebCrawler {
    public class MainwindowViewModel : ViewModelBase {
        public MainwindowViewModel() {
            LaunchCommand = new ActionCommand(LaunchAction);
            SaveCommand = new ActionCommand(SaveAction);
            OpenCommand = new ActionCommand(OpenAction);
            _dataIo = new DataIO();
        }

        private void OpenAction() {
            if (SaveObject is null) {
                MessageBox.Show("Can not open any website until the crawler is launched", "error");
                return;
            }
            _openDialog = new OpenView();
            _openDialog.DeleteEvent += OpenDialog_DeleteEvent;
            var allFiles = _dataIo.GetSaved();
            _openDialog.files.ItemsSource = allFiles;
            _openDialog.ShowDialog();

            if (_openDialog.CanOpen) {
                var file = _openDialog.files.SelectedItem;
                var data = _dataIo.Open((string)file);
                SaveObject.Open(data);
            }
        }

        private void OpenDialog_DeleteEvent(string item) {
            var result = MessageBox.Show("Are you sure you want to delete?", "warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) {
                _dataIo.Delete(item);
                MessageBox.Show("Deleted!", "Success");
            }
            var allFiles = _dataIo.GetSaved();
            _openDialog.files.ItemsSource = allFiles;
        }

        private void SaveAction() {
            if (SaveObject is null) {
                MessageBox.Show("Can not save any website until the crawler is launched", "error");
                return;
            }
            var saveView = new SaveView();
            saveView.ShowDialog();
            var siteName = saveView.SiteName;
            if (string.IsNullOrWhiteSpace(siteName)) {
                return;
            }
            _dataIo = new DataIO();
            var data = SaveObject.Save();
            data.SiteName = siteName;
            _dataIo.Save(data);
            MessageBox.Show("Saved!", "Success");
        }

        private void LaunchAction() {
            var crawlerView = new CrawlerView();
            SaveObject = crawlerView.DataContext as ISaveOpen;
            Navigator.INSTANCE.RequestNavigate(crawlerView);
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
        public ActionCommand SaveCommand { get; }
        public ActionCommand OpenCommand { get; }

        private DataIO _dataIo;
        private OpenView _openDialog;

        public ISaveOpen SaveObject { get; private set; }
    }
}
