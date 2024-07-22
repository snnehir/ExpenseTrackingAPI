using ExpenseTrackingApp.Services.Services.Mail;
using ExpenseTrackingApp.WebAPI.Middlewares;
using Hangfire;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.MapsterConfigurations();
builder.Services.AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


//builder.Services.AddLogging(); // microsoft logger

// serilog example
Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("log.txt",
		rollingInterval: RollingInterval.Day,
		rollOnFileSizeLimit: true)
	.CreateLogger();
Log.Information("Starting up...");
builder.Logging.ClearProviders();

builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAndConfigureSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContextService();
builder.Services.AddRedisCache();
builder.Services.AddMemoryCache();
builder.Services.AddAndConfigureAuthentication();
var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
														   .UseSimpleAssemblyNameTypeSerializer()
														   .UseRecommendedSerializerSettings()
														   .UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

var app = builder.Build();
// app.UseMiddleware<ExceptionHandlingMiddlewareMicrosoft>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();