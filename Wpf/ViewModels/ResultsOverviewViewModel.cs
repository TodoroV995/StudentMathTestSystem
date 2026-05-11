using Application.DTOs.Results;
using StudentMathTestSystem.Wpf.Commands;
using System.Collections.ObjectModel;
using Wpf.Services.Api;

namespace Wpf.ViewModels
{
    public class ResultsOverviewViewModel : BaseViewModel
    {
        private readonly TestApiService _testApiService;

        public ObservableCollection<TestResultDto> Results { get; set; } = new ObservableCollection<TestResultDto>();

        public RelayCommand RefreshCommand { get; }

        public ResultsOverviewViewModel()
        {
            _testApiService = new TestApiService();

            RefreshCommand = new RelayCommand(async _ => await LoadResultsAsync());

            LoadResultsAsync();
        }

        private async Task LoadResultsAsync()
        {
            Results.Clear();

            var results = await _testApiService.GetAllResultsAsync();

            foreach (var result in results)
            {
                Results.Add(result);
            }
        }
    }
}