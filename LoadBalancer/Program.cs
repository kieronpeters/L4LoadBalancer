var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<BackgroudHealthCheckerService>();
builder.Services.AddSingleton<IHealthCheckerService, HealthCheckerService>();
builder.Services.AddSingleton<IStatusReporterService, StatusReporterService>();
builder.Services.AddSingleton<IConnectionService, ConnectionService>();

//add port arguements for server to create list of backendUrls registered
var urls = args.Select(x => $"https://localhost:{x}").ToList();

var backendConfig = new BackendConfig() { BackendUrls = urls};
builder.Services.AddSingleton(backendConfig);

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