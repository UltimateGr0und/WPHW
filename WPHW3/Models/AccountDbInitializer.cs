﻿using System;
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
            User user = new User() { FullName="Admins name" };
            Account account = new Account() { Name = "admin", Password="admin",AccountType = AccountType.Admin };
            context.Accounts.Add(account);
            context.Users.Add(user);
            account.User = user;
            context.SaveChanges();
            base.Seed(context);
        }
    }
}