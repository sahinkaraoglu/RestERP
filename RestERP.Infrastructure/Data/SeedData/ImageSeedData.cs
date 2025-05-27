using RestERP.Core.Doman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestERP.Infrastructure.Data.SeedData
{
    public class ImageSeedData
    {
        public static List<Image> GetImages()
        {
            return new List<Image>
            {
                // Kahvaltı kategori görüntüleri
                new Image { Id = 1, FoodId = 1, Path = "/img/Food/Breakfast/KahvaltiTabagi.jpg" },
                new Image { Id = 2, FoodId = 2, Path = "/img/Food/Breakfast/SerpmeKahvalti.jpg" },
                new Image { Id = 3, FoodId = 3, Path = "/img/Food/Breakfast/PeynirTabagi.jpeg" },
                new Image { Id = 4, FoodId = 4, Path = "/img/Food/Breakfast/ZeytinTabagi.jpeg" },
                new Image { Id = 5, FoodId = 5, Path = "/img/Food/Breakfast/Omlet.jpg" },
                new Image { Id = 6, FoodId = 6, Path = "/img/Food/Breakfast/KasarliOmlet.jpg" },
                new Image { Id = 7, FoodId = 7, Path = "/img/Food/Breakfast/KarisikOmlet.jpg" },
                new Image { Id = 8, FoodId = 8, Path = "/img/Food/Breakfast/Menemen.jpg" },
                new Image { Id = 9, FoodId = 9, Path = "/img/Food/Breakfast/SucukluYumurta.jpeg" },
                new Image { Id = 10, FoodId = 10, Path = "/img/Food/Breakfast/Mihlama.jpg" },
                new Image { Id = 11, FoodId = 11, Path = "/img/Food/Breakfast/PastirmaliYumurta.jpg" },
                new Image { Id = 12, FoodId = 12, Path = "/img/Food/Breakfast/SahandaSucuk.jpg" },
                new Image { Id = 13, FoodId = 13, Path = "/img/Food/Breakfast/PatatesTava.jpg" },
                new Image { Id = 14, FoodId = 14, Path = "/img/Food/Breakfast/MiniKalemBoregi.jpg" },
                new Image { Id = 15, FoodId = 15, Path = "/img/Food/Breakfast/BalKaymak.jpg" },
                new Image { Id = 16, FoodId = 16, Path = "/img/Food/Breakfast/TereyagiPorsiyonu.jpg" },
                new Image { Id = 17, FoodId = 17, Path = "/img/Food/Breakfast/SogusTabagi.jpg" },
                new Image { Id = 18, FoodId = 18, Path = "/img/Food/Breakfast/MeyveTabagi.jpg" },
                
                // Zeytinyağlı yemekler görüntüleri
                new Image { Id = 19, FoodId = 19, Path = "/img/Food/OliveOilDishes/biberdolmasi.jpg" },
                new Image { Id = 20, FoodId = 20, Path = "/img/Food/OliveOilDishes/soslupatlican.jpg" },
                new Image { Id = 21, FoodId = 21, Path = "/img/Food/OliveOilDishes/enginar.jpg" },
                new Image { Id = 22, FoodId = 22, Path = "/img/Food/OliveOilDishes/tazefasulye.jpg" },
                new Image { Id = 23, FoodId = 23, Path = "/img/Food/OliveOilDishes/yapraksarma.jpg" },
                new Image { Id = 24, FoodId = 24, Path = "/img/Food/OliveOilDishes/zeytinyagitabagi.jpg" },
                
                // Ara sıcaklar görüntüleri
                new Image { Id = 25, FoodId = 25, Path = "/img/Food/HotAppetizers/patatestava.jpg" },
                new Image { Id = 26, FoodId = 26, Path = "/img/Food/HotAppetizers/pacangaboregi.jpg" },
                new Image { Id = 27, FoodId = 27, Path = "/img/Food/HotAppetizers/mantargraten.jpg" },
                new Image { Id = 28, FoodId = 28, Path = "/img/Food/HotAppetizers/mantarkavurma.jpg" },
                new Image { Id = 29, FoodId = 29, Path = "/img/Food/HotAppetizers/julyendilbaligi.jpg" },
                    
                // Balık çeşitleri görüntüleri
                new Image { Id = 30, FoodId = 30, Path = "/img/Food/TypesofFish/cipuraizgara.jpg" },
                new Image { Id = 31, FoodId = 31, Path = "/img/Food/TypesofFish/cipuraizgarabuyuk.jpg" },
                new Image { Id = 32, FoodId = 32, Path = "/img/Food/TypesofFish/mezgittava.jpg" },
                new Image { Id = 33, FoodId = 33, Path = "/img/Food/TypesofFish/feslegenlisomonizgara.jpg" },
                new Image { Id = 34, FoodId = 34, Path = "/img/Food/TypesofFish/levrekfileto.jpg" },
                new Image { Id = 35, FoodId = 35, Path = "/img/Food/TypesofFish/levrekizgara.jpg" },
                new Image { Id = 36, FoodId = 36, Path = "/img/Food/TypesofFish/levrekizgarabuyuk.jpg" },
                new Image { Id = 37, FoodId = 37, Path = "/img/Food/TypesofFish/kiremittelevrek.jpg" },
                new Image { Id = 38, FoodId = 38, Path = "/img/Food/TypesofFish/somonizgara.jpg" },
                new Image { Id = 39, FoodId = 39, Path = "/img/Food/TypesofFish/dilbaligiizgara.jpg" },
                new Image { Id = 40, FoodId = 40, Path = "/img/Food/TypesofFish/somonkavurma.jpg" },
                
                // Mevsim balıkları görüntüleri
                new Image { Id = 41, FoodId = 41, Path = "/img/Food/SeasonalFish/istavrit.jpg" },
                new Image { Id = 42, FoodId = 42, Path = "/img/Food/SeasonalFish/sarikanat.jpg" },
                new Image { Id = 43, FoodId = 43, Path = "/img/Food/SeasonalFish/palamut.jpg" },
                new Image { Id = 44, FoodId = 44, Path = "/img/Food/SeasonalFish/hamsitava.jpg" },
                new Image { Id = 45, FoodId = 45, Path = "/img/Food/SeasonalFish/mezgit.jpg" },
                new Image { Id = 46, FoodId = 46, Path = "/img/Food/SeasonalFish/tekir.jpg" },
                new Image { Id = 47, FoodId = 47, Path = "/img/Food/SeasonalFish/lufer.jpg" },

                //Izgara ve Kavurmalar
                new Image { Id = 48, FoodId = 48, Path = "/img/Food/TypesofGrillDishes/kofteizgara.jpg" },
                new Image { Id = 49, FoodId = 49, Path = "/img/Food/TypesofGrillDishes/kasarlikofteizgara.jpg" },
                new Image { Id = 50, FoodId = 50, Path = "/img/Food/TypesofGrillDishes/pilicizgara.jpg" },
                new Image { Id = 51, FoodId = 51, Path = "/img/Food/TypesofGrillDishes/pilickavurma.jpg" },
                new Image { Id = 52, FoodId = 52, Path = "/img/Food/TypesofGrillDishes/begendilijulyenpilic.jpg" },
                new Image { Id = 53, FoodId = 53, Path = "/img/Food/TypesofGrillDishes/bonfile.jpg" },
                new Image { Id = 54, FoodId = 54, Path = "/img/Food/TypesofGrillDishes/kasarlibonfile.jpg" },
                new Image { Id = 55, FoodId = 55, Path = "/img/Food/TypesofGrillDishes/begendilijulyenbonfile.jpg" },
                new Image { Id = 56, FoodId = 56, Path = "/img/Food/TypesofGrillDishes/karisikizgara.jpg" },
                new Image { Id = 57, FoodId = 57, Path = "/img/Food/TypesofGrillDishes/cobankavurma.jpg" },

                //Tatlılar
                new Image { Id = 58, FoodId = 58, Path = "/img/Food/Dessert/sutlac.jpg" },
                new Image { Id = 59, FoodId = 59, Path = "/img/Food/Dessert/trilece.jpg" },
                new Image { Id = 60, FoodId = 60, Path = "/img/Food/Dessert/asure.jpg" },
                new Image { Id = 61, FoodId = 61, Path = "/img/Food/Dessert/profiterol.jpg" },
                new Image { Id = 62, FoodId = 62, Path = "/img/Food/Dessert/volkanik.jpg" },
                new Image { Id = 63, FoodId = 63, Path = "/img/Food/Dessert/meyvelitatlilar.jpg" },
                new Image { Id = 64, FoodId = 64, Path = "/img/Food/Dessert/hamurisitatlilari.jpg" },
                new Image { Id = 65, FoodId = 65, Path = "/img/Food/Dessert/dondurmaporsiyon.jpg" },
                new Image { Id = 66, FoodId = 66, Path = "/img/Food/Dessert/meyvetabagi.jpg" },
                new Image { Id = 67, FoodId = 67, Path = "/img/Food/Dessert/tahinlisarma.jpg" },

                //İçecekler
                new Image { Id = 68, FoodId = 68, Path = "/img/Food/Drinks/mesrubatcesitleri.jpg" },
                new Image { Id = 69, FoodId = 69, Path = "/img/Food/Drinks/ayran.jpg" },
                new Image { Id = 70, FoodId = 70, Path = "/img/Food/Drinks/soda.jpg" },
                new Image { Id = 71, FoodId = 71, Path = "/img/Food/Drinks/meyvelisoda.jpg" },
                new Image { Id = 72, FoodId = 72, Path = "/img/Food/Drinks/sikmaportakalnarsuyu.jpg" },
                new Image { Id = 73, FoodId = 73, Path = "/img/Food/Drinks/narcicegiserbeti.jpg" },
                new Image { Id = 74, FoodId = 74, Path = "/img/Food/Drinks/cay.jpg" },
                new Image { Id = 75, FoodId = 75, Path = "/img/Food/Drinks/hazirkahve.jpg" },
                new Image { Id = 76, FoodId = 76, Path = "/img/Food/Drinks/turkkahvesi.jpg" },
                new Image { Id = 77, FoodId = 77, Path = "/img/Food/Drinks/filtrekahve.jpg" },
                new Image { Id = 78, FoodId = 78, Path = "/img/Food/Drinks/su.jpg" },
            };
        }
    }
}
