using RestERP.Core.Doman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestERP.Infrastructure.Data.SeedData
{
    public class FoodCategorySeedData
    {
        public static List<FoodCategory> GetFoodCategories()
        {
            return new List<FoodCategory>
            {
                new FoodCategory
                {
                    Id = 1,
                    Name = "Breakfast",
                    TurkishName = "Kahvaltı",
                },
                new FoodCategory
                {
                    Id = 2,
                    Name = "OliveOilDishes",
                    TurkishName = "Zeytinyağlılar",
                },
                 new FoodCategory
                {
                    Id = 3,
                    Name = "HotAppetizers",
                    TurkishName = "Ara Sıcaklar",
                },
                new FoodCategory
                {
                    Id = 4,
                    Name = "Types of Fish",
                    TurkishName = "Balık Çeşitleri",
                },
                new FoodCategory
                {
                    Id = 5,
                    Name = "SeasonalFish",
                    TurkishName = "Mevsim Balıkları",
                },
                new FoodCategory
                {
                    Id = 6,
                    Name = "TypesofGrillDishes",
                    TurkishName = "Izgara ve Kavurmalar",
                },
                new FoodCategory
                {
                    Id = 7,
                    Name = "Dessert",
                    TurkishName = "Tatlılar",
                },
                new FoodCategory
                {
                    Id = 8,
                    Name = "Drinks",
                    TurkishName = "İçecekler",
                }
            };
        }
    }
}
