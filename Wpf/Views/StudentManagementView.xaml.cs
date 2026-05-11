using System.Windows.Controls;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class StudentManagementView : UserControl
    {
        public StudentManagementView()
        {
            InitializeComponent();
            DataContext = new StudentManagementViewModel();
        }
    }
}