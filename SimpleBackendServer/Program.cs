var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// header check to ensure is coming only from loadbalancer:
app.Use(async (context, next) =>
{
    var secret = "my-secret";
if (!context.Request.Headers.TryGetValue("X-LB-SECRET", out var headerValue) ||
    headerValue != secret) {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Forbidden");
        return;
    }
    await next();
});

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
