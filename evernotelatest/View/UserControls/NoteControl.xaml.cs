using EverNoteApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EverNoteLatest.View.UserControls
{
    /// <summary>
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();
        }
        public Notes NotesDP
        {
            get { return (Notes)GetValue(NotesDPProperty); }
            set { SetValue(NotesDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotesDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotesDPProperty =
            DependencyProperty.Register("NotesDP", typeof(Notes), typeof(NoteControl), new PropertyMetadata(null,SetValue));

        private static void SetValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notesLocal = d as NoteControl;
            if (notesLocal != null)
            {
                Notes newNotes = e.NewValue as Notes;
                notesLocal.noteTitleText.Text = newNotes.Title;
                notesLocal.noteContentText.Text = newNotes.Content;
                notesLocal.noteDateCreatedText.Text = newNotes.CreatedTime.ToShortDateString();
                notesLocal.noteDateUpdatedText.Text= newNotes.UpdateTime.ToShortDateString();
            }
        }
    }
}
