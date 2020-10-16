using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKuafor.Models
{
    public class CustomerModel
    {
        public int ID {get; set; }

        [Required(ErrorMessage = "İsim giriniz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim giriniz.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı giriniz.")]
        [Remote("IsUserExists", "Register", ErrorMessage = "Bu isim zaten kullanılıyor.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email giriniz.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                           ErrorMessage = "Email adresi geçersiz.")]
        [Remote("IsEmailExists", "Register", ErrorMessage = "Bu email zaten kullanılıyor.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre giriniz.")]
        [RegularExpression(@"([a-zA-Z0-9_\-\.]){8,15}",
                           ErrorMessage = "Şifre çok kısa, uzun ya da geçersiz.")]
        public string Password { get; set; }

        public bool isAdmin = false;

        public DateTime RegisterDate { get; set; }
    }
}