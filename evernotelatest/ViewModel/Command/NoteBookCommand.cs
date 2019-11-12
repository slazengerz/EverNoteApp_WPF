using EverNoteApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EverNoteApp.ViewModel.Command
{
    class NoteBookCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainViewModel Vm;

        public NoteBookCommand(MainViewModel vm)
        {
            Vm = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Vm.CreateNoteBook();
            Vm.getNoteBooks();
        }
    }
}
