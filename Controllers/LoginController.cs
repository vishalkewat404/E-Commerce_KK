using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult LoginPage()
		{
			return View();
		}
	}
}
