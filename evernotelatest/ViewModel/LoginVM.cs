using EverNoteApp.Model;
using EverNoteApp.ViewModel.Command;
using EverNoteLatest;
using SQLite;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EverNoteApp.ViewModel
{
    class LoginVM:INotifyPropertyChanged
    {
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        //create an eventhandler to trigger if user is logged in
        public static event EventHandler HasLoggedIn;
        public event PropertyChangedEventHandler PropertyChanged;
        public Users user { get; set; }
        public LoginVM()
        {
            user = new Users();
            LoginCommand = new LoginCommand(this);
            RegisterCommand = new RegisterCommand(this);
        }

        public string UserName
        {
            get { return user.FirstName; }
            set {
                user.FirstName = value;
                OnPropertyChanged("UserName");
            }
        }
        public string Password
        {
            get { return user.Password; }
            set
            {
                user.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Email
        {
            get { return user.Email; }
            set
            {
                user.Email = value;
                OnPropertyChanged("Email");
            }
        }
        public void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
                Console.WriteLine("calling canexecutechanged " + propertyname);
                LoginCommand.reValidateButtonState();
                RegisterCommand.reValidateButtonState();
            }
        }
        public async Task<bool> checkUserExists(Users user)
        {
            //using(SQLiteConnection con=new SQLiteConnection(DataBaseHelper.dbfile))
            //{
            //    //firstname is used as username
            //    con.CreateTable<Users>();
            //    var useExists = con.Table<Users>().
            //                    Where(u => u.FirstName == user.FirstName && u.Password.Equals(user.Password))
            //                    .FirstOrDefault();
            //    if (useExists!= null)
            //    {
            //        Console.WriteLine("User found");
            //        App.UserId = useExists.Id;
            //        HasLoggedIn(this, new EventArgs());
            //        return true;
            //    }
            //}
            try
            {
                var useExists = (await App.MobileServiceClient.GetTable<Users>()
                                .Where(u => u.FirstName==user.FirstName).ToListAsync()).FirstOrDefault();
                if (useExists != null && useExists.Password.Equals(user.Password))
                {
                    Console.WriteLine("User found");
                    App.UserId = useExists.Id;
                    HasLoggedIn(this, new EventArgs());
                    return true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in logging in"+e.Message);
            }
            return false;

        }
        public async Task<bool> registerUser(Users user)
        {
            //using (SQLiteConnection con = new SQLiteConnection(DataBaseHelper.dbfile))
            //{
            //    con.CreateTable<Users>();
            //    var useExists = DataBaseHelper.Insert<Users>(user);
            //    if (useExists>0)
            //    {
            //        App.UserId = user.UserId.ToString();
            //        HasLoggedIn(this, EventArgs.Empty);
            //        return true;
            //    }
            //}
            try {
                await App.MobileServiceClient.GetTable<Users>().InsertAsync(user);
                App.UserId = user.Id;
                HasLoggedIn(this, EventArgs.Empty);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Excetion in Registration"+e.Message);
                return false;
            }
   
        }
    }
}
