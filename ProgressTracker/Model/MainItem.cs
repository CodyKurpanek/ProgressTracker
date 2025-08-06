using ProgressTracker.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTracker.Model
{
    public class MainItem
    {
        public string Text { get; set; }
        public string Search { get; set; }
        public int Count { get; set; }
        public HashSet<string> Matches { get; set; }

        public MainItem(string text, string search)
        {
            this.Text = text;
            this.Search = search;
            this.Count = 0;
            this.Matches = new HashSet<string>();
        }
    }


    public class MainItemManager
    {

        private readonly string mainItemBaseDirectory = "C:\\Users\\Public\\Documents\\ProgressTracker\\Notes\\";

        public MainItemManager() 
        {
            if (!Directory.Exists(mainItemBaseDirectory))
            {
                Directory.CreateDirectory(mainItemBaseDirectory);
            }

        }
    }
    

}
