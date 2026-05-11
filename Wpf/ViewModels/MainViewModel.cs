using StudentMathTestSystem.Wpf.Commands;
using System.Windows.Input;

namespace Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowTeacherUploadCommand { get; }
        public ICommand ShowStudentAnalyticsCommand { get; }

        public MainViewModel()
        {
            ShowTeacherUploadCommand = new RelayCommand(_ => ShowTeacherUpload());
            ShowStudentAnalyticsCommand = new RelayCommand(_ => ShowStudentAnalytics());

            ShowTeacherUpload();
        }

        private void ShowTeacherUpload()
        {
            CurrentViewModel = new TeacherUploadViewModel();
        }

        private void ShowStudentAnalytics()
        {
            CurrentViewModel = new StudentAnalyticsViewModel();
        }
    }
}