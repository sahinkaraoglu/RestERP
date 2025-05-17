using RestERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestERP.Infrastructure.Data.SeedData
{
    public class CategorySeedData
    {
        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Breakfast",
                    TurkishName = "Kahvaltı",
                },
                new Category
                {
                    Id = 2,
                    Name = "OliveOilDishes",
                    TurkishName = "Zeytinyağlılar",
                },
                 new Category
                {
                    Id = 3,
                    Name = "HotAppetizers",
                    TurkishName = "AraSıcaklar",
                },
                new Category
                {
                    Id = 4,
                    Name = "Types of Fish",
                    TurkishName = "Balık Çeşitleri",
                },
                new Category
                {
                    Id = 5,
                    Name = "SeasonalFish",
                    TurkishName = "Mevsim Balıkları",
                },
                new Category
                {
                    Id = 6,
                    Name = "TypesofGrillDishes",
                    TurkishName = "Izgara ve Kavurmalar",
                },
                new Category
                {
                    Id = 7,
                    Name = "Dessert",
                    TurkishName = "Tatlılar",
                },
                new Category
                {
                    Id = 8,
                    Name = "Drinks",
                    TurkishName = "İçecekler",
                }
            };
        }
    }
}
