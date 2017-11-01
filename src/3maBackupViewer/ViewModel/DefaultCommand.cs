using System;
using System.Windows.Input;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel
{
    public class DefaultCommand : ICommand
    {
        private readonly Action<object> execute;
        private bool canExecute;

        public DefaultCommand(Action<object> execute, bool canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void SetCanExecute(bool newCanExecute)
        {
            this.canExecute = newCanExecute;
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}