using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_Commerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            HttpContext.Session.Clear(); // Clear old session
            return View();
        }

        [HttpPost]
        public IActionResult LoginPage(LoginPageClass user)
        {
            if (!ModelState.IsValid)
                return View(user);

            string conString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(conString))
            {
                if (con.State==System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
               
                string query = "SELECT [User_Id] ,[User_Name] ,[User_Email] ,[User_Password] ,[IsActive] FROM [E_Commerce].[dbo].[tbl_User] where User_Email=@Email and User_Password=@Password and IsActive=1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", user.email);
                    cmd.Parameters.AddWithValue("@Password", user.password); // ⚠️ Plain text (use hashing in production)

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        // Store session
                        HttpContext.Session.SetString("User_Id", dr["User_Id"].ToString());
                        HttpContext.Session.SetString("User_Name", dr["User_Name"].ToString());
                        HttpContext.Session.SetString("User_Email", dr["User_Email"].ToString());
                        HttpContext.Session.SetString("User_Password", dr["User_Password"].ToString());

                        return RedirectToAction("AdminDashBoard", "Admin"); // redirect after login
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid Email or Password";
                        return View(user);
                    }
                }
            }
        }
    }
}
