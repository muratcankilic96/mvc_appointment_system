using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKuafor.Controllers
{
    public class AdministrationController : Controller

    {



        [HttpPost]
        public ActionResult Employee(string employeeField, string action)
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            Debug.WriteLine(employeeField + " is sent.");           
            cn.Open();
            if(action.Equals("delete")) { 
                using (SqlCommand command = new SqlCommand("DELETE FROM " + "Employees" + " WHERE " + "employee_id" + " = '" + employeeField + "'", cn))
                {
                    command.ExecuteNonQuery();
                    cn.Close();
                    return RedirectToAction("Employee");
                }
            }
            else
            {
                cn.Close();
                TempData["to-edit-id"] = employeeField;
                return RedirectToAction("EditEmployee");
            }
        }

        [HttpPost]
        public ActionResult EditEmployee()
        {
            return View();
        }

            // GET: EditEmployee
            public ActionResult EditEmployee(Models.Employees e) {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM Employees WHERE employee_id = " + ((TempData["to-edit-id"] != null) ? TempData["to-edit-id"] : -1);
            Debug.WriteLine(_sql);
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                e.employee_id = sdr.GetInt32(0);
                e.name = sdr.GetString(1);
                e.surname = sdr.GetString(2);
                e.username = sdr.GetString(3);
                e.password = sdr.GetString(4);
                e.email = sdr.GetString(5);
                e.telephonenumber = sdr.GetInt64(6);
            }
            cn.Close();
            return View(e);
        }


    // GET: Employee
    public ActionResult Employee()
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM Employees";
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            ViewData["employee-list"] = "";
            while (sdr.Read())
            {
                ViewData["employee-list"] += "<li name='employee-element' id='employee-" + sdr.GetInt32(0) + "'>" + sdr.GetString(1) + " " + sdr.GetString(2) + " " + sdr.GetString(3) + " " + sdr.GetString(4) + " " + sdr.GetString(5) + " " + sdr.GetInt64(6).ToString() + "</li>";
            }
            cn.Close();
            return View();
        }

        // GET: Administration
        public ActionResult Index()
        {
            return RedirectToAction("Employee");
        }
    }
}