using Avalonia.Controls;
using Pathfinder.ViewModels;

namespace Pathfinder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this); // Pass 'this' to the view model
        }
    }
}