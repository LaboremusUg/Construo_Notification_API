using FluentValidation;
using Hangfire;
using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Services;
using Construo.NotificationAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Construo.NotificationAPI.Controllers;

/// <inheritdoc />
/// <summary>
/// Handles messaging
/// </summary>
[ProducesResponseType(typeof(OkResult), 200)]
[ProducesResponseType(typeof(NotFoundResult), 404)]
[ProducesResponseType(typeof(UnauthorizedResult), 401)]
[ProducesResponseType(typeof(BadRequestResult), 500)]
[Produces("application/json")]
[Route("api")]
public class MessageController : BaseController
{
    private readonly IEmailService _emailService;
    private readonly IMessageLoggingService _messageLoggingService;
    private readonly ISmsService _smsService;
    private readonly IValidator<Sms> _smsValidator;

    private readonly ILogger<MessageController> _logger;

    /// <inheritdoc />
    public MessageController(IEmailService emailService, ILogger<MessageController> logger, ISmsService smsService,
        IValidator<Sms> smsValidator, IMessageLoggingService messageLoggingService)
    {
        _emailService = emailService;
        _logger = logger;
        _smsService = smsService;
        _smsValidator = smsValidator;
        _messageLoggingService = messageLoggingService;
    }

    /// <summary>
    /// Send email
    /// </summary>
    /// <param name="emailDetails">The object that contains all the details about the email to be sent</param>
    /// <returns></returns>
    [HttpPost("email")]
    public async Task<CustomResponse> SendEmail([FromBody] EmailDetails emailDetails)
    {
        var response = new CustomResponse();

        _logger.LogInformation("Initializing email sending request...");

        if (emailDetails == null)
        {
            _logger.LogCritical($"NullReferenceException: EmailDetails was null");

            response.Message = "EmailDetails was null";
            response.StatusCode = HttpStatusCode.BadRequest;
        }

        else if (!ModelState.IsValid)
        {
            _logger.LogCritical(
                $"Validation failed: {ModelState.Values.SelectMany(s => s.Errors.Select(q => q.ErrorMessage))}");

            response.Message = "EmailDetails validation failed";
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        else
        {
            _logger.LogInformation("Adding message to the queue...");
            var messageQueueItem = await _messageLoggingService.CreateAsync(emailDetails.MessageQueueItem());
            BackgroundJob.Schedule(() => _emailService.SendMailAsync(emailDetails, messageQueueItem.Id),
                emailDetails.TimeToSend);

            _logger.LogInformation("The email has been successfully added to the queue");

            response.Message = "The email has been successfully added to the queue";
            response.StatusCode = HttpStatusCode.OK;
        }

        return response;
    }

    /// <summary>   
    /// Endpoint that allows a client to POST an SMS to be sent
    /// </summary>
    /// <param name="sms"></param>
    /// <returns></returns>
    [HttpPost("sms")]
    public ActionResult SendSms([FromBody] Sms sms)
    {
        _logger.LogInformation($"sending sms to:{string.Join(",", sms.Recipients)}");
        var validationResult = _smsValidator.Validate(sms);
        if (validationResult.IsValid)
        {
            BackgroundJob.Enqueue(() => _smsService.SendAsync(sms));
            return Ok(new
            {
                message = "Message added to queue"
            });
        }

        _logger.LogError($"Invalid request {JsonConvert.SerializeObject(validationResult)}");

        return BadRequest(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
    }
}
