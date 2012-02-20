using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DataDrivenUI.Tests
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            people = new ObservableCollection<PersonViewModel>();

            for (int i = 0; i < 20; i++)
            {
                people.Add(new PersonViewModel()
                               {
                                   Name = "Test", /*BirthDate = new DateTime(1988, 5, 20),*/ Characteristics = "Good man"
                               });
            }
        }

        private ObservableCollection<PersonViewModel> people;
        public ObservableCollection<PersonViewModel> People
        {
            get { return people; }
        }
    }
}
