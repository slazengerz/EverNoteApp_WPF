using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverNoteApp.Model
{
    public class Notes:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string id;
        private string noteBookId;
        private string title;
        private string content;
        private DateTime createdtime;
        private DateTime updatedtime;
        private string filelocation;

        [PrimaryKey, AutoIncrement]
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string NoteBookId
        {
            get { return noteBookId; }
            set
            {
                noteBookId = value;
                OnPropertyChanged("NoteBookId");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }
        public DateTime CreatedTime
        {
            get { return createdtime; }
            set
            {
                createdtime = value;
                OnPropertyChanged("CreatedTime");
            }
        }

        public DateTime UpdateTime
        {
            get { return updatedtime; }
            set
            {
                updatedtime = value;
                OnPropertyChanged("UpdateTime");
            }
        }

        public string FileLocation
        {
            get { return filelocation; }
            set {
                filelocation = value;
                OnPropertyChanged("FileLocation");
            }
        }

        private void OnPropertyChanged(string propertyname)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
