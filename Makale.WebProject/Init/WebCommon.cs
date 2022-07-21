using Makale.Common;
using Makale.Entities;
using Makale.WebProject.Models;
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
            User user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}