using ProgressTracker.Model;
using ProgressTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProgressTracker.View
{
    /// <summary>
    /// Interaction logic for NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window
    {
        public NoteWindowViewModel nwvm;
        // this event comes from Note Window, as it does not have access
        // to the list of MainItems. Params: Note and the note's plain text.
        public event Action<Note, string>? MatchRequest;
        public Note ThisNote;

        public NoteWindow(Note note, NoteWindowViewModel nwvm)
        {
            InitializeComponent();
            ThisNote = note;
            this.nwvm = nwvm;
            this.Title = $"Note - {note.Date}";

            Utils.Rtf.LoadRichTextBox(NoteText, nwvm.content);
            this.Closing += CloseWindow;
        }



        private void CloseWindow(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {

            nwvm.content = Utils.Rtf.GetRtf(NoteText);
            //save contents to file
            nwvm.NoteSaveCommand.Execute(null);

            string plainText = Utils.Rtf.GetPlainText(NoteText);
            MatchRequest?.Invoke(ThisNote, plainText);
        }


    }
}




