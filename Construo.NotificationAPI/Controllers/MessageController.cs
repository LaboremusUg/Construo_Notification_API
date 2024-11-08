using Construo.NotificationAPI.Core.Helpers;
using Construo.NotificationAPI.Models;
using Construo.NotificationAPI.Services;
using Construo.NotificationAPI.ViewModels;
using FluentValidation;
using Hangfire;
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
            _logger.LogCritical("NullReferenceException: EmailDetails was null");
            return new CustomResponse
            {
                Message = "EmailDetails was null",
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        if (!ModelState.IsValid)
        {
            var errors = string.Join(", ", ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage));
            _logger.LogCritical($"Validation failed: {errors}");

            return new CustomResponse
            {
                Message = "EmailDetails validation failed",
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        try
        {
            _logger.LogInformation("Adding message to the queue...");

            // Create message queue item
            var messageQueueItem = await _messageLoggingService.CreateAsync(emailDetails.MessageQueueItem());

            // Schedule email sending
            BackgroundJob.Schedule(() => _emailService.SendMailAsync(emailDetails, messageQueueItem.Id),
                                   emailDetails.TimeToSend);

            _logger.LogInformation("The email has been successfully added to the queue");

            return new CustomResponse
            {
                Message = "The email has been successfully added to the queue",
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while processing the email: {ex.Message}");
            return new CustomResponse
            {
                Message = "An error occurred while processing your request",
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
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
