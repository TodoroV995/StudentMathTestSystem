using System.Windows.Controls;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class ResultsOverviewView : UserControl
    {
        public ResultsOverviewView()
        {
            InitializeComponent();
            DataContext = new ResultsOverviewViewModel();
        }
    }
}