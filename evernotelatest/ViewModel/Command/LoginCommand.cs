using EverNoteApp.Model;
using System;
using System.Windows.Input;

namespace EverNoteApp.ViewModel.Command
{
    class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private LoginVM VM;
        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            //LoginVM user = parameter as LoginVM;
            //return true;
            if (VM != null)
            {
                Console.WriteLine("User is not null and it comes here");
                if (VM.UserName != null && VM.Password != null)
                {
                    Console.WriteLine($"First Name {VM.UserName}");
                    return true;
                }
                return false;
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
            LoginVM vm = parameter as LoginVM;
            Users user = new Users();
            user.FirstName = vm.UserName;
            user.Password = vm.Password;
            await VM.checkUserExists(user);
        }
    }
}