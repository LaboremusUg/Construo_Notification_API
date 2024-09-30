using Construo.NotificationAPI.Models.Sms;
using Construo.NotificationAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Construo.NotificationAPI.Controllers;

[Route("api/sms-providers")]
public class ServiceProviderController : BaseController
{
    private readonly IServiceProviderRepository _serviceProviderRepo;
    public ServiceProviderController(IServiceProviderRepository repo)
    {
        _serviceProviderRepo = repo;
    }
    // GET: api/<controller>
    /// <summary>
    /// Returns all configured service providers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<SmsServiceProvider>> Get()
    {
        return await _serviceProviderRepo.GetAll();
    }
    // GET: api/<controller>
    /// <summary>
    /// Returns a service provider with the specified name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("search/{name}")]
    [Produces(type: typeof(List<SmsServiceProvider>))]
    public IActionResult Get(string name)
    {
        var result = _serviceProviderRepo.Search(b => b.Name.StartsWith(name));
        //Use a view model
        return Ok(result.Select(x => new { x.Id, x.Name }).ToList());
    }

    // GET api/<controller>/5
    /// <summary>
    /// Returns details of a given service provider
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces(type: typeof(SmsServiceProvider))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _serviceProviderRepo.GetByIdAsync(id);
        //Use a view model
        return Ok(new { result.Id, result.Name });
    }
}
