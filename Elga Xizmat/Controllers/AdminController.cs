using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elga_Xizmat.Models;
using Elga_Xizmat.Models.Class;
using System.IO;

namespace Elga_Xizmat.Controllers
{
    public class AdminController : Controller
    {
        ELX_DBEntities db = new ELX_DBEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;
            var model = new Adses();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddProduct(Adses ad, HttpPostedFileBase file)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;
            ad.Date = DateTime.Now;
            ad.User_ID = 1;
            ad.State_ID = 1;

            string path = Path.Combine(Server.MapPath("~/Content/img/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);

            path = Path.Combine(Server.MapPath("~/Content/img/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);

            ad.Picture = "/Content/img/" + file.FileName;

            db.Adses.Add(ad);
            db.SaveChanges();

            return View(ad);
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
        public ActionResult ProductList(int? product_rubric, int? stateProduct)
        {
            if (product_rubric == null && stateProduct != null)
            {
                return View(db.Adses.Where(x => x.State_ID == stateProduct));
            }
            else if (product_rubric != null && stateProduct == null)
            {
                return View(db.Adses.Where(x => x.Rubric_Id == product_rubric));
            }
            return View(db.Adses);
        }
        public ActionResult ProductEdit(int id)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;

            return View(db.Adses.Find(id));
        }

        [HttpPost]
        public ActionResult ProductEdit(Adses ads)
        {
            Adses ad = db.Adses.FirstOrDefault(x => x.Id == ads.Id);

            ad = ads;
            db.Entry(ad);
            db.SaveChangesAsync();

            return RedirectToAction("ProductList", "Admin");
        }
        public ActionResult ProductDelete(int id)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;
            ViewBag.ID = id;
            return View(db.Adses.Find(id));
        }

        [HttpPost]
        public ActionResult ProductDelete(Adses ads)
        {
            Adses a = db.Adses.FirstOrDefault(x => x.Id == ads.Id);
            db.Adses.Remove(a);
            db.SaveChanges();

            return RedirectToAction("ProductList", "Admin");
        }
        public ActionResult ProductDetails(int id)
        {
            ViewBag.StateList = db.Regions;
            ViewBag.RubricList = db.Rubric;
            ViewBag.ID = id;
            return View(db.Adses.Find(id));
        }

        public ActionResult Users()
        {
            return View(db.Users.Where(x => x.Type == 2));
        }
        public ActionResult Admins()
        {
            return View(db.Users.Where(x => x.Type == 1));
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }
        public ActionResult StateAdminOrUser(int id)
        {
            Users uu = db.Users.FirstOrDefault(x => x.ID == id);

            if (uu.State == 1)
            {
                uu.State = 2;
            }
            else
            {
                uu.State = 1;
            }

            db.Entry(uu);
            db.SaveChanges();
            //db.Users.Remove(uu);
            //db.SaveChanges();

            if (uu.Type == 1)
            {
                return RedirectToAction("Admins");
            }
            return RedirectToAction("Users");
        }
        public ActionResult EditUserOrAdmin(int id)
        {
            return View(db.Users.Find(id));
        }

        [HttpPost]
        public ActionResult EditUserOrAdmin(Users u)
        {
            Users p = db.Users.FirstOrDefault(x => x.ID == u.ID);

            p.User_Name = u.User_Name;
            p.Email = u.Email;
            p.Phone_number = u.Phone_number;
            p.Password = u.Password;

            db.Entry(p);
            db.SaveChanges();

            if (p.Type == 1)
            {
                return RedirectToAction("Admins");
            }
            return RedirectToAction("Users");
        }
        public ActionResult DeleteUserOrAdmin(int id)
        {
            ViewBag.ID = id;
            return View(db.Users.FirstOrDefault(x => x.ID == id));
        }

        [HttpPost]
        public ActionResult DeleteUserOrAdmin(Users u)
        {
            Users a = db.Users.FirstOrDefault(x => x.ID == u.ID);
            db.Users.Remove(a);
            db.SaveChanges();

            if (a.Type == 1)
            {
                return RedirectToAction("Admins");
            }
            return RedirectToAction("Users");
        }
        public ActionResult AddAdmin()
        {
            Users us = new Users();
            return View(us);
        }

        [HttpPost]
        public ActionResult AddAdmin(Users usr)
        {
            usr.Type = 1;
            usr.State = 1;
            db.Users.Add(usr);
            db.SaveChanges();

            return RedirectToAction("Admins", "Admin");
        }

        public ActionResult Buy(int id)
        {
            Adses a = db.Adses.FirstOrDefault(x => x.Id == id);
            a.Date = DateTime.Now;
            a.State_ID = 4;
            db.Entry(a);
            db.SaveChanges();

            return RedirectToAction("Productlist");
        }

        public ActionResult Actived(int id)
        {
            Adses a = db.Adses.FirstOrDefault(x => x.Id == id);
            a.Date = DateTime.Now;
            a.State_ID = 1;
            db.Entry(a);
            db.SaveChanges();

            return RedirectToAction("Productlist");
        }

        public ActionResult Rejection(int id)
        {
            Adses a = db.Adses.FirstOrDefault(x => x.Id == id);
            a.Date = DateTime.Now;
            a.State_ID = 5;
            db.Entry(a);
            db.SaveChanges();

            return RedirectToAction("Productlist");
        }

        public ActionResult Proba()
        {
            return View();
        }
        public ActionResult AddRubricVsTypeProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRubricVsTypeProduct(RubricVsTypeProduct r)
        {
            Rubric rub = new Rubric();
            rub.Name_Uz = r.rubric;
            db.Rubric.Add(rub);
            db.SaveChanges();

            Type_Product tProd = new Type_Product();
            tProd.Name_Uz = r.typeProduct;
            tProd.Rubric_Id = db.Rubric.FirstOrDefault(x => x.Name_Uz == r.rubric).Id;
            db.Type_Product.Add(tProd);

            db.SaveChanges();

            return RedirectToAction("Admins", "Admin");
        }

    }
}