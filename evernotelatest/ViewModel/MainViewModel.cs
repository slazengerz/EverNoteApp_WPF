using EverNoteApp.Model;
using EverNoteApp.ViewModel.Command;
using System.Collections.ObjectModel;
using SQLite;
using System;
using EverNoteLatest;
using System.ComponentModel;
using EverNoteLatest.ViewModel.Command;

namespace EverNoteApp.ViewModel
{
    class MainViewModel:INotifyPropertyChanged
    {
        public ExitCommand ExitCommand { get; set; }
        public NoteBookCommand NoteBookCommand { get; set; }
        public NoteCommand NoteCommand { get; set; }

        private NoteBook selectednotebook;
        public ObservableCollection<NoteBook> NoteBookCollection { get; set; }
        public ObservableCollection<Notes> NotesCollection { get; set; }

        private string statusbarcontent;

        private bool isediting;
        private bool iseditingnote;
        public StartEditingCommand StartEditingCommand { get; set; }
        public StartEditingNoteCommand StartEditingNoteCommand { get; set; }
        public HasEditedCommand HasEditedCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public DeleteNoteCommand DeleteNoteCommand { get; set; }
        private Notes selectednote { get; set; }
        public static event EventHandler SelectedNoteEvent;
        public DeleteNoteBookCommand DeleteNoteBookCommand { get; set; }
        public HasEditedNoteCommand HasEditedNoteCommand { get; set; }

        public MainViewModel()
        {
            ExitCommand = new ExitCommand();
            NoteBookCommand = new NoteBookCommand(this);
            NoteCommand = new NoteCommand(this);

            NoteBookCollection = new ObservableCollection<NoteBook>();
            NotesCollection = new ObservableCollection<Notes>();
            //LoginVM loginvm = new LoginVM();
            LoginVM.HasLoggedIn += Loginvm_HasLoggedIn;
            IsEditing = false;
            IsEditingNote = false;
            StartEditingCommand = new StartEditingCommand(this);
            HasEditedCommand = new HasEditedCommand(this);
            DeleteNoteCommand = new DeleteNoteCommand(this);
            DeleteNoteBookCommand = new DeleteNoteBookCommand(this);
            StartEditingNoteCommand = new StartEditingNoteCommand(this);
            HasEditedNoteCommand = new HasEditedNoteCommand(this);

            //getNotes();
        }

        public bool IsEditing
        {
            get { return isediting; }
            set {
                isediting = value;
                OnPropertyChanged("IsEditing");
            }
        }

        public bool IsEditingNote
        {
            get { return iseditingnote; }
            set
            {
                iseditingnote = value;
                OnPropertyChanged("IsEditingNote");
            }
        }

        private void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
            
        }

        public String StatusBarContent
        {
            get { return statusbarcontent; }
            set
            {
                statusbarcontent = value;
            }
        }

        private string richtextboxcontent;


        public String RichTextBoxContent
        {
            get { return richtextboxcontent; }
            set
            {
                richtextboxcontent = value;
                StatusBarContent = string.Format($"Word Count {RichTextBoxContent.Length}");
            }
        }
        public NoteBook SelectedNoteBook {
            get { return selectednotebook; }
            set
            {
                selectednotebook = value;
                //call getnotes here
                if (SelectedNoteBook != null)
                {
                    Console.WriteLine("from prop val is" + value.Name);
                    getNotes(SelectedNoteBook.Id);
                }
            }
        }

        public Notes SelectedNote
        {
            get { return selectednote; }
            set
            {
                selectednote = value;
                SelectedNoteEvent(this,EventArgs.Empty);
            }
        }

        private void Loginvm_HasLoggedIn(object sender, EventArgs e)
        {
            Console.WriteLine("from Loginvm_HasLoggedIn");
            getNoteBooks();
        }

        //this will be called from NoteBookCommand 
        //on click of new notebook button
        public async void CreateNoteBook()
        {
            try
            {
                NoteBook newNoteBook = new NoteBook()
                {
                    Name = "Fresh NoteBook",
                    UserId = App.UserId
                };
                await App.MobileServiceClient.GetTable<NoteBook>().InsertAsync(newNoteBook);
                Console.WriteLine("Record inserted in NoteBook Table");
                getNoteBooks();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in creating Notebook"+e.Message);
            }
        }

        public async void getNoteBooks()
        {
            //NoteBook noteBookTable = new NoteBook();
            //using(SQLiteConnection con=new SQLiteConnection(DataBaseHelper.dbfile))
            //{
            //    if (DataBaseHelper.tableExists<NoteBook>())
            //    {
            //        int.TryParse(App.UserId,out int result);
            //        Console.WriteLine("result is"+result);
            //        var notebooks = con.Table<NoteBook>().
            //                        Where(n => n.UserId.Equals(result)).ToList();
            //        NoteBookCollection.Clear();
            //        foreach (var notebook in notebooks)
            //        {
            //            NoteBookCollection.Add(notebook);
            //        }
            //        Console.WriteLine($"Total Notebooks count is {NoteBookCollection.Count}");
            //    }
            //}
            try
            {
                var notebooks = await App.MobileServiceClient.GetTable<NoteBook>().
                                        Where(n => n.UserId == App.UserId).ToListAsync();
                NoteBookCollection.Clear();
                foreach (var notebook in notebooks)
                {
                    NoteBookCollection.Add(notebook);
                }
                Console.WriteLine($"Total Notebooks count is {NoteBookCollection.Count}");
            }catch(Exception e)
            {
                Console.WriteLine("Exception in getting notebooks"+e.Message);
            }
        }

        public async void CreateNotes(string notebookid)
        {
            //using(SQLiteConnection con=new SQLiteConnection(DataBaseHelper.dbfile))
            //{
            //    Notes notes = new Notes()
            //    {
            //        NoteBookId = notebookid,
            //        Title = "Fresh Note",
            //        UpdateTime = DateTime.Now,
            //        CreatedTime = DateTime.Now,

            //    };
            //    DataBaseHelper.Insert(notes);
            //}
            try
            {
                Notes notes = new Notes()
                {
                    NoteBookId = notebookid,
                    Title = "Fresh Note",
                    UpdateTime = DateTime.Now,
                    CreatedTime = DateTime.Now,

                };
                await App.MobileServiceClient.GetTable<Notes>().InsertAsync(notes);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in creating notes"+e.Message);
            }
        }

        public async void getNotes(string noteBookId)
        {
            //if (DataBaseHelper.tableExists<Notes>())
            //{
            //    using (SQLiteConnection con = new SQLiteConnection(DataBaseHelper.dbfile))
            //    {
            //        var notes = con.Table<Notes>().
            //                        Where(n => n.NoteBookId.Equals(notebookid)).
            //                        ToList();

            //        NotesCollection.Clear();
            //        foreach (var note in notes)
            //        {
            //            NotesCollection.Add(note);
            //        }
            //    }
            //}
            try
            {
                var notes = await App.MobileServiceClient.GetTable<Notes>().
                                    Where(n => n.NoteBookId== noteBookId).ToListAsync();
                NotesCollection.Clear();
                foreach (var note in notes)
                {
                    NotesCollection.Add(note);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exceptionin getting notes"+e.Message);
            }
        }

        public void StartedEditing()
        {
            IsEditing = true;
        }

        public void startEditingNote()
        {
            IsEditingNote = true;
        }

        public async void HasRenamed(NoteBook notebook)
        {
            //using(SQLiteConnection connection=new SQLiteConnection(DataBaseHelper.dbfile))
            //{
            //    var updatedresult=DataBaseHelper.Update<NoteBook>(notebook);
            //    if (updatedresult)
            //    {
            //        IsEditing = false;
            //        getNoteBooks();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Error in updating notebook");
            //    }
            //}
            try
            {
                await App.MobileServiceClient.GetTable<NoteBook>().UpdateAsync(notebook);
                IsEditing = false;
                getNoteBooks();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error in updating Notebook"+e.Message);
            }
        }

        public async void hasEditedNote(Notes note)
        {
            try
            {
                await App.MobileServiceClient.GetTable<Notes>().UpdateAsync(note);
                IsEditingNote = false;
                Console.WriteLine("NoteBook Id form getnotes is"+ SelectedNoteBook.Id);
                getNotes(note.NoteBookId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error inEditing Notes" + e.Message);
            }
        }

        public async void deleteNote(Notes note)
        {
            //using(SQLiteConnection con=new SQLiteConnection(DataBaseHelper.dbfile))
            //{
            //    var result = DataBaseHelper.Delete<Notes>(note);
            //    if (!result)
            //    {
            //        Console.WriteLine("Error in deleting note "+note.Title);
            //    }
            //    if (SelectedNoteBook != null)
            //    {
            //        getNotes(SelectedNoteBook.Id);
            //    }
            //}
            try
            {
                var notebookId = note.NoteBookId;
                await App.MobileServiceClient.GetTable<Notes>().DeleteAsync(note);
                getNotes(notebookId);

            }catch(Exception e)
            {
                Console.WriteLine("Error in deleting note"+e.Message);
            }
        }

        public async void deleteNoteBook(NoteBook notebook)
        {
            try
            {
                if (notebook != null)
                {
                    await App.MobileServiceClient.GetTable<NoteBook>().DeleteAsync(notebook);
                    Console.WriteLine("Successfully Deleted the NoteBook");
                    getNoteBooks();
                }

            }catch(Exception e)
            {
                Console.WriteLine("Erro in deleting NoteBook"+e.Message);
            }
        }

        public async void updateNote(Notes note)
        {
            //update the selected note
            //this is required as we are updating filelocation of a particular note
            //file location contains the text typed corresponding to each note.
            //DataBaseHelper.Update(note);
            try
            {
                await App.MobileServiceClient.GetTable<Notes>().UpdateAsync(note);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error in Updating notes"+e.Message);
            }
        }
    }
}
