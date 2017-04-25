using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace writeboard.ext.Controllers
{
    public class External : Controller
    {
        public IActionResult Index()
        {
            string appURL = "";
            if (HttpContext.Current.Request.URL == "http://writeboard.net")
            {
                ViewBag.appURL = "http:app//writeboard.net"
            }

            else
            {
                ViewBag.appURL = "writeboard-app-tst.azurewebsites.net"
            }
            SqlConnection conn = new SqlConnection("Server=writeboard-db.database.windows.net;Database=writeboard;User Id=wbadmin;Password=neuedu#2017");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)FROM dbo.writeboards", conn);
            ViewBag.wbCount = (int)cmd.ExecuteScalar();
            conn.Close();
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
