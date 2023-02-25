using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using WPHW3.Models;

namespace WPHW3.Util
{
    public class NinjectRegistrations:NinjectModule
    {
        public override void Load()
        {
            Bind<IAccountRepo>().To<AccountRepo>();
        }
    }
}