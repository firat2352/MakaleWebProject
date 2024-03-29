﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Makale.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalı")]
        public string Username { get; set; }
        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password), StringLength(70, ErrorMessage = "{0} alanı max {1} karakter olmalı"), EmailAddress(ErrorMessage ="{0} alanı için geçerli e-posta adresi giriniz.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalı")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalı"),Compare("Password",ErrorMessage ="{0} ile {1} uyuşmuyor")]
        public string Repassword { get; set; }
    }
}