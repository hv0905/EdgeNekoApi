using Microsoft.AspNetCore.HttpOverrides;
using Ni.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<MusicService>();
builder.Services.AddHostedService<MusicUpdateService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddHttpClient("Chrome", options =>
{
    options.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
    options.DefaultRequestHeaders.UserAgent.TryParseAdd("AppleWebKit/537.36 (KHTML, like Gecko)");
    options.DefaultRequestHeaders.UserAgent.TryParseAdd("Chrome/105.0.0.0");
    options.DefaultRequestHeaders.UserAgent.TryParseAdd("Safari/537.36");
});
builder.Services.AddLogging();
builder.Logging.AddConsole();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.ForwardLimit = builder.Configuration.GetSection("transportSettings").GetValue("forwardLimit", 1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();