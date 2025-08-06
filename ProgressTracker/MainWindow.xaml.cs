using ProgressTracker.ViewModel;
using ProgressTracker.Model;
using ProgressTracker.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgressTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        private readonly List<NoteWindowViewModel> ChildNoteVMs = new();
        
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            this.DataContext = vm;
            vm.RequestOpenNote += (note) => OpenNote(note);
            
        }

        private void NoteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedNotes = NotesList.SelectedItems.Cast<NoteViewModel>().ToList();
            vm.SelectedNotes = selectedNotes;
        }

        public void OpenNote(Note note)
        {
            NoteWindowViewModel nwvm = new NoteWindowViewModel(note);
            ChildNoteVMs.Add(nwvm);

            NoteWindow noteWindow = new NoteWindow(note, nwvm);

            // When wanting to 
            noteWindow.MatchRequest += (matchNote, matchText) =>
            {
                vm.FindMatches(matchNote, matchText);
            };
            noteWindow.Closing += (sender, e) =>
            {
                ChildNoteVMs.Remove(nwvm);
            };


            noteWindow.Show();

        }
    }
}