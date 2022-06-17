using Makale.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _lockTest = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }


        private static void CreateContext()
        {
             if(context == null)
             {
                lock(_lockTest)
                {
                    if (context == null) {

                        context = new DatabaseContext();
                    }
                }
             }
        }
    }
}
