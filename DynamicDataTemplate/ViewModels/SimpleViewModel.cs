using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DataDrivenUI;

namespace DynamicDataTemplate.ViewModels
{
    public class SimpleViewModel: ViewModelBase
    {
        public SimpleViewModel()
        {
            for (var i = 20; i < 40; i++)
            {
                People.Add(new PersonViewModel()
                               {
                                   Age = i, 
                                   Name = "Mark"
                               });
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                NotifyPropertyChanged("Text");
            }
        }

        public int something;
        public int Something
        {
            get { return something; }
            set 
            { 
                something = value;
                NotifyPropertyChanged("Something");
            }
        }

        public ICommand DoSmth
        {
            get
            {
                return CommandBase.Empty;
            }
        }

        private ObservableCollection<PersonViewModel> people; 
        public ObservableCollection<PersonViewModel> People
        {
            get { return people ?? (people = new ObservableCollection<PersonViewModel>()); }
            set
            {
                NotifyPropertyChanged("People");
            }
        }
    }
}
