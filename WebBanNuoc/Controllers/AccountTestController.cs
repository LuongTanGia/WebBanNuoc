using WebBanNuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebBanNuoc.DAL;
using System.Data.Entity.Validation;

namespace WebBanNuoc.Controllers
{
    public class AccountTestController : Controller
    {
        // GET: AccountTest
        DrinksStoreEntities1 db = new DrinksStoreEntities1();

        public ActionResult TestLogin()
        {
            return View();
        }
        public JsonResult SaveData(Tbl_Members model)
        {
            try
            {
                model.isValid = false;
                db.Tbl_Members.Add(model);
                db.SaveChanges();
                BuildEmailTemplate(model.MemberId);
                return Json("Registration Successfull", JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }
        public JsonResult RegisterConfirm(int regId)
        {
            Tbl_Members Data = db.Tbl_Members.Where(x => x.MemberId == regId).FirstOrDefault();
            Data.isValid = true;
            db.SaveChanges();
            var msg = "Your Email Is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public void BuildEmailTemplate(int regID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = db.Tbl_Members.Where(x => x.MemberId == regID).FirstOrDefault();
            var url = "https://localhost:44339/" + "AccountTest/Confirm?regId=" + regID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account Is Successfully Created", body, regInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "hhtt.tuvan@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("hhtt.tuvan@gmail.com", "H123456@");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult CheckValidUser(Tbl_Members model)
        {
           
            string result = "Fail";
            var DataItem = db.Tbl_Members.Where(x => x.UserName == model.UserName && x.Password == model.Password).SingleOrDefault();
            
                if (DataItem != null && DataItem.isValid == true)
                {
                    Session["MemberId"] = DataItem.MemberId.ToString();
                    Session["UserName"] = DataItem.UserName.ToString();
                Session["Name"] = DataItem.Name.ToString();
                result = "Success";
                }
            
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AfterLogin()
        {
            if (Session["MemberId"] != null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("TestLogin");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("TestLogin");
        }
    }
}