using ProgressTracker.Model;
using ProgressTracker.ViewModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProgressTracker.ViewModel
{
    public class NoteWindowViewModel
    {

        private NoteManager noteManager;
        private string date;
        public string content { get; set; }

        public NoteWindowViewModel(Note note)
        {
            noteManager = new NoteManager();

            this.date = note.Date;
            noteManager = new NoteManager();
            content = noteManager.GetContentByDate(note.Date);
            NoteSaveCommand = new MainWindowViewModel.RelayCommand(_ => SaveNote());
        }


        public ICommand NoteSaveCommand;
        public void SaveNote()
        {
            noteManager.SaveContentByDate(date, content);
        }
    }

}
