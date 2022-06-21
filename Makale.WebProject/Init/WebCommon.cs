using Makale.Common;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makale.WebProject.Init
{
    public class WebCommon : ICommon
    {


        public string GetCurrentUserName()
        {
           if(HttpContext.Current.Session["login"] !=null)
            {
                User user = HttpContext.Current.Session["login"] as User;
                return user.Username;
            }

            return null;
        }
    }
}