using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using MimeKit;
using MailKit.Net.Smtp;

namespace writeboard.ext.Controllers
{
    public class External : Controller
    {
        [HttpGet("/{wbAtion?}")]
        [HttpPost("/{wbAction?}")]
        public IActionResult Index(string wbAction)
        {
            //get current url, set app url
            ViewBag.currentURL = getCurrentURL(HttpContext);
            if (ViewBag.currentURL.toString().IndexOf("writeboard.net") >= 0) 
            {
                ViewBag.appURL = "http://app.writeboard.net";
            }
            else
            {
                ViewBag.appURL = "http://writeboard-app-tst.azurewebsites.net";
            }

            //process contact us action
            if (wbAction == "contact")
            {
                //get contact form entries
                string contactName = HttpContext.Request.Form["wb-name"];
                string contactEmail = HttpContext.Request.Form["wb-email"];
                string contactSubject = HttpContext.Request.Form["wb-subject"];
                string contactMsg = HttpContext.Request.Form["wb-msg"];

                //send successful registration email
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("writeboard-team", "writeboard-team@outlook.com"));
                message.To.Add(new MailboxAddress("writeboard-team", "writeboard-team@outlook.com"));
                message.Subject = "WriteBoard - Contact";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<b>" + contactName + " Sent:</b><p><p>" + contactSubject + "<p><p>" + contactMsg + "<p> Respond to: <a href='mailto://" + contactEmail + "'>" + contactEmail + "</a>";
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.live.com", 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("writeboard-team@outlook.com", "neuedu#2017");
                    client.Send(message);
                    client.Disconnect(true);
                }
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
