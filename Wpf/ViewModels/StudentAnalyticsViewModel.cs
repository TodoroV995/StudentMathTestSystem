using Application.DTOs.Results;
using StudentMathTestSystem.Wpf.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Services.Api;

namespace Wpf.ViewModels
{
    public class StudentAnalyticsViewModel : BaseViewModel
    {
        private readonly StudentApiService _studentApiService;

        private StudentListItemViewModel _selectedStudent;
        private TestResultDto _selectedResult;
        private string _statusMessage;
        private bool _isBusy;

        public ObservableCollection<StudentListItemViewModel> Students { get; }
            = new ObservableCollection<StudentListItemViewModel>();

        public ObservableCollection<TestResultDto> Results { get; }
            = new ObservableCollection<TestResultDto>();

        public StudentListItemViewModel SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();

                Results.Clear();
                SelectedResult = null;
            }
        }

        public TestResultDto SelectedResult
        {
            get => _selectedResult;
            set
            {
                _selectedResult = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadResultsCommand { get; }

        public StudentAnalyticsViewModel()
        {
            _studentApiService = new StudentApiService();

            LoadResultsCommand = new RelayCommand(async _ => await LoadResultsAsync());

            _ = LoadStudentsAsync();
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                var students = await _studentApiService.GetStudentsAsync();

                Students.Clear();

                foreach (var student in students)
                {
                    Students.Add(new StudentListItemViewModel
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Grade = student.Grade
                    });
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to load students: " + ex.Message;
            }
        }

        private async Task LoadResultsAsync()
        {
            if (SelectedStudent == null)
            {
                StatusMessage = "Please select a student.";
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = "Loading student results...";
                Results.Clear();

                var results = await _studentApiService.GetStudentResultsAsync(SelectedStudent.Id);

                foreach (var result in results)
                {
                    Results.Add(result);
                }

                StatusMessage = Results.Count == 0
                    ? "No results found for this student."
                    : "Results loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to load results: " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class StudentListItemViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Grade { get; set; }

        public string FullName => $"{FirstName} {LastName} - Grade {Grade}";
    }
}