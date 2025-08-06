using ProgressTracker.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProgressTracker.Model
{
    public class Note
    {
        public string Title { get; set; }
        public string Date { get; set; }


        public Note(string title, string date)
        {
            this.Title = title;
            this.Date = date;
        }

        public override bool Equals(object? obj)
        {
            return obj is Note note && Date == note.Date;
        }


        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

    }


    // This class manages utility functions for loading and saving Notes.
    // It also handles 
    public class NoteManager
    {

        // Keeps track of what notes are currently showing in the listbox
        private HashSet<Note> openNotes;
        private readonly string noteBaseDirectory = "C:\\Users\\Public\\Documents\\ProgressTracker\\Notes\\";
        

        public NoteManager() 
        {             
            if (!Directory.Exists(noteBaseDirectory))
            {
                Directory.CreateDirectory(noteBaseDirectory);
            }
            openNotes = new HashSet<Note>();
        }




        // Based on dateString, get the note file path. This should probably put something into the listbox in the UI.
        public string GetContentByDate(string date)
        {

            string filepath = DateToFilepath(date);
            if (!File.Exists(filepath))
            {
                CreateNote(date);
            }

            return File.ReadAllText(filepath);
        }
        public void SaveContentByDate(string date, string content)
        {
            string filepath = DateToFilepath(date);
            File.WriteAllText(filepath, content);
        }

        private void CreateNote(string date)
        {
            string rtfContent = @"{\rtf1\ansi\deff0" +
                @"{\fonttbl{\f0\fnil\fcharset0 Calibri;}}" +
                @"{\colortbl;\red0\green0\blue0;}" +
                @"\viewkind4\uc1\pard\cf1\f0\fs22 new note\par}";


            string filepath = DateToFilepath(date);
            File.WriteAllText(filepath, rtfContent);
        }



        public bool CanAddNote(Note note)
        {
            
            if (openNotes.Contains(note))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        // Updates the note inplace if it exists, otherwise return the note to be added.
        public Note UpdateNote(string date, string text)
        {
            Note note = new Note("Mentions: " + text, date);
            var existingNote = openNotes.FirstOrDefault(n => n.Equals(note));
            if (existingNote != null)
            {
                if (existingNote.Title == "")
                {
                    existingNote.Title = "Mentions: " + text;
                }
                else if(!existingNote.Title.Contains(text))
                {
                    existingNote.Title = existingNote.Title + ", " + text;
                }
                return null;
            }
            else
            {
                return note;
            }
        }


        // Update the Model as notes are added to the viewmodel
        public void OnAddNote(Note note)
        {
            openNotes.Add(note);

        }
        // Update the Model as notes are added to the viewmodel
        public void OnRemoveNote(Note note)
        {
            openNotes.Remove(note);
        }





        private string FilepathToDate(string filepath)
        {
            return Path.GetFileName(filepath);
        }
        private string DateToFilepath(string dateString)
        {
            return (Path.Combine(noteBaseDirectory, $"{dateString}.rtf"));
        }

    }





}
