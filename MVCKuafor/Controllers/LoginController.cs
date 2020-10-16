using MVCKuafor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKuafor.Controllers
{
    public class LoginController : Controller
    {
        // GET: Logout
        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["ID"] = null;
            Session["isAdmin"] = null;
            Session["isEmployee"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Valid
        public ActionResult Valid()
        {
            return View();
        }

        // GET: Invalid
        public ActionResult Invalid()
        {
            return View();
        }

        // GET: Login
        public ActionResult Index()
        {
            return RedirectToAction("customer");
        }

        // GET: Customer
        public ActionResult Customer()
        {
            return View();
        }

        // GET: Employee
        public ActionResult Employee()
        {
            return View();
        }

        // POST: Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customer(CustomerLoginModel cl)
        {
            if(ModelState.IsValid)
            {
                if(cl.CheckValidity(cl.Username, cl.Password))
                {
                    TempData["Valid"] = "Kullanıcı girişi başarıyla gerçekleştirildi.";
                    Session["username"] = cl.Username;
                    Session["ID"] = cl.ID;
                    Session["isAdmin"] = cl.isAdmin;
                    Debug.WriteLine("Session variables: " + Session["username"].ToString() + " " + Session["ID"].ToString() + " " + Session["isAdmin"].ToString());
                    return RedirectToAction("valid");
                }
                else
                {
                    TempData["Invalid"] = "Böyle bir kullanıcı adı ve şifre kombinasyonu eşleşmiyor.";
                    return RedirectToAction("invalid");
                }
            }
            return View();
        }

        // POST: Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Employee(CustomerLoginModel cl)
        {
            if (ModelState.IsValid)
            {
                if (cl.CheckEmployeeValidity(cl.Username, cl.Password))
                {
                    TempData["Valid"] = "Çalışan girişi başarıyla gerçekleştirildi.";
                    Session["username"] = cl.Username;
                    Session["ID"] = cl.ID;
                    Session["isAdmin"] = cl.isAdmin;
                    Session["isEmployee"] = 1;
                    Debug.WriteLine("Session variables: " + Session["username"].ToString() + " " + Session["ID"].ToString() + " " + Session["isAdmin"].ToString() + " " + Session["isEmployee"].ToString());
                    return RedirectToAction("valid");
                }
                else
                {
                    TempData["Invalid"] = "Böyle bir kullanıcı adı ve şifre kombinasyonu eşleşmiyor.";
                    return RedirectToAction("invalid");
                }
            }
            return View();
        }
    }
}