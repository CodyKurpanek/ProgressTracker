using ProgressTracker.Model;
using ProgressTracker.View;
using ProgressTracker.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Quic;
using Microsoft.Win32;


namespace ProgressTracker.ViewModel
{
	public class MainWindowViewModel : MyNotifyPropertyChanged
	{
        public ICommand OpenNoteWindowCommand { get; }
        public ICommand RemoveNoteCommand { get; }
        public ICommand SelectNoteCommand { get; }
        public ICommand AddMatchedNotesCommand { get; }

        public MainWindowViewModel()
        {
            mainItems = new ObservableCollection<MainItemViewModel>
            {
                new MainItemViewModel(new MainItem("Keep track of what I have debugged", "Debug")),
                new MainItemViewModel(new MainItem("Keep track of resolved github issues", "Resolved Issue")),
                new MainItemViewModel(new MainItem("Keep track of code reviews", "Review"))
            };

            noteManager = new NoteManager();

            notes = new ObservableCollection<NoteViewModel> { };

            SelectNoteCommand = new RelayCommand(_ => SelectDate());

            OpenNoteWindowCommand = new RelayCommand(param =>
            {
                if (param is NoteViewModel noteViewModel)
                {
                    RequestOpenNote?.Invoke(noteViewModel.Note);
                }
            });

            RemoveNoteCommand = new RelayCommand(_ => RemoveDate());

            AddMatchedNotesCommand = new RelayCommand(param =>
            {
                if (param is MainItemViewModel mainItemViewModel)
                {
                    AddMatchedNotes(mainItemViewModel);
                }
            });
        }
        
        public event Action<Note>? RequestOpenNote;
        

        //==================================   Main Items
        private ObservableCollection<MainItemViewModel> mainItems;
		
		public ObservableCollection<MainItemViewModel> MainItems
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
        



        private List<NoteViewModel> selectedNotes;
        public List<NoteViewModel> SelectedNotes
        {
            get => selectedNotes;
            set
            {
                selectedNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NoteViewModel> notes;
        public ObservableCollection<NoteViewModel> Notes
		{
			get { return notes; }
            set 
			{ notes = value;
			OnPropertyChanged();
            }
        }
        //public void OpenNote(Note param)
        //{
        //    NoteViewModel nn = new NoteViewModel("new note", param.Date);
        //    notes.Add(nn);
        //    noteManager.GetContentByDate(param.Date);

        //}

        public void SelectDate()
        {

            if (calendarDate == null)
            {
                
                return;
            }
            

            Note nn = new Note("", calendarDate.Value.ToString("yyyy-MM-dd"));
            if (noteManager.CanAddNote(nn))
            {
                notes.Add(new NoteViewModel(nn));
                noteManager.OnAddNote(nn);
            }
        }
        public void AddMatchedNotes(MainItemViewModel mainItemViewModel)
        {
            foreach (string date in mainItemViewModel.MainItem.Matches)
            {
                Note n = noteManager.UpdateNote(date, mainItemViewModel.Search);
                if(n != null)
                {
                    notes.Add(new NoteViewModel(n));
                    noteManager.OnAddNote(n);
                }
            }
        }

        public void RemoveDate()
        {
            if (selectedNotes != null)
            {
                foreach (NoteViewModel noteViewModel in selectedNotes)
                {
                    notes.Remove(noteViewModel);
                    noteManager.OnRemoveNote(noteViewModel.Note);
                }
            }
        }






        //=====================================   Calendar
        private DateTime? calendarDate;


        public DateTime? CalendarDate
        {
            get { return calendarDate; }
            set { calendarDate = value;
                OnPropertyChanged();
            }
        }


        //=================================  Matching
        public void FindMatches(Note note, string searchText)
        {

            
            foreach (MainItemViewModel itemViewModel in MainItems)
            {

                //TODO: Refactor this search logic to have it's own model class for improvements later.
                if (string.IsNullOrEmpty(searchText) || string.IsNullOrEmpty(itemViewModel.Search))
                {
                    continue;
                }
                if(searchText.IndexOf(itemViewModel.Search, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    //TODO Fix if I remove something from a note.
                    //TODO Save the counts somewhere in a file so no need to re-match when opening.

                    if (!itemViewModel.Matches.Contains(note.Date))
                    {
                        itemViewModel.Matches.Add(note.Date);
                        itemViewModel.Count++;
                    }

                    
                }
                
            }
        }
        
    }

    public class MainItemViewModel : MyNotifyPropertyChanged
    {
        private readonly MainItem _mainItem;
        public string Text
        {
            get { return _mainItem.Text; }
            set { _mainItem.Text = value;  OnPropertyChanged(); }
        }
        public string Search
        {
            get { return _mainItem.Search; }
            set { _mainItem.Search = value; OnPropertyChanged(); }
        }
        public int Count
        {
            get { return _mainItem.Count; }
            set { _mainItem.Count = value; OnPropertyChanged(); }
        }
        public MainItem MainItem
        {
            get { return _mainItem;  }
        }
        public HashSet<string> Matches 
        { 
            get { return _mainItem.Matches; }
            set { _mainItem.Matches = value; }
        }
        public MainItemViewModel(MainItem item)
        {
            _mainItem = item;
        }
    }

    public class NoteViewModel : MyNotifyPropertyChanged
    {
        private readonly Note _note;
        public string Title
        {
            get { return _note.Title; }
            set { _note.Title = value; OnPropertyChanged(); }
        }
        public string Date
        {
            get { return _note.Date; }
            set { _note.Date = value; OnPropertyChanged(); }
        }
        public Note Note { get { return _note; } }
        public NoteViewModel(Note note)
        {
            this._note = note;
        }
        public override bool Equals(object? obj)
        {
            return obj is NoteViewModel noteViewModel && _note == noteViewModel.Note;
        }
    }
}




