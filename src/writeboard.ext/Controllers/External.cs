using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace writeboard.ext.Controllers
{
    public class External : Controller
    {
        public IActionResult Index()
        {
            //get current url, set app url
            string currentURL = getCurrentURL(HttpContext);
            if (currentURL == "http://writeboard.net")
            {
                ViewBag.appURL = "http://app.writeboard.net";
            }
            else
            {
                ViewBag.appURL = "http://writeboard-app-tst.azurewebsites.net";
            }

            //get writeboard unique registration count
            SqlConnection conn = new SqlConnection("Server=writeboard-db.database.windows.net;Database=writeboard;User Id=wbadmin;Password=neuedu#2017");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)FROM dbo.writeboards", conn);
            ViewBag.wbCount = (int)cmd.ExecuteScalar();
            conn.Close();
            return View();
        }

        public string getCurrentURL(HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}";
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
