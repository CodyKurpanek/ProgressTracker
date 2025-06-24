using ProgressTracker.Model;
using ProgressTracker.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static ProgressTracker.ViewModel.MainWindowViewModel;


namespace ProgressTracker.ViewModel
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
        public MainWindowViewModel()
        {
            mainItems = new ObservableCollection<MainItem>
            {
                new MainItem("Item 1", "Search 1"),
                new MainItem("Item 2", "Search 2"),
                new MainItem("Item 3", "Search 3")
            };

            noteManager = new NoteManager();

            notes = new ObservableCollection<Note>{
                new Note( "Note 1", "2023-10-01")
            };

            CalendarSelectCommand = new RelayCommand(_ => SelectDate());
            OpenNoteCommand = new RelayCommand(param =>
            { 
                if (param is Note note) 
                {
                    RequestOpenNote?.Invoke(note);
                }
            });
        }
        public event Action<Note>? RequestOpenNote;

        //==================================   Main Items
        private ObservableCollection<MainItem> mainItems;
		
		public ObservableCollection<MainItem> MainItems
		{
			get { return mainItems; }
			set
			{
				mainItems = value;
				OnPropertyChanged();
			}
		}

        // =================================     Notes
        private NoteManager noteManager;
        private ObservableCollection<Note> notes;
        public ICommand OpenNoteCommand { get; }
        public ObservableCollection<Note> Notes
		{
			get { return notes; }
            set 
			{ notes = value;
			OnPropertyChanged();
            }
        }
        public void OpenNote(Note param)
        {
            Note nn = new Note("new note", param.Date);
            notes.Add(nn);
            noteManager.GetContentByDate(param.Date);

            
            
            
        }






        //=====================================   Calendar
        private DateTime? calendarDate;

        public ICommand CalendarSelectCommand { get; }


        public DateTime? CalendarDate
        {
            get { return calendarDate; }
            set { calendarDate = value;
                OnPropertyChanged();
            }
        }

        public void SelectDate()
        {
            if (calendarDate == null)
            {
                return;
            }
            
            Note nn = new Note("new note", calendarDate.Value.ToString("yyyy-MM-dd"));
            notes.Add(nn);
        }

        //================================   Helper
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class RelayCommand : ICommand
        {
            private readonly Action<object?> _execute;
            private readonly Func<object?, bool>? _canExecute;

            public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
            public void Execute(object? parameter) => _execute(parameter);
            public event EventHandler? CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }

    public class Note
    {
        public string Title { get; set; }
        public string Date { get; set; }

        public Note(string title, string date)
        {
            this.Title = title;
            this.Date = date;
        }

    }
}




