using Application.DTOs.ExamUpload;
using Microsoft.Win32;
using StudentMathTestSystem.Wpf.Commands;
using System.Collections.ObjectModel;
using System.IO;
using Wpf.Services.Api;

namespace Wpf.ViewModels
{
    public class TeacherUploadViewModel : BaseViewModel
    {
        private readonly TestApiService _testApiService;

        private string _selectedXmlFilePath;
        private string _statusMessage;
        private bool _isBusy;
        private int _processedStudentsCount;
        private int _processedExamsCount;
        private int _correctTasksCount;
        private int _incorrectTasksCount;

        public ObservableCollection<ExamResultDto> ProcessedResults { get; set; } = new ObservableCollection<ExamResultDto>();

        public int ProcessedStudentsCount
        {
            get => _processedStudentsCount;
            set
            {
                _processedStudentsCount = value;
                OnPropertyChanged();
            }
        }

        public int ProcessedExamsCount
        {
            get => _processedExamsCount;
            set
            {
                _processedExamsCount = value;
                OnPropertyChanged();
            }
        }

        public int CorrectTasksCount
        {
            get => _correctTasksCount;
            set
            {
                _correctTasksCount = value;
                OnPropertyChanged();
            }
        }

        public int IncorrectTasksCount
        {
            get => _incorrectTasksCount;
            set
            {
                _incorrectTasksCount = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand BrowseXmlCommand { get; }
        public RelayCommand UploadAndProcessCommand { get; }

        public string SelectedXmlFilePath
        {
            get => _selectedXmlFilePath;
            set
            {
                _selectedXmlFilePath = value;
                OnPropertyChanged();

                UploadAndProcessCommand?.RaiseCanExecuteChanged();
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

                UploadAndProcessCommand?.RaiseCanExecuteChanged();
            }
        }

        public TeacherUploadViewModel()
        {
            _testApiService = new TestApiService();

            BrowseXmlCommand = new RelayCommand(_ => BrowseXml());
            UploadAndProcessCommand = new RelayCommand(
                async _ => await UploadAndProcessAsync(),
                _ => CanUploadAndProcess());
        }

        private bool CanUploadAndProcess()
        {
            return !IsBusy
                   && !string.IsNullOrWhiteSpace(SelectedXmlFilePath)
                   && File.Exists(SelectedXmlFilePath);
        }

        private void BrowseXml()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                Title = "Select Exam XML File"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedXmlFilePath = dialog.FileName;
                StatusMessage = string.Empty;
            }
        }

        private async Task UploadAndProcessAsync()
        {
            if (!CanUploadAndProcess())
            {
                StatusMessage = "Please select a valid XML file first.";
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = "Uploading and processing XML...";

                var result = await _testApiService.UploadXmlAsync(SelectedXmlFilePath);

                ProcessedStudentsCount = result.TotalStudents;
                ProcessedExamsCount = result.TotalExams;

                CorrectTasksCount = result.Results
                    .SelectMany(x => x.Tasks)
                    .Count(x => x.IsCorrect);

                IncorrectTasksCount = result.Results
                    .SelectMany(x => x.Tasks)
                    .Count(x => !x.IsCorrect);

                ProcessedResults.Clear();

                if (result.Results != null)
                {
                    foreach (var examResult in result.Results)
                    {
                        ProcessedResults.Add(examResult);
                    }
                }

                StatusMessage = "XML uploaded and processed successfully.";
                SelectedXmlFilePath = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = "Upload failed: " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}