using Application.DTOs.ExamUpload;
using Application.DTOs.Results;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Wpf.Services.Api
{
    public class TestApiService : ApiServiceBase
    {
        public async Task<ExamUploadResultDto?> UploadXmlAsync(string filePath)
        {
            using (var form = new MultipartFormDataContent())
            using (var fileStream = File.OpenRead(filePath))
            {
                var fileContent = new StreamContent(fileStream);

                fileContent.Headers.ContentType =
                    new MediaTypeHeaderValue("application/xml");

                form.Add(
                    fileContent,
                    "file",
                    Path.GetFileName(filePath));

                var response = await _httpClient.PostAsync(
                    "api/ExamUploads/xml",
                    form);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ExamUploadResultDto>(
                    JsonOptions);
            }
        }

        public async Task<List<TestResultDto>> GetAllResultsAsync()
        {
            return await GetAsync<List<TestResultDto>>("api/Results")
                   ?? new List<TestResultDto>();
        }
    }
}