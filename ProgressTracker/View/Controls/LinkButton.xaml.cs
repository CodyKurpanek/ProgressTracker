using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgressTracker.View.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LinkButton : UserControl
    {
        public static readonly DependencyProperty text =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(LinkButton),
                new PropertyMetadata(""));

        public string Text
        {
            get => (string)GetValue(text);
            set => SetValue(text, value);
        }


        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(LinkButton),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        public LinkButton()
        {
            InitializeComponent();
        }
    }
}
