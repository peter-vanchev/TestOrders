using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestOrders.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
