using System.Windows.Controls;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class TeacherUploadView : UserControl
    {
        public TeacherUploadView()
        {
            InitializeComponent();

            DataContext = new TeacherUploadViewModel();
        }
    }
}
