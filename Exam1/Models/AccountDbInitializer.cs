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
            List<Category> categories = new List<Category> { new Category("all"),
                new Category("gadjets"),
                new Category("electronic"),
                new Category("computers"),
                new Category("furniture"),
                new Category("cosmetic"),
                new Category("sport"),
                new Category("auto"),
                new Category("hobbies"),
                new Category("accesories"),
                new Category("clothes"),
                new Category("other")};
            context.Categories.AddRange(categories);

            Account admin = new Account() { Username = "admin", Password="admin1",AccountType = AccountType.Admin };
            context.Accounts.Add(admin);

            Account account = new Account() { Username = "customer", Password = "customer1", AccountType = AccountType.Customer };
            context.Accounts.Add(account);
            for (int i = 0; i < 15; i++)
            {
                ProductInfo productInfo = new ProductInfo { Name="Product"+i.ToString(), Price=5.95, Description = "blablablablablabla", TotalAmount = 99, Category="other"};
                context.ProductInfos.Add(productInfo);

                account.ProductsToSell.Add(productInfo);
                productInfo.Seller = account;
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}