using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LoremNET;

namespace Exam1.Models
{
    public class AccountDbInitializer: DropCreateDatabaseAlways<AccountDBContext>
    {
        protected override void Seed(AccountDBContext context)
        {
            List<Category> categories = new List<Category> {
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

            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                string name = Lorem.Words(3);
                string description = String.Join(",",Lorem.Paragraphs(10, 3, 3).ToArray());
                string category = categories.ElementAt(random.Next(0,11)).Name;
                ProductInfo productInfo = new ProductInfo { Name=name, Price=random.Next(1000), Description = description, TotalAmount = random.Next(1000), Category=category};
                context.ProductInfos.Add(productInfo);

                account.ProductsToSell.Add(productInfo);
                productInfo.Seller = account;
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}