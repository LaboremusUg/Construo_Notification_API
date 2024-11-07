using AutoMapper;
using Construo.NotificationAPI.Models.Sms;
using Construo.NotificationAPI.Repository;
using Construo.NotificationAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Construo.NotificationAPI.Controllers;

[Route("api/client")]
public class ClientController : BaseController
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientController(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    // GET: api/<controller>
    /// <summary>
    /// Returns all configured clients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces(typeof(IEnumerable<ClientViewModel>))]
    public async Task<IActionResult> Get()
    {
        var clients = await _clientRepository.GetAll();
        var enumerable = clients as Client[] ?? clients.ToArray();
        if (enumerable.Any())
        {
            var passwordHiddenEnumerable = enumerable.Select(x =>
            {
                x.ServiceProviderPassword = "*******";
                return x;
            });
            var viewModel = _mapper.Map<IEnumerable<ClientViewModel>>(passwordHiddenEnumerable);
            return Ok(viewModel);
        }

        return NotFound();
    }

    // GET api/<controller>/5
    /// <summary>
    /// Returns details of the client with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces(typeof(ClientViewModel))]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _clientRepository.GetById(id);
        if (result != null)
        {
            result.ServiceProviderPassword = "*********";
            var viewModel = _mapper.Map<ClientViewModel>(result);
            return Ok(viewModel);
        }

        return NotFound();

    }

    // POST api/<controller>
    /// <summary>
    /// Creates a new client for sending SMS
    /// </summary>
    /// <param name="clientViewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces(typeof(IEnumerable<Client>))]
    public async Task<IActionResult> Post([FromBody] ClientViewModel clientViewModel)
    {
        if (ModelState.IsValid)
        {
            if (clientViewModel.Id == Guid.Empty)
            {
                clientViewModel.Id = Guid.NewGuid();
            }
            var client = _mapper.Map<Client>(clientViewModel);
            var result = await _clientRepository.CreateAsync(client);
            return Ok(result);
        }

        return BadRequest(ModelState);
    }
}
