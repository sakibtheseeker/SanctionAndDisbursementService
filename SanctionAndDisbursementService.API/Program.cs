using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using SanctionAndDisbursementService.API.Exception;
using SanctionsAndDisbursementService.Application.DTO;
using SanctionsAndDisbursementService.Application.DTO.LoanDeals;
using SanctionsAndDisbursementService.Application.Interfaces;
using SanctionsAndDisbursementService.Application.Mapper;
using SanctionsAndDisbursementService.Infrastructure.Data;
using SanctionsAndDisbursementService.Infrastructure.Repository;
using Serilog;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("Logs/log-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();


builder.Host.UseSerilog();


// Add services to the container.
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("dbconn")
    ));


builder.Services.AddScoped<ISanctionRepo, SanctionRepo>();
builder.Services.AddScoped<IDisbursementRepo, DisbursementRepo>();


builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;

        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

    });


builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails(); 

Log.Error("Test error log");

builder.Services.AddHttpClient<LoanDealsClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["LoanDealsAddress"] ?? "https://localhost:7094");
});


var app = builder.Build();

app.UseExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();


app.Run();
