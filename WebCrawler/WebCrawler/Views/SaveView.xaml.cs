using System.Windows;
using WebCrawler.ViewModels;

namespace WebCrawler.Views {
    /// <summary>
    /// Interaction logic for SaveView.xaml
    /// </summary>
    public partial class SaveView : Window {
        public SaveView() {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            DataContext = vm = new SaveViewModel();
            vm.Close += CloseWindow;
        }

        private void CloseWindow(string siteName) {
            SiteName = siteName;
            Close();
        }

        SaveViewModel vm;
        public string SiteName { get; private set; }
    }
}
