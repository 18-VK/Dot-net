using System;
using EFCorePractice.Models;

namespace EFCorePractice
{
    class EFpractice
    {
        public static void Main(string[] args)
        {
            //CURD operation 
            using(var DB = new EFContext())
            {
                //Insert 
                Product ObjProd = new Product() { Name = "CMF Buds 2", Price = 1799.00M};
                DB.Products.Add(ObjProd);

                DB.SaveChanges();

                //Read 
                List<Product> ProductsList = DB.Products.ToList();

                //Update  
                Product? Prod1 = DB.Products.Where(P => P.Name == "CMF Buds 2").FirstOrDefault();
                if (Prod1 != null)
                {
                    Prod1.Price = 1899.00M;
                    DB.SaveChanges();
                }
                // Delete 
                // Prod1 
                DB.Products.Remove(Prod1);
                DB.SaveChanges();

                ProductsList = DB.Products.ToList();

            }

        }
    }
}