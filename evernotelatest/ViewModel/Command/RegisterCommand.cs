using EverNoteApp.Model;
using System;
using System.Windows.Input;

namespace EverNoteApp.ViewModel.Command
{
    class RegisterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public LoginVM VM;
        public RegisterCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            Console.WriteLine("Inside CanExecute");
            if(!string.IsNullOrEmpty(VM.Password) && !string.IsNullOrEmpty(VM.UserName) && !string.IsNullOrEmpty(VM.Email))
            {
                Console.WriteLine("Returning True from Register");
                return true;
            }
            return false;
        }
        public void reValidateButtonState()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public async void Execute(object parameter)
        {
            Users user = parameter as Users;
            await VM.registerUser(user);
        }
    }
}
