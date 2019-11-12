using EverNoteApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EverNoteLatest.ViewModel.Command
{
    class ExampleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ExampleWindowVM VM;

        public ExampleCommand(ExampleWindowVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            Users user = parameter as Users;
            if (user != null)
            {
                Console.WriteLine("name is"+user.FirstName);
                return true;
            }
            Console.WriteLine("user is null, returning false");
            return false;      
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("from execute");
            VM.ToString();
        }
    }
}
