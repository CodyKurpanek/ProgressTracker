using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ProgressTracker.Model
{
    // This class holds and manages the notes within the list of notes
    public class NoteManager
    {
        private readonly string noteBaseDirectory = "C:\\Users\\Public\\Documents\\ProgressTracker\\Notes\\";
        

        public NoteManager() 
        { 
            if (!Directory.Exists(noteBaseDirectory))
            {
                Directory.CreateDirectory(noteBaseDirectory);
            }
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
