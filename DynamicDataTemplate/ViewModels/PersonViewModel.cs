using System.Windows;
using System.Windows.Input;
using DataDrivenUI;

namespace DynamicDataTemplate.ViewModels
{
    public class PersonViewModel: ViewModelBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                NotifyPropertyChanged("Age");
            }
        }

        private ICommand edit;
        public ICommand Edit
        {
            get { return edit ?? 
                (edit = new CommandBase(
                    p => MessageBox.Show(string.Format("Editing now person with {0} name with age of {1}", Name, Age)))); }
        }
    }
}
