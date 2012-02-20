using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataDrivenUI
{
    public class CommandBase: ICommand
    {
        private Action<Object> execute;
        private Predicate<object> canExecute; 
        public CommandBase(Action<Object> execute, Predicate<Object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public static ICommand Empty
        {
            get
            {
                return new CommandBase(p => { });
            }
        }

        public void Execute(object parameter)
        {
            Task.Factory.StartNew(() => execute(parameter));
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute(parameter);
            }
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
