﻿using Makale.DataAccessLayer.EF;
using Makale.Entities;
using Makale.Entities.Messages;
using Makale.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class NoteUserManager
    {
        private Repository<User> repo_user = new Repository<User>();
        public BusinessLayerResult<User> RegisterUser(RegisterViewModel data)
        {
           User user= repo_user.Find(x => x.Username == data.Username || x.Email==data.Email);
           BusinessLayerResult<User> res = new BusinessLayerResult<User>();


            if (user!=null)
            {
                 if(user.Username == data.Username)
                {
                    res.AddError(ErrorMessagesCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }


                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessagesCode.EmailAlreadyExists, "Email Kayıtlı");
                }
            }

            else
            {
                int db_result=repo_user.Insert(new User()
                {
                    Username = data.Username,
                    Email=data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false,

                }); 

                if(db_result>0)
                {
                    res.Result = repo_user.Find(x => x.Email == data.Email && x.Username == data.Username);
                }
            }

            return res;
        }

        public BusinessLayerResult<User> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<User> res = new BusinessLayerResult<User>();
            res.Result= repo_user.Find(x => x.Username == data.Username &&  x.Password == data.Password);

            
          

            if (res.Result != null)
            {
                if(!res.Result.IsActive)
                {
                    res.AddError(ErrorMessagesCode.UserIsNotActive, "Kullanıcı Aktif Değil");
                    res.AddError(ErrorMessagesCode.CheckYourEmail, "Lütfen E-postanızı Kontrol Ediniz");
                }
               
            }
            else
            {
                res.AddError(ErrorMessagesCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor");
            }

            return res;
        }
    }
}