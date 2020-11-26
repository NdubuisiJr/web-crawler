using System;
using System.Windows.Input;

namespace WebCrawler.Command {
    public class ActionCommand<T> : ICommand {
        private Action<T> _execute;
        private Predicate<T> _predicate;

        public ActionCommand(Action<T> execute, Predicate<T> predicate = null) {
            _execute = execute;
            _predicate = predicate;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter) {
            return _predicate.Invoke((T)parameter);
        }

        public void Execute(object parameter) {
            _execute.Invoke((T)parameter);
        }
    }
}
