using InterviewTest001.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace InterviewTest001.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public ActionResult UploadFiles(string email, string fullname)
        {
            Random rd = new Random();
            var uniqueCode = "TN/" + rd.Next(1111, 9999) + "/" + rd.Next(0, 1000);
            using (InterviewTest001.Models.Context.AppDataContext db = new Models.Context.AppDataContext())
            {
                var chkEmail = db.UserInformations.Where(f => f.Email == email).FirstOrDefault();

                if (chkEmail.Equals(email))
                {
                    return Content("Email already exists");
                }
                else
                {
                    UserInformation newUser = new UserInformation
                    {
                        Email = email,
                        FullName = fullname,
                        TransactionNo = uniqueCode,
                        UserId = Guid.NewGuid()
                    };
                    db.UserInformations.Add(newUser);
                    db.SaveChanges();

                    Session["UserEmail"] = email;
                }
            }
            try
            {
                string FileName = "";
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {

                    HttpPostedFileBase file = files[i];
                    string fname;
                    fname = file.FileName;
                    FileName = file.FileName;

                    // Get the complete folder path and store the file inside it.    
                    fname = Path.Combine(Server.MapPath("~/Content/Documents/"), fname);
                    file.SaveAs(fname);

                    //Code to save inside database
                    using (InterviewTest001.Models.Context.AppDataContext db = new Models.Context.AppDataContext())
                    {
                        UserDocument newDoc = new UserDocument
                        {
                            DocumentId = Guid.NewGuid(),
                            DocumentTitle = FileName,
                            DocumentPath = "/Content/Documents/" + fname,
                            DocumentSize = file.ContentLength,
                            DateCreated = DateTime.Now,
                            CreatedBy = email,
                            TransactionNo = uniqueCode
                        };
                        db.UserDocuments.Add(newDoc);
                        db.SaveChanges();

                        SendEmail(email, uniqueCode);
                    }                                                     
                }
                return Json(uniqueCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(err.Message, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Page_2(string code)
        {
            var email = Session["UserEmail"] as string;
            if (email == null)
            {
                return Content("Can not find user email. Please login to continue");
            }

            using (InterviewTest001.Models.Context.AppDataContext db = new Models.Context.AppDataContext())
            {
                var files = db.UserDocuments.Where(f => f.CreatedBy == email && f.TransactionNo == code).ToList();

                if (!String.IsNullOrEmpty(email))
                {
                    files = files.Where(s => s.CreatedBy.Contains(email)).ToList();
                }
                if (files.Count >= 1)
                {
                    return View(files);

                }
                else
                {
                    List<UserDocument> doc = new List<UserDocument>();
                    return View(doc);
                }
            }
        }

        private ActionResult DeleteFile(string id)
        {
            using (InterviewTest001.Models.Context.AppDataContext db = new Models.Context.AppDataContext())
            {
                //Code to Delete From Database and folder here

            }
            return View();
        }


        private void SendEmail(string email, string uniqueid)
        {
         
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Users uploaded documents";
                mail.Body = "Find your attached documents below. Your transaction id is " + uniqueid;

                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment();
                //mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("email@gmail.com", "use a password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
        }
    }
}