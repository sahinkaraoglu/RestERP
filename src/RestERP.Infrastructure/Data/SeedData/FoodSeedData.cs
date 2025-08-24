using RestERP.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestERP.Infrastructure.Data.SeedData
{
    public class FoodSeedData
    {
        public static List<Food> GetFood()
        {
            return new List<Food>
            {
                new Food
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Turkish Breakfast Spread",
                    TurkishName = "Kahvaltı Tabağı",
                    Price = 200m,
                    Description = "Çeşitli peynirler, zeytinler, domatesler, salatalıklar ve ekmeklerle servis edilen geleneksel bir Türk kahvaltısı."
                },
                new Food
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Mixed Breakfast (Per Person)",
                    TurkishName = "Serpme Kahvaltı",
                    Price = 300m,
                    Description = "Kişi başı serpme usulü sunulan zengin kahvaltı tabağı. Çeşitli peynirler, reçeller, bal, kaymak ve diğer kahvaltılıklar içerir."
                },
                new Food
                {
                    Id = 3,
                    CategoryId = 1,
                    Name = "Cheese Platter",
                    TurkishName = "Peynir Tabağı",
                    Price = 70m,
                    Description = "Çeşitli yerel ve ithal peynirlerden oluşan seçki. Taze ekmek ile servis edilir."
                },
                new Food
                {
                    Id = 4,
                    CategoryId = 1,
                    Name = "Olive Platter",
                    TurkishName = "Zeytin Tabağı",
                    Price = 47.50m,
                    Description = "Farklı yörelerden toplanmış yeşil ve siyah zeytinler."
                },
                new Food
                {
                    Id = 5,
                    CategoryId = 1,
                    Name = "Omelette",
                    TurkishName = "Omlet",
                    Price = 47.50m,
                    Description = "Taze yumurtalardan hazırlanan sade omlet."
                },
                new Food
                {
                    Id = 6,
                    CategoryId = 1,
                    Name = "Omelette with Gorgonzola Cheese",
                    TurkishName = "Kaşarlı Omlet",
                    Price = 55m,
                    Description = "Kaşar peyniri ile hazırlanmış lezzetli omlet."
                },
                new Food
                {
                    Id = 7,
                    CategoryId = 1,
                    Name = "Mixed Omelette",
                    TurkishName = "Karışık Omlet",
                    Price = 80m,
                    Description = "Peynir, domates, biber ve diğer malzemelerle hazırlanmış karışık omlet."
                },
                new Food
                {
                    Id = 8,
                    CategoryId = 1,
                    Name = "Scrambled Eggs with Tomatoes and Green Pepper",
                    TurkishName = "Menemen",
                    Price = 62.50m,
                    Description = "Domates, yeşil biber ve yumurta ile hazırlanan geleneksel Türk kahvaltı yemeği."
                },
                new Food
                {
                    Id = 9,
                    CategoryId = 1,
                    Name = "Scrambled Eggs with Garlic Sausages",
                    TurkishName = "Sucuklu Yumurta",
                    Price = 80m,
                    Description = "Kızarmış sucuk parçaları ile pişirilmiş yumurta."
                },
                new Food
                {
                    Id = 10,
                    CategoryId = 1,
                    Name = "Melted Cheese and Cornmeal",
                    TurkishName = "Mıhlama",
                    Price = 100m,
                    Description = "Karadeniz bölgesine özgü, mısır unu ve eritilmiş peynir ile hazırlanan geleneksel yemek."
                },
                new Food
                {
                    Id = 11,
                    CategoryId = 1,
                    Name = "Scrambled Eggs with Pastrami",
                    TurkishName = "Pastırmalı Yumurta",
                    Price = 75m,
                    Description = "İnce dilimlenmiş pastırma ile pişirilmiş yumurta."
                },
                new Food
                {
                    Id = 12,
                    CategoryId = 1,
                    Name = "Fried Garlic Sausage",
                    TurkishName = "Sahanda Sucuk",
                    Price = 85m,
                    Description = "Sahanda kızartılmış dilimlenmiş sucuk."
                },
                new Food
                {
                    Id = 13,
                    CategoryId = 1,
                    Name = "French Fries",
                    TurkishName = "Patates Tava",
                    Price = 40m,
                    Description = "Kızarmış taze patates."
                },
                new Food
                {
                    Id = 14,
                    CategoryId = 1,
                    Name = "Rolled Pastry",
                    TurkishName = "Mini Kalem Böreği",
                    Price = 40m,
                    Description = "İçi peynirli mini kalem börekleri."
                },
                new Food
                {
                    Id = 15,
                    CategoryId = 1,
                    Name = "Honey & Clotted Cream",
                    TurkishName = "Bal & Kaymak",
                    Price = 60m,
                    Description = "Taze kaymak ve organik bal ile servis edilir."
                },
                new Food
                {
                    Id = 16,
                    CategoryId = 1,
                    Name = "A Serving of Butter",
                    TurkishName = "Tereyağı Porsiyonu",
                    Price = 47.50m,
                    Description = "Taze tereyağı porsiyonu."
                },
                new Food
                {
                    Id = 17,
                    CategoryId = 1,
                    Name = "Tomato and Cucumber",
                    TurkishName = "Söğüş Tabağı",
                    Price = 47.50m,
                    Description = "Doğranmış domates, salatalık ve yeşilliklerden oluşan söğüş tabağı."
                },
                new Food
                {
                    Id = 18,
                    CategoryId = 1,
                    Name = "Fruit Platter",
                    TurkishName = "Meyve Tabağı",
                    Price = 55m,
                    Description = "Mevsim meyvelerinden oluşan tabak."
                },
                new Food
                {
                    Id = 19,
                    CategoryId = 2,
                    Name = "Stuffed Peppers",
                    TurkishName = "Biber Dolma",
                    Price = 57.50m,
                    Description = "Pirinç ve baharatlarla doldurulmuş taze biberler."
                },
                new Food
                {
                    Id = 20,
                    CategoryId = 2,
                    Name = "Sauced Aubergine",
                    TurkishName = "Soslu Patlıcan",
                    Price = 57.50m,
                    Description = "Özel sos ile hazırlanmış patlıcan yemeği."
                },
                new Food
                {
                    Id = 21,
                    CategoryId = 2,
                    Name = "Artichoke",
                    TurkishName = "Enginar",
                    Price = 57.50m,
                    Description = "Zeytinyağlı enginar yemeği."
                },
                new Food
                {
                    Id = 22,
                    CategoryId = 2,
                    Name = "Green Beans",
                    TurkishName = "Taze Fasulye",
                    Price = 57.50m,
                    Description = "Zeytinyağlı taze fasulye yemeği."
                },
                new Food
                {
                    Id = 23,
                    CategoryId = 2,
                    Name = "Stuffed Vine Leaves",
                    TurkishName = "Yaprak Sarma",
                    Price = 57.50m,
                    Description = "Pirinç ve baharatlarla doldurulmuş asma yaprağı sarması."
                },
                new Food
                {
                    Id = 24,
                    CategoryId = 2,
                    Name = "Assorted Olive Oil Dish Platter",
                    TurkishName = "Zeytinyağı Tabağı",
                    Price = 80m,
                    Description = "Çeşitli zeytinyağlı yemeklerden oluşan tabak."
                },
                new Food
                {
                    Id = 25,
                    CategoryId = 3,
                    Name = "Fresh Fries",
                    TurkishName = "Patates Tava",
                    Price = 40m,
                    Description = "Taze patateslerden hazırlanmış kızartma."
                },
                new Food
                {
                    Id = 26,
                    CategoryId = 3,
                    Name = "Pastrami Pastry",
                    TurkishName = "Paçanga Böreği",
                    Price = 67.50m,
                    Description = "İçi pastırma ile doldurulmuş çıtır börek."
                },
                new Food
                {
                    Id = 27,
                    CategoryId = 3,
                    Name = "Mushroom Gratin",
                    TurkishName = "Mantar Graten",
                    Price = 75m,
                    Description = "Fırında kaşar peyniri ile gratine edilmiş mantar."
                },
                new Food
                {
                    Id = 28,
                    CategoryId = 3,
                    Name = "Fried Mushrooms",
                    TurkishName = "Mantar Kavurma",
                    Price = 55m,
                    Description = "Tereyağında kavrulmuş taze mantarlar."
                },
                new Food
                {
                    Id = 29,
                    CategoryId = 3,
                    Name = "Julienne Sole Fish",
                    TurkishName = "Julyen Dil Balığı",
                    Price = 210m,
                    Description = "İnce doğranmış dil balığından hazırlanan özel sote."
                },
                new Food
                {
                    Id = 30,
                    CategoryId = 4,
                    Name = "Grilled Sea Bream 550 - 650 gr.",
                    TurkishName = "Çipura Izgara 550 - 650 gr.",
                    Price = 550m,
                    Description = "550-650 gr ağırlığında ızgara çipura balığı."
                },
                new Food
                {
                    Id = 31,
                    CategoryId = 4,
                    Name = "Grilled Sea Bream 300 - 350 gr.",
                    TurkishName = "Çipura Izgara 300 - 350 gr.",
                    Price = 310m,
                    Description = "300-350 gr ağırlığında ızgara çipura balığı."
                },
                new Food
                {
                    Id = 32,
                    CategoryId = 4,
                    Name = "Fried Whiting Fish",
                    TurkishName = "Mezgit Tava",
                    Price = 260m,
                    Description = "Un ile kaplanıp kızartılmış mezgit balığı."
                },
                new Food
                {
                    Id = 33,
                    CategoryId = 4,
                    Name = "Pan Seared Salmon",
                    TurkishName = "Fesleğen Soslu Somon Izgara",
                    Price = 415m,
                    Description = "Fesleğen sosu ile servis edilen ızgara somon balığı."
                },
                new Food
                {
                    Id = 34,
                    CategoryId = 4,
                    Name = "Sea Bass Filleted",
                    TurkishName = "Levrek Fileto",
                    Price = 400m,
                    Description = "Filetolanmış levrek balığı."
                },
                new Food
                {
                    Id = 35,
                    CategoryId = 4,
                    Name = "Grilled Sea Bass 550 - 650 gr.",
                    TurkishName = "Levrek Izgara 550 - 650 gr.",
                    Price = 550m,
                    Description = "550-650 gr ağırlığında ızgara levrek balığı."
                },
                new Food
                {
                    Id = 36,
                    CategoryId = 4,
                    Name = "Grilled Sea Bass 300 - 350 gr.",
                    TurkishName = "Levrek Izgara 300 - 350 gr.",
                    Price = 300m,
                    Description = "300-350 gr ağırlığında ızgara levrek balığı."
                },
                new Food
                {
                    Id = 37,
                    CategoryId = 4,
                    Name = "Sea Bass on the Clay Tile",
                    TurkishName = "Kiremitte Levrek",
                    Price = 415m,
                    Description = "Kiremit üzerinde özel soslar ile pişirilmiş levrek balığı."
                },
                new Food
                {
                    Id = 38,
                    CategoryId = 4,
                    Name = "Broiled Salmon Steak",
                    TurkishName = "Somon Izgara",
                    Price = 400m,
                    Description = "Taze ızgara somon balığı."
                },
                new Food
                {
                    Id = 39,
                    CategoryId = 4,
                    Name = "Grilled Sole Fish",
                    TurkishName = "Dil Balığı Izgara",
                    Price = 270m,
                    Description = "Izgara edilmiş dil balığı."
                },
                new Food
                {
                    Id = 40,
                    CategoryId = 4,
                    Name = "Baked Salmon",
                    TurkishName = "Somon Kavurma",
                    Price = 415m,
                    Description = "Özel soslar ile kavrulmuş somon balığı."
                },
                new Food
                {
                    Id = 41,
                    CategoryId = 5,
                    Name = "Horse Mackerel",
                    TurkishName = "İstavrit",
                    Price = 285m,
                    Description = "Mevsiminde taze istavrit balığı."
                },
                new Food
                {
                    Id = 42,
                    CategoryId = 5,
                    Name = "Blue Fish",
                    TurkishName = "Sarıkanat",
                    Price = 295m,
                    Description = "Taze sarıkanat balığı."
                },
                new Food
                {
                    Id = 43,
                    CategoryId = 5,
                    Name = "Bonito",
                    TurkishName = "Palamut",
                    Price = 340m,
                    Description = "Mevsiminde taze palamut balığı."
                },
                new Food
                {
                    Id = 44,
                    CategoryId = 5,
                    Name = "Fried Anchovies",
                    TurkishName = "Hamsi Tava",
                    Price = 250m,
                    Description = "Mısır unu ile kaplanıp kızartılmış hamsi balığı."
                },
                new Food
                {
                    Id = 45,
                    CategoryId = 5,
                    Name = "Whiting Fish",
                    TurkishName = "Mezgit",
                    Price = 270m,
                    Description = "Taze mezgit balığı."
                },
                new Food
                {
                    Id = 46,
                    CategoryId = 5,
                    Name = "Red Mullet",
                    TurkishName = "Tekir",
                    Price = 290m,
                    Description = "Taze tekir balığı."
                },
                new Food
                {
                    Id = 47,
                    CategoryId = 5,
                    Name = "Loufer",
                    TurkishName = "Lüfer",
                    Price = 375m,
                    Description = "Mevsiminde taze lüfer balığı."
                },
                new Food
                {
                    Id = 48,
                    CategoryId = 6,
                    Name = "Grilled Köfte",
                    TurkishName = "Köfte Izgara",
                    Price = 200m,
                    Description = "Özel baharatlarla hazırlanmış ızgara köfte."
                },
                new Food
                {
                    Id = 49,
                    CategoryId = 6,
                    Name = "Grilled Köfte with Cheese",
                    TurkishName = "Kaşarlı Köfte Izgara",
                    Price = 210m,
                    Description = "Kaşar peyniri ile servis edilen ızgara köfte."
                },
                new Food
                {
                    Id = 50,
                    CategoryId = 6,
                    Name = "Chicken Shish",
                    TurkishName = "Piliç Izgara",
                    Price = 130m,
                    Description = "Özel marine edilmiş tavuk şiş."
                },
                new Food
                {
                    Id = 51,
                    CategoryId = 6,
                    Name = "Roasting Chicken",
                    TurkishName = "Piliç Kavurma",
                    Price = 145m,
                    Description = "Özel baharatlarla kavurulmuş tavuk parçaları."
                },
                new Food
                {
                    Id = 52,
                    CategoryId = 6,
                    Name = "Chicken Julienne with Eggplant Purée",
                    TurkishName = "Beğendili Julyen Piliç",
                    Price = 145m,
                    Description = "Közlenmiş patlıcan ezmesi üzerinde julyen doğranmış tavuk parçaları."
                },
                new Food
                {
                    Id = 53,
                    CategoryId = 6,
                    Name = "Veal Fillet",
                    TurkishName = "Bonfile",
                    Price = 390m,
                    Description = "Tercihe göre pişirilmiş dana bonfile."
                },
                new Food
                {
                    Id = 54,
                    CategoryId = 6,
                    Name = "Veal Fillet with Turkish Cheese",
                    TurkishName = "Kaşarlı Bonfile",
                    Price = 400m,
                    Description = "Kaşar peyniri ile servis edilen dana bonfile."
                },
                new Food
                {
                    Id = 55,
                    CategoryId = 6,
                    Name = "Steak Fillet Julienne with Eggplant Purée",
                    TurkishName = "Beğendili Julyen Bonfile",
                    Price = 400m,
                    Description = "Közlenmiş patlıcan ezmesi üzerinde julyen doğranmış dana bonfile."
                },
                new Food
                {
                    Id = 56,
                    CategoryId = 6,
                    Name = "Mixed Grill",
                    TurkishName = "Karışık Izgara",
                    Price = 390m,
                    Description = "Köfte, pirzola, tavuk ve diğer etlerin ızgara karışımı."
                },
                new Food
                {
                    Id = 57,
                    CategoryId = 6,
                    Name = "Shepherd's Roasting",
                    TurkishName = "Çoban Kavurma",
                    Price = 325m,
                    Description = "Sebzeler ile hazırlanmış özel kavurma."
                },
                new Food
                {
                    Id = 58,
                    CategoryId = 7,
                    Name = "Sütlaç",
                    TurkishName = "Sütlaç",
                    Price = 55m,
                    Description = "Geleneksel fırında pişirilmiş sütlaç."
                },
                new Food
                {
                    Id = 59,
                    CategoryId = 7,
                    Name = "Tres Leches Cake",
                    TurkishName = "Trileçe",
                    Price = 55m,
                    Description = "Üç farklı süt ile hazırlanmış özel tatlı."
                },
                new Food
                {
                    Id = 60,
                    CategoryId = 7,
                    Name = "Noah's Pudding",
                    TurkishName = "Aşure",
                    Price = 55m,
                    Description = "Geleneksel aşure tatlısı."
                },
                new Food
                {
                    Id = 61,
                    CategoryId = 7,
                    Name = "Profiterole",
                    TurkishName = "Profiterol",
                    Price = 55m,
                    Description = "Çikolata sosu ile servis edilen profiterol."
                },
                new Food
                {
                    Id = 62,
                    CategoryId = 7,
                    Name = "Volcanic",
                    TurkishName = "Volkanik",
                    Price = 80m,
                    Description = "Sıcak çikolata sosu ile servis edilen özel tatlı."
                },
                new Food
                {
                    Id = 63,
                    CategoryId = 7,
                    Name = "Fruit Dessert",
                    TurkishName = "Meyveli Tatlılar",
                    Price = 60m,
                    Description = "Mevsim meyveleriyle hazırlanmış tatlı çeşitleri."
                },
                new Food
                {
                    Id = 64,
                    CategoryId = 7,
                    Name = "Pastries",
                    TurkishName = "Hamur İşi Tatlıları",
                    Price = 60m,
                    Description = "Geleneksel hamur işi tatlılar."
                },
                new Food
                {
                    Id = 65,
                    CategoryId = 7,
                    Name = "Ice Cream",
                    TurkishName = "Dondurma Porsiyon",
                    Price = 47.50m,
                    Description = "Çeşitli aromalarda dondurma porsiyonu."
                },
                new Food
                {
                    Id = 66,
                    CategoryId = 7,
                    Name = "Mixed Fruits",
                    TurkishName = "Meyve Tabağı",
                    Price = 55m,
                    Description = "Mevsim meyvelerinden hazırlanmış meyve tabağı."
                },
                new Food
                {
                    Id = 67,
                    CategoryId = 7,
                    Name = "Tahini Bread Roll",
                    TurkishName = "Tahinli Sarma",
                    Price = 67.50m,
                    Description = "Tahin ile hazırlanmış geleneksel tatlı."
                },
                new Food
                {
                    Id = 68,
                    CategoryId = 8,
                    Name = "Canned Soft Drinks",
                    TurkishName = "Meşrubat Çeşitleri",
                    Price = 55m,
                    Description = "Kutu içecek çeşitleri."
                },
                new Food
                {
                    Id = 69,
                    CategoryId = 8,
                    Name = "Ayran",
                    TurkishName = "Ayran",
                    Price = 16m,
                    Description = "Geleneksel yoğurt içeceği."
                },
                new Food
                {
                    Id = 70,
                    CategoryId = 8,
                    Name = "Mineral Water",
                    TurkishName = "Soda",
                    Price = 16m,
                    Description = "Maden suyu."
                },
                new Food
                {
                    Id = 71,
                    CategoryId = 8,
                    Name = "Frute Mineral Water",
                    TurkishName = "Meyveli Soda",
                    Price = 19m,
                    Description = "Çeşitli aromalarda meyveli maden suyu."
                },
                new Food
                {
                    Id = 72,
                    CategoryId = 8,
                    Name = "Orange / Pomegranate Juice",
                    TurkishName = "Sıkma Portakal / Nar Suyu",
                    Price = 55m,
                    Description = "Taze sıkılmış portakal veya nar suyu."
                },
                new Food
                {
                    Id = 73,
                    CategoryId = 8,
                    Name = "Nar Çiçeği Şerbeti",
                    TurkishName = "Nar Çiçeği Şerbeti",
                    Price = 55m,
                    Description = "Geleneksel nar çiçeği şerbeti."
                },
                new Food
                {
                    Id = 74,
                    CategoryId = 8,
                    Name = "Tea",
                    TurkishName = "Çay",
                    Price = 10m,
                    Description = "Demli Türk çayı."
                },
                new Food
                {
                    Id = 75,
                    CategoryId = 8,
                    Name = "Instant Coffee",
                    TurkishName = "Hazır Kahve",
                    Price = 40m,
                    Description = "Nescafe, sade veya sütlü olarak servis edilir."
                },
                new Food
                {
                    Id = 76,
                    CategoryId = 8,
                    Name = "Turkish Coffee",
                    TurkishName = "Türk Kahvesi",
                    Price = 40m,
                    Description = "Geleneksel köpüklü Türk kahvesi."
                },
                new Food
                {
                    Id = 77,
                    CategoryId = 8,
                    Name = "Filter Coffee",
                    TurkishName = "Filtre Kahve",
                    Price = 47.50m,
                    Description = "Taze çekilmiş kahve çekirdeklerinden hazırlanmış filtre kahve."
                },
                new Food
                {
                    Id = 78,
                    CategoryId = 8,
                    Name = "Water",
                    TurkishName = "Su",
                    Price = 8m,
                    Description = "0.5L içme suyu."
                }
            };
        }
    }
}
