using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Ni.Models;

namespace Ni.Controllers;

public class HomeController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment environment;

    public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        this._configuration = configuration;
        this.environment = environment;
    }

    public HelloApiResult Index()
    {
        return new HelloApiResult()
        {
            Code = 0,
            Message = $"Welcome to EdgeNeko Api! Visit our wiki at {_configuration["WikiPath"]}",
            WikiPath = _configuration["WikiPath"],
            Mode = environment.EnvironmentName,
            ServerTime = DateTime.Now,
            ServerTimeUtc = DateTime.UtcNow,
            ServerRuntime = $"{RuntimeInformation.FrameworkDescription} on {RuntimeInformation.OSDescription}"
        };
    }
}
