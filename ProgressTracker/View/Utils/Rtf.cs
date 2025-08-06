using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProgressTracker.View.Utils
{
    public static class Rtf
    {
        public static string GetPlainText(RichTextBox rtb)
        {
            var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return range.Text;
        }
        public static string GetRtf(RichTextBox rtb)
        {
            using (var stream = new MemoryStream())
            {
                var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range.Save(stream, DataFormats.Rtf);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public static void LoadRichTextBox(RichTextBox rtb, string rtf)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rtf)))
            {
                var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range.Load(stream, DataFormats.Rtf);
            }
        }
    }
}
