using System.Windows.Controls;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class StudentAnalyticsView : UserControl
    {
        public StudentAnalyticsView()
        {
            InitializeComponent();
            DataContext = new StudentAnalyticsViewModel();
        }
    }
}