using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WPHW3.Models
{
    public class AccountDbInitializer: DropCreateDatabaseAlways<AccountDBContext>
    {
        protected override void Seed(AccountDBContext context)
        {
            context.Accounts.Add(new Account() { Name="admin", Password="admin", IsLocked=true });
            base.Seed(context);
        }
    }
}