using MVCKuafor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKuafor.Controllers
{

    public class AppointmentController : Controller
    {
        private MVCKuaforContext db = new MVCKuaforContext();

        [HttpPost]
        public ActionResult Waitlist(string rendezvousField)
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            cn.Open();
            using (SqlCommand command = new SqlCommand("DELETE FROM " + "Rendezvous" + " WHERE " + "RendezvousId" + " = '" + rendezvousField + "'", cn))
            {
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Waitlist");
        }

        [HttpPost]
        public ActionResult Info(string rendezvousField)
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            Debug.WriteLine(rendezvousField + " is sent.");
            //string _sql = "SELECT RendezvousId, TimeSlot, name, surname FROM Rendezvous r, Employees e WHERE CustomerId = " + ((Session["ID"] != null) ? (Session["ID"]) : (0)) + " AND employee_id = employeeId";
            cn.Open();
            using (SqlCommand command = new SqlCommand("DELETE FROM " + "Rendezvous" + " WHERE " + "RendezvousId" + " = '" + rendezvousField + "'", cn))
            {
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Info");
        }

        public ActionResult Waitlist()
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT RendezvousId, TimeSlot, name, surname FROM Rendezvous r, CustomerModels c WHERE employeeId = " + ((Session["ID"] != null) ? (Session["ID"]) : (0)) + " AND customerId = ID AND timeslot > getdate()";
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            ViewData["rendezvous-list"] = "";
            while (sdr.Read())
            {
                ViewData["rendezvous-list"] += "<li name='rendezvous-element' id='rendezvous-" + sdr.GetInt32(0) + "'>" + sdr.GetDateTime(1) + " " + sdr.GetString(2) + " " + sdr.GetString(3) + "</li>";
            }
            cn.Close();
            return View();
        }

        public ActionResult Info()
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT RendezvousId, TimeSlot, name, surname FROM Rendezvous r, Employees e WHERE CustomerId = " + ((Session["ID"] != null) ? (Session["ID"]) : (0)) + " AND employee_id = employeeId AND timeslot > getdate()";
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            ViewData["rendezvous-list"] = "";
            while (sdr.Read())
            {
                ViewData["rendezvous-list"] += "<li name='rendezvous-element' id='rendezvous-"+ sdr.GetInt32(0) +"'>" + sdr.GetDateTime(1) + " " + sdr.GetString(2) + " " + sdr.GetString(3) + "</li>";
            }
            cn.Close();
            return View();
        }

        [HttpPost]
        public ActionResult Calendar([Bind(Include = "TimeSlot,CustomerId,EmployeeId,IsUnavailable")] Rendezvous r)
        {

            if (ModelState.IsValid)
            {
                if(r.IsUnavailable != 2) { 
                    r.RendezvousId = new Random().Next() % 100000000;
                    Debug.WriteLine(r.RendezvousId + " " + r.IsUnavailable + " " + r.EmployeeId + " " + r.TimeSlot);
                    db.Rendezvouses.Add(r);
                    db.SaveChanges();
                }
                else
                {
                    SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
                    cn.Open();
                    Debug.WriteLine(r.TimeSlot.ToString());
                    string timeconvert = r.TimeSlot.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    using (SqlCommand command = new SqlCommand("DELETE FROM " + "Rendezvous" + " WHERE " + "timeslot = " + "'" + timeconvert + "'", cn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                Debug.WriteLine("Something is invalid.");
            }
            return RedirectToAction("Calendar");
        }

        [HttpPost]
        public ActionResult ByDate([Bind(Include = "TimeSlot,EmployeeId")] Rendezvous r)
        {
       
            if(ModelState.IsValid)
            {
                r.RendezvousId = new Random().Next() % 100000000;
                r.CustomerId = int.Parse(Session["ID"].ToString());
                r.IsUnavailable = 0;
                Debug.WriteLine(r.RendezvousId + " " + r.IsUnavailable + " " + r.EmployeeId + " " + r.TimeSlot);
                db.Rendezvouses.Add(r);
                db.SaveChanges();
            }
            else
            {
                Debug.WriteLine("Something is invalid.");
            }
            return RedirectToAction("ByDate");
        }

        [HttpPost]
        public ActionResult ChangeEmployee(string nameSurnameField)
        {
            Debug.WriteLine("Dude! You sent: " + nameSurnameField);

            string[] ecm_split = nameSurnameField.Split(' ');

            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM Employees WHERE name = '" + ecm_split[0] + "' AND surname = '" + ecm_split[1] + "'";
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            while(sdr.Read()) { 
                Session["employee-id"] = sdr.GetInt32(0);
            }
            Debug.WriteLine(Session["employee-id"]);
            return RedirectToAction("ByDate");
        }

        public int TimeToIntConversion(DateTime timeOfDay) {
            string date = timeOfDay.TimeOfDay.ToString();
            if (date.Equals("08:00:00"))
            {
                return 1;
            }
            else if (date.Equals("09:00:00"))
            {
                return 2;
            }
            else if (date.Equals("10:00:00"))
            {
                return 3;
            }
            else if (date.Equals("11:00:00"))
            {
                return 4;
            }
            else if (date.Equals("12:00:00"))
            {
                return 5;
            }
            else if (date.Equals("13:00:00"))
            {
                return 6;
            }
            else if (date.Equals("14:00:00"))
            {
                return 7;
            }
            else if (date.Equals("15:00:00"))
            {
                return 8;
            }
            else if (date.Equals("16:00:00"))
            {
                return 9;
            }
            else if (date.Equals("17:00:00"))
            {
                return 10;
            }
            else
            {
                return 0;
            }
        }

        public string IntToStringConversion(int timeSlot)
        {
            switch(timeSlot)
            {
                case 1:
                    return "08:00";
                case 2:
                    return "09:00";
                case 3:
                    return "10:00";
                case 4:
                    return "11:00";
                case 5:
                    return "12:00";
                case 6:
                    return "13:00";
                case 7:
                    return "14:00";
                case 8:
                    return "15:00";
                case 9:
                    return "16:00";
                case 10:
                    return "17:00";
                default:
                    return "00:00";
            }
        }

        public ActionResult Calendar()
        {
            int d_of_w = 0;
            string availability = "available";
            DateTime day1_old = DateTime.Today;
            DateTime day1 = DateTime.Today;
            if (day1_old.DayOfWeek == DayOfWeek.Saturday)
            {
                d_of_w += 2;
                day1 = DateTime.Today.AddDays(d_of_w);
            }
            if (day1_old.DayOfWeek == DayOfWeek.Sunday)
            {
                d_of_w += 1;
                day1 = DateTime.Today.AddDays(d_of_w);
            }
            if (day1_old.DayOfWeek == DayOfWeek.Friday)
            {
                d_of_w += 2;
            }
            DateTime day2 = DateTime.Today.AddDays(1 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Thursday)
            {
                d_of_w += 2;
            }
            DateTime day3 = DateTime.Today.AddDays(2 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Wednesday)
            {
                d_of_w += 2;
            }
            DateTime day4 = DateTime.Today.AddDays(3 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Tuesday)
            {
                d_of_w += 2;
            }
            DateTime day5 = DateTime.Today.AddDays(4 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Monday)
            {
                d_of_w += 2;
            }
            if (day1_old.DayOfWeek == DayOfWeek.Sunday || day1_old.DayOfWeek == DayOfWeek.Saturday)
            {
                d_of_w += 2;
            }
            DateTime day6 = DateTime.Today.AddDays(5 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Friday)
            {
                d_of_w += 2;
            }
            DateTime day7 = DateTime.Today.AddDays(6 + d_of_w);

            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM Rendezvous WHERE EmployeeId = " + ((Session["ID"] != null) ? (Session["ID"]) : (-1));
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                DateTime date = sdr.GetDateTime(1);
                byte isAvailable = sdr.GetByte(4);
                if (isAvailable == 0) availability = "appointed"; else availability = "unavailable";
                if (date.Date.Equals(day1.Date))
                {
                    ViewData["day1slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day2.Date))
                {
                    ViewData["day2slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day3.Date))
                {
                    ViewData["day3slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day4.Date))
                {
                    ViewData["day4slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day5.Date))
                {
                    ViewData["day5slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day6.Date))
                {
                    ViewData["day6slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day7.Date))
                {
                    ViewData["day7slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
            }
            cn.Close();

            ViewData["Day1"] = day1.ToString("dd-MM-yyyy");
            ViewData["Day2"] = day2.ToString("dd-MM-yyyy");
            ViewData["Day3"] = day3.ToString("dd-MM-yyyy");
            ViewData["Day4"] = day4.ToString("dd-MM-yyyy");
            ViewData["Day5"] = day5.ToString("dd-MM-yyyy");
            ViewData["Day6"] = day6.ToString("dd-MM-yyyy");
            ViewData["Day7"] = day7.ToString("dd-MM-yyyy");

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if (ViewData["day" + i + "slot" + j] == null) ViewData["day" + i + "slot" + j] = "<th name='datefield' class='available'>" + IntToStringConversion(j) + "</th>";
                }
            }

            return View();
        }

        // GET: ByDate
        public ActionResult ByDate()
        {
            if(Session["employee-id"] == null) Session["employee-id"] = 1;
            Debug.WriteLine(Session["employee-id"]);
            int d_of_w = 0;
            string availability = "available";
            DateTime day1_old = DateTime.Today;
            DateTime day1     = DateTime.Today;
            if (day1_old.DayOfWeek == DayOfWeek.Saturday)
            {
                d_of_w += 2;
                day1 = DateTime.Today.AddDays(d_of_w);
            }
            if (day1_old.DayOfWeek == DayOfWeek.Sunday)
            {
                d_of_w += 1;
                day1 = DateTime.Today.AddDays(d_of_w);
            }
            if (day1_old.DayOfWeek == DayOfWeek.Friday)
            {
                d_of_w += 2;
            }
            DateTime day2 = DateTime.Today.AddDays(1 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Thursday)
            {
                d_of_w += 2;
            }
            DateTime day3 = DateTime.Today.AddDays(2 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Wednesday)
            {
                d_of_w += 2;
            }
            DateTime day4 = DateTime.Today.AddDays(3 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Tuesday)
            {
                d_of_w += 2;
            }
            DateTime day5 = DateTime.Today.AddDays(4 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Monday)
            {
                d_of_w += 2;
            }
            if (day1_old.DayOfWeek == DayOfWeek.Sunday || day1_old.DayOfWeek == DayOfWeek.Saturday)
            {
                d_of_w += 2;
            }
            DateTime day6 = DateTime.Today.AddDays(5 + d_of_w);
            if (day1_old.DayOfWeek == DayOfWeek.Friday)
            {
                d_of_w += 2;
            }
            DateTime day7 = DateTime.Today.AddDays(6 + d_of_w);
            
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM Rendezvous WHERE EmployeeId = " + Session["employee-id"];
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                DateTime date = sdr.GetDateTime(1);
                byte isAvailable = sdr.GetByte(4);
                if (isAvailable == 0) availability = "appointed"; else availability = "unavailable";
                if (date.Date.Equals(day1.Date))
                {
                    ViewData["day1slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day2.Date))
                {
                    ViewData["day2slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day3.Date))
                {
                    ViewData["day3slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day4.Date))
                {
                    ViewData["day4slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day5.Date))
                {
                    ViewData["day5slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day6.Date))
                {
                    ViewData["day6slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
                if (date.Date.Equals(day7.Date))
                {
                    ViewData["day7slot" + TimeToIntConversion(date)] = "<th class='" + availability + "'>" + date.ToString("HH:mm") + "</th>";
                }
            }
            cn.Close();

            ViewData["Day1"] = day1.ToString("dd-MM-yyyy");
            ViewData["Day2"] = day2.ToString("dd-MM-yyyy");
            ViewData["Day3"] = day3.ToString("dd-MM-yyyy");
            ViewData["Day4"] = day4.ToString("dd-MM-yyyy");
            ViewData["Day5"] = day5.ToString("dd-MM-yyyy");
            ViewData["Day6"] = day6.ToString("dd-MM-yyyy");
            ViewData["Day7"] = day7.ToString("dd-MM-yyyy");

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if(ViewData["day" + i + "slot" + j] == null) ViewData["day" + i + "slot" + j] = "<th name='datefield' class='available'>" + IntToStringConversion(j) + "</th>";
                }
            }

            _sql = "SELECT * FROM Employees";
            cn.Open();
            cmd = new SqlCommand(_sql, cn);
            sdr = cmd.ExecuteReader();

            ViewData["employees-list"] = "";
            while (sdr.Read())
            {
                ViewData["employees-list"] += "<li name='employee-element'>" + sdr.GetString(1) + " " + sdr.GetString(2) + "</li>";
            }
            cn.Close();

            _sql = "SELECT * FROM Employees WHERE employee_id = " + Session["employee-id"];
            cn.Open();
            cmd = new SqlCommand(_sql, cn);
            sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                ViewData["employee-name"] += sdr.GetString(1) + " " + sdr.GetString(2);
            }
            cn.Close();

            return View();
        }

   
        public ActionResult Index()
        {
            return RedirectToAction("ByDate");
        }
    }
}