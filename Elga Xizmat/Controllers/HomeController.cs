using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elga_Xizmat.Models;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Elga_Xizmat.Controllers
{
    public class HomeController : Controller
    {
        ELX_DBEntities db = new ELX_DBEntities();

        public ActionResult Index()
        {
            ViewBag.reklama = db.Adverts.FirstOrDefault(x=>x.Status==1);

            
             

           foreach (var item in db.Adverts.Where(x=>x.Status==1))
            {

                if (item.NeedCountView!=null)
                {
                     if (item.NeedCountView > 0)
                      {
                          item.NeedCountView--;
                      }
                    else
                    {

                        item.NeedCountView = null;
                        item.Status = 0;
                    }
                }

                else if(item.NeedDayCount != null)
                {

                    if( (item.Date.Value.Day-DateTime.Now.Day) == item.NeedDayCount)
                    {
                        item.NeedDayCount = null;
                        item.Status = 0;
                    }

                }
                
            }
                       
            db.SaveChanges();
                        
            return View(db.Adses);
        }
        public ActionResult Pruducts(int id)
        {
            return View(db.Adses.Where(x => x.Rubric_Id == id));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Message msg)
        {
            db.Message.Add(msg);
            db.SaveChanges();

            return View();
        }
        public ActionResult Food()
        {

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            Users result = db.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (result != null)
            {

                if (result.Type == 2)
                {
                    Session["Active"] = "1";
                    Session["User"] = result.ID;
                    Session["Type"] = result.Type;
                    return RedirectToAction("ProductList", "Home");
                }
                Session["Active"] = "1";
                Session["User"] = result.ID;
                Session["Type"] = result.Type;
                return RedirectToAction("Index", "Admin");
            }



            return View();
        }
        public ActionResult Exit()
        {
            Session["Active"] = "0";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(string name, string phone, string email, string password, string confirm_password )
        //{
        //    if (password == confirm_password) { 
        //    Users u = new Users();
        //    u.User_Name = name;
        //    u.Email = email;
        //    u.Phone_number = Convert.ToDecimal(phone);
        //    u.Password = password;

        //        db.Users.Add(u);
        //        db.SaveChanges();

        //        return RedirectToAction("Index", "Admin");
        //    }

        //    return View();
        //}
        public ActionResult Add()
        {
            try
            {
                if (Session["Active"].ToString() != "1") return RedirectToAction("Login", "Home");

            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;

            ViewBag.Id = Session["User"].ToString();
            int id = Int32.Parse(ViewBag.Id);

            Users u = db.Users.FirstOrDefault(x => x.ID == id);
            ViewBag.Usertype = u.Type;

            var model = new Adses();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Adses ad, HttpPostedFileBase file)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;



            ad.Date = DateTime.Now;
            ad.User_ID = Convert.ToInt32(Session["User"]);
            ad.State_ID = 2;

            string path = Path.Combine(Server.MapPath("~/Content/img/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);

            path = Path.Combine(Server.MapPath("~/Content/img/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);

            ad.Picture = "/Content/img/" + file.FileName;

            db.Adses.Add(ad);
            db.SaveChanges();

            return RedirectToAction("ProductList", "Home");
        }

        public ActionResult FillCity(int Region_Id)
        {
            var areas = db.Areas.Where(c => c.Region_Id == Region_Id);
            return Json(areas, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillRubric(int Rubric_Id)
        {
            var typeProducts = db.Type_Product.Where(c => c.Rubric_Id == Rubric_Id);
            return Json(typeProducts, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductList()
        {
            ViewBag.user = Convert.ToInt32(Session["User"]);
            int id = (int)ViewBag.user;

            return View(db.Adses.Where(m => m.User_ID == id).OrderByDescending(x => x.Id));
        }
        public ActionResult ProductEditorDelete(int id)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;



            return View(db.Adses.FirstOrDefault(x => x.Id == id));
        }
        public ActionResult ProductEdit(Adses ads)
        {
            Adses ad = db.Adses.FirstOrDefault(x => x.Id == ads.Id);

            ad = ads;
            db.Entry(ad);
            db.SaveChanges();

            return RedirectToAction("ProductList", "Home");
        }
        public ActionResult ProductDelete(Adses ads)
        {
            Adses a = db.Adses.FirstOrDefault(x => x.Id == ads.Id);
            db.Adses.Remove(a);
            db.SaveChanges();

            return RedirectToAction("ProductList", "Home");
        }
        public ActionResult ProductDetails(int id)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;

            return View(db.Adses.FirstOrDefault(x => x.Id == id));
        }

        public ActionResult SendMessage()
        {

            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        [HttpPost]

        public async Task<ActionResult> Register(Users model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.User_Name };
                user.Email = model.Email;
                user.ConfirmedEmail = false;
                //var result = await UserManager.CreateAsync(user, model.Password);
                //if (result.Succeeded)
                //{
                // наш email с заголовком письма
                MailAddress from = new MailAddress("somemail@yandex.ru", "Web Registration");
                // кому отправляем
                MailAddress to = new MailAddress(user.Email);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Email confirmation";
                // текст письма - включаем в него ссылку
                m.Body = string.Format("Для завершения регистрации перейдите по ссылке:" +
                                "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>",
                    Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                m.IsBodyHtml = true;
                // адрес smtp-сервера, с которого мы и будем отправлять письмо
                SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.yandex.ru", 25);
                // логин и пароль
                smtp.Credentials = new System.Net.NetworkCredential("somemail@yandex.ru", "password");
                smtp.Send(m);
                return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                //}
                //else
                //{
                //    AddErrors(result);
                //}
            }
            return View(model);
        }

        [AllowAnonymous]
        public string Confirm(string Email)
        {
            return "На почтовый адрес " + Email + " Вам высланы дальнейшие" +
                    "инструкции по завершению регистрации";
        }

        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        //{
        //    ApplicationUser user = this.UserManager.FindById(Token);
        //    if (user != null)
        //    {
        //        if (user.Email == Email)
        //        {
        //            user.ConfirmedEmail = true;
        //            await UserManager.UpdateAsync(user);
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });
        //        }
        //        else
        //        {
        //            return RedirectToAction("Confirm", "Account", new { Email = user.Email });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Confirm", "Account", new { Email = "" });
        //    }
        //}


    }
}