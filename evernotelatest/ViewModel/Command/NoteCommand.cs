using EverNoteApp.Model;
using System;
using System.Windows.Input;

namespace EverNoteApp.ViewModel.Command
{
    class NoteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainViewModel Vm;
        public NoteCommand(MainViewModel vm)
        {
            Vm = vm;
        }
        public bool CanExecute(object parameter)
        {
            NoteBook selectedNoteBook = Vm.SelectedNoteBook;
            Console.WriteLine("selectednotebook val"+selectedNoteBook);
            return (selectedNoteBook != null ? true : false);
        }

        public void Execute(object parameter)
        {
            NoteBook selectedNoteBook = Vm.SelectedNoteBook;
            Vm.CreateNotes(selectedNoteBook.Id);
            Vm.getNotes(selectedNoteBook.Id);
        }
    }
}
