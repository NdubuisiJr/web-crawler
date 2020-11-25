namespace WebCrawler.Navigation {
    public interface INavigator {
        void RequestNavigate(object view);
        MainwindowViewModel SetNavigatorContext(MainwindowViewModel viewModel);
    }
}
