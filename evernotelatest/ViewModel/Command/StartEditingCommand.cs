using EverNoteApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EverNoteLatest.ViewModel.Command
{
    class StartEditingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private MainViewModel VM;

        public StartEditingCommand(MainViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.StartedEditing();
        }
    }
}
