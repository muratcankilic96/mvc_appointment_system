using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCKuafor.Models;

namespace MVCKuafor.Controllers
{
    public class RegisterController : Controller
    {
        private MVCKuaforContext db = new MVCKuaforContext();


        // GET: Register/Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        // POST: Register/Index
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,Name,Surname,Username,Email,Password,RegisterDate,isAdmin")] CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {

                customerModel.RegisterDate = DateTime.Now;
                db.CustomerModels.Add(customerModel);
                db.SaveChanges();
                TempData["Success"] = "Kayıt başarıyla gerçekleştirildi, "+ customerModel.Name + ".";
                return RedirectToAction("Success");
              
            }

            return View(customerModel);
        }

      

        public JsonResult IsUserExists(string Username)
        {
            return Json(!db.CustomerModels.Any(x => x.Username == Username), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExists(string Email)
        {
            return Json(!db.CustomerModels.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
