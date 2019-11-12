using EverNoteApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace EverNoteLatest.View.UserControls
{
    /// <summary>
    /// Interaction logic for NoteBookUC.xaml
    /// </summary>
    public partial class NoteBookUC : UserControl
    {
        public NoteBook NoteBookDP
        {
            get {
                System.Console.WriteLine("in get of UC");
                return (NoteBook)GetValue(NoteBookDPProperty);
            }
            set { SetValue(NoteBookDPProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NoteBookDP.  
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteBookDPProperty =
            DependencyProperty.Register("NoteBookDP", typeof(NoteBook), typeof(NoteBookUC), new PropertyMetadata(null,SetValue));


        private static void SetValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NoteBookUC newNoteBook = d as NoteBookUC; 
            if (newNoteBook != null)
            {
                System.Console.WriteLine("in set of uc" + (e.NewValue as NoteBook).Name);
                newNoteBook.noteBookNameTxtBlk.Text = (e.NewValue as NoteBook).Name;
            }
        }
        public NoteBookUC()
        {
            InitializeComponent();
        }
    }
}
