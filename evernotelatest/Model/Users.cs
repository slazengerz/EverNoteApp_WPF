using SQLite;
using System.ComponentModel;

namespace EverNoteApp.Model
{
    class Users:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string id;
        private string email;
        private string firstname;
        private string lastname;
        private string password;

        [PrimaryKey, AutoIncrement]
        public string Id
        {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        public string FirstName
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string LastName
        {
            get { return lastname; }
            set
            {
                lastname = value;
                OnPropertyChanged("LastName");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                System.Console.WriteLine("prroperty changed name is"+propertyname);
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
