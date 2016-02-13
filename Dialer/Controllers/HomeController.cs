using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dialer.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text;

namespace Dialer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Make the most of your precious time!";

        
            var httpWReq =
                (HttpWebRequest)WebRequest.Create("https://api.tropo.com/v1/sessions");

            var encoding = new ASCIIEncoding();
            var postDataTemplate = "<session>" +
                                   "<token>{0}</token>" +
                                   "<var name=\"numberToDial\" value=\"{1}\"></var>" +
                                   "<var name=\"msg\" value=\"{2}\"></var>" +
                                   "</session>";

            var tokenToUse = "564c7674644b434e6c76614f694c5350596c704c6a78706a6f6d634753646d4d505247467248565a6d6f784c";
            var numberToDial = "phone number"; // enter valid number
            var message = "Greetings. This is a reminder that you have a service call appointment scheduled.";

            var postData = string.Format(postDataTemplate, tokenToUse, numberToDial, message);

            var data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.Accept = "text/xml";
            httpWReq.ContentType = "text/xml";
            httpWReq.ContentLength = data.Length;

            var newStream = httpWReq.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            var response = (HttpWebResponse)httpWReq.GetResponse();
            byte[] buffer = new byte[response.ContentLength];
            using (var stream = response.GetResponseStream())
            {
                stream.Read(buffer, 0, (int)response.ContentLength);
            }
            var bufferAsString = buffer.Aggregate("", (current, t) => current + (char)t);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Did not get status OK 200 from POST");
            }
            newStream.Close();
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Info";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("x.com"));  // replace with valid value 
                message.From = new MailAddress("");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "",  // replace with valid value
                        Password = ""  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com	";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }
        public ActionResult Sent()
        {
            return View();
        }
    }
}