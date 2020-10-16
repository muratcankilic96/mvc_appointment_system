using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKuafor.Models
{
    public class CustomerLoginModel
    {

        [Required(ErrorMessage = "Kullanıcı adı giriniz.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre giriniz.")]
        public string Password { get; set; }

        public int ID { get; set; }

        public byte isAdmin { get; set; }

        public bool CheckValidity(string _usn, string _pwd)
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM CustomerModels WHERE Username = '" + _usn + "' AND Password = '" + _pwd + "'";
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            if (sdr.Read()) {
                ID = sdr.GetInt32(0);
                isAdmin = sdr.GetByte(7);
                cn.Close();
            return true; }
            else { 
             cn.Close();
            return false;
            }
        }
        public bool CheckEmployeeValidity(string _usn, string _pwd)
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=MVCKuaforContext-20180723110834; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCKuaforContext-20180723110834.mdf");
            string _sql = "SELECT * FROM Employees WHERE Username = '" + _usn + "' AND Password = '" + _pwd + "'";
            cn.Open();
            SqlCommand cmd = new SqlCommand(_sql, cn);
            SqlDataReader sdr = cmd.ExecuteReader();

            if (sdr.Read())
            {
                ID = sdr.GetInt32(0);
                isAdmin = 0;
                cn.Close();
                return true;
            }
            else
            {
                cn.Close();
                return false;
            }
        }
    }
}