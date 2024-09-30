using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Construo.NotificationAPI.Controllers;

[Authorize]
public class BaseController : Controller
{
}
