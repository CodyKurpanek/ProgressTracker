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

        public NoteWindow(Note note)
        {
            InitializeComponent();
            nwvm = new NoteWindowViewModel(note);
            this.DataContext = nwvm;

            var textRange = new TextRange(NoteText.Document.ContentStart, NoteText.Document.ContentEnd);

            // Load the nwvm.content into the note.
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(nwvm.content)))
            {
                var range = new TextRange(NoteText.Document.ContentStart, NoteText.Document.ContentEnd);
                range.Load(stream, DataFormats.Rtf);
            }
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
            var vm = (NoteWindowViewModel)DataContext;

            var range = new TextRange(NoteText.Document.ContentStart, NoteText.Document.ContentEnd);
            using var stream = new MemoryStream();
            range.Save(stream, DataFormats.Rtf);
            vm.content = Encoding.UTF8.GetString(stream.ToArray());

            vm.NoteSaveCommand.Execute(null); // Now saves the content
        }


    }
}




