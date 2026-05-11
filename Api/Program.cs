using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Xml;
using MathProcessor.Interfaces;
using MathProcessor.Services;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ITestService, TestService>();
            builder.Services.AddScoped<IResultService, ResultService>();
            builder.Services.AddScoped<IExamXmlParser, ExamXmlParser>();
            builder.Services.AddScoped<IExamProcessingService, ExamProcessingService>();
            builder.Services.AddScoped<IMathExpressionProcessor, MathExpressionProcessor>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}