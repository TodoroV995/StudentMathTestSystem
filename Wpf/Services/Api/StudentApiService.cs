using Application.DTOs.Results;
using Application.DTOs.Students;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Wpf.Services.Api
{
    public class StudentApiService : ApiServiceBase
    {
        public async Task<List<StudentDto>> GetStudentsAsync()
        {
            return await GetAsync<List<StudentDto>>("api/Students")
                   ?? new List<StudentDto>();
        }

        public async Task<StudentDto?> CreateStudentAsync(CreateStudentDto dto)
        {
            return await PostAsync<CreateStudentDto, StudentDto>(
                "api/Students",
                dto);
        }

        public async Task<List<TestResultDto>> GetStudentResultsAsync(int studentId)
        {
            return await GetAsync<List<TestResultDto>>(
                       $"api/Results/student/{studentId}")
                   ?? new List<TestResultDto>();
        }

        public async Task UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/Students/{id}",
                dto,
                JsonOptions);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteStudentAsync(int id)
        {
            await DeleteAsync($"api/Students/{id}");
        }

        public async Task<ImportStudentsResultDto?> ImportStudentsFromXmlAsync(string filePath)
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
                    "api/Students/import-xml",
                    form);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ImportStudentsResultDto>(
                    JsonOptions);
            }
        }
    }
}