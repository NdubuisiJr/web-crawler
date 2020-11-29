using System;
using System.Windows;

namespace WebCrawler.Views {
    /// <summary>
    /// Interaction logic for OpenView.xaml
    /// </summary>
    public partial class OpenView : Window {
        public OpenView() {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            CanOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            CanOpen = true;
            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            DeleteEvent?.Invoke((string)files.SelectedItem);
        }

        public event Action<string> DeleteEvent;
        public bool CanOpen { get; private set; }
    }
}
