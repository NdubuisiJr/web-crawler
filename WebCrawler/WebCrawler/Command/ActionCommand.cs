using System;
using System.Windows.Input;

namespace WebCrawler.Command {
    public class ActionCommand<T> : ICommand {
        private Action<T> _execute;

        public ActionCommand(Action<T> execute) {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            _execute.Invoke((T)parameter);
        }
    }
}
