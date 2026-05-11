using StudentMathTestSystem.Wpf.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Application.DTOs.Students;
using Wpf.Services.Api;
using System.IO;

namespace Wpf.ViewModels
{
    public class StudentManagementViewModel : INotifyPropertyChanged
    {
        private string _selectedXmlFilePath;
        private string _firstName;
        private string _lastName;
        private string _grade;
        private StudentViewModel _selectedStudent;
        private readonly StudentApiService _studentApiService;

        public ObservableCollection<StudentViewModel> Students { get; set; } = new ObservableCollection<StudentViewModel>();
        public bool IsFirstNameValid => !string.IsNullOrWhiteSpace(FirstName);

        public bool IsLastNameValid => !string.IsNullOrWhiteSpace(LastName);

        public bool IsGradeValid => !string.IsNullOrWhiteSpace(Grade) && int.TryParse(Grade, out _);

        public string SelectedXmlFilePath
        {
            get => _selectedXmlFilePath;
            set
            {
                _selectedXmlFilePath = value;
                OnPropertyChanged();
                ImportStudentsCommand.RaiseCanExecuteChanged();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFirstNameValid));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLastNameValid));
            }
        }

        public string Grade
        {
            get => _grade;
            set
            {
                _grade = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsGradeValid));
            }
        }

        public StudentViewModel SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();

                if (_selectedStudent != null)
                {
                    FirstName = _selectedStudent.FirstName;
                    LastName = _selectedStudent.LastName;
                    Grade = _selectedStudent.Grade;
                }

                AddStudentCommand.RaiseCanExecuteChanged();
                UpdateStudentCommand.RaiseCanExecuteChanged();
                DeleteStudentCommand.RaiseCanExecuteChanged();
            }
        }

        private bool ValidateStudentForm()
        {
            if (!IsFirstNameValid ||
                !IsLastNameValid ||
                !IsGradeValid)
            {
                MessageBox.Show(
                    "Please fill all required fields correctly.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        public RelayCommand BrowseXmlCommand { get; }
        public RelayCommand ImportStudentsCommand { get; }
        public RelayCommand AddStudentCommand { get; }
        public RelayCommand UpdateStudentCommand { get; }
        public RelayCommand DeleteStudentCommand { get; }
        public RelayCommand ClearCommand { get; }

        public StudentManagementViewModel()
        {
            _studentApiService = new StudentApiService();

            BrowseXmlCommand = new RelayCommand(BrowseXml);
            ImportStudentsCommand = new RelayCommand(ImportStudents, CanImportStudents);
            AddStudentCommand = new RelayCommand(AddStudent, CanAddStudent);
            UpdateStudentCommand = new RelayCommand(UpdateStudent, CanEditSelectedStudent);
            DeleteStudentCommand = new RelayCommand(DeleteStudent, CanEditSelectedStudent);
            ClearCommand = new RelayCommand(ClearForm);

            LoadStudents();
        }

        private async void LoadStudents()
        {
            Students.Clear();

            var students = await _studentApiService.GetStudentsAsync();

            foreach (var student in students)
            {
                Students.Add(new StudentViewModel
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Grade = student.Grade.ToString()
                });
            }
        }

        private void BrowseXml(object parameter)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                Title = "Select Students XML File"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedXmlFilePath = dialog.FileName;
            }
        }

        private async void ImportStudents(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SelectedXmlFilePath) ||
                !File.Exists(SelectedXmlFilePath))
            {
                MessageBox.Show(
                    "Please select a valid XML file.",
                    "Import Students",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var result = await _studentApiService.ImportStudentsFromXmlAsync(SelectedXmlFilePath);

            var message =
                $"Import completed.{Environment.NewLine}{Environment.NewLine}" +
                $"Imported: {result.ImportedCount}{Environment.NewLine}" +
                $"Skipped: {result.SkippedCount}";

            if (result.Errors != null && result.Errors.Any())
            {
                message += Environment.NewLine + Environment.NewLine + "Errors:" + Environment.NewLine;
                message += string.Join(Environment.NewLine, result.Errors.Select(e => "- " + e));
            }

            MessageBox.Show(
                message,
                "Import Students",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LoadStudents();

            SelectedXmlFilePath = string.Empty;
        }

        private async void AddStudent(object parameter)
        {
            if (!ValidateStudentForm())
                return;

            var dto = new CreateStudentDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Grade = int.Parse(Grade)
            };

            var createdStudent = await _studentApiService.CreateStudentAsync(dto);

            Students.Add(new StudentViewModel
            {
                Id = createdStudent.Id,
                FirstName = createdStudent.FirstName,
                LastName = createdStudent.LastName,
                Grade = createdStudent.Grade.ToString()
            });

            ClearForm(null);
        }

        private async void UpdateStudent(object parameter)
        {
            if (SelectedStudent == null)
                return;

            if (!ValidateStudentForm())
                return;

            var dto = new UpdateStudentDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Grade = int.Parse(Grade)
            };

            await _studentApiService.UpdateStudentAsync(SelectedStudent.Id, dto);

            LoadStudents();

            ClearForm(null);
        }

        private async void DeleteStudent(object parameter)
        {
            if (SelectedStudent == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete student '{SelectedStudent.FirstName} {SelectedStudent.LastName}'?",
                "Delete Student",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            await _studentApiService.DeleteStudentAsync(SelectedStudent.Id);

            Students.Remove(SelectedStudent);

            ClearForm(null);
        }

        private bool CanAddStudent(object parameter)
        {
            return SelectedStudent == null;
        }

        private bool CanEditSelectedStudent(object parameter)
        {
            return SelectedStudent != null;
        }

        private bool CanImportStudents(object parameter)
        {
            return !string.IsNullOrWhiteSpace(SelectedXmlFilePath)
                   && File.Exists(SelectedXmlFilePath);
        }

        private void ClearForm(object parameter)
        {
            SelectedStudent = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            Grade = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StudentViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }
    }
}