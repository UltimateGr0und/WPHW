using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exam1.Models
{
    public class AccountDbInitializer: DropCreateDatabaseAlways<AccountDBContext>
    {
        protected override void Seed(AccountDBContext context)
        {
            Account admin = new Account() { Username = "admin", Password="admin1",AccountType = AccountType.Admin };
            context.Accounts.Add(admin);
            
            ProductInfo productInfo = new ProductInfo { Name="Product", Price=5.95, Description = "blablablablablabla", TotalAmount = 99};
            context.ProductInfos.Add(productInfo);

            Account account = new Account() { Username = "customer", Password = "customer1", AccountType = AccountType.Customer };
            context.Accounts.Add(account);

            account.ProductsToSell.Add(productInfo);
            productInfo.Seller = account;

            context.SaveChanges();

            base.Seed(context);
        }
    }
}