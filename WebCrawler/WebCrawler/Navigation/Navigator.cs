namespace WebCrawler.Navigation {
    public class Navigator : INavigator {
        private MainwindowViewModel _viewModel;

        private static INavigator _navigator;

        public static INavigator INSTANCE {
            get {
                if(_navigator is null) {
                    _navigator = new Navigator();
                }
                return _navigator;
            }
        }

        private Navigator() {}

        public MainwindowViewModel SetNavigatorContext(MainwindowViewModel viewModel) {
            _viewModel = viewModel;
            return viewModel;
        }

        public void RequestNavigate(object view) {
            _viewModel.Content = view;
        }
    }
}
