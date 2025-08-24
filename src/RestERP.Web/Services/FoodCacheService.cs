using Microsoft.Extensions.Caching.Memory;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Web.Services
{
    public class FoodCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<FoodCacheService> _logger;
        private readonly IFoodService _foodService;
        
        // Cache anahtarları private readonly olarak tanımlanmalı
        private static readonly string CategoriesCacheKey = "FoodCategories";
        private static readonly string FoodsCacheKey = "Foods";
        private static readonly string ImagesCacheKey = "Images";
        
        // Cache süreleri farklılaştırılabilir
        private static readonly TimeSpan CategoriesCacheDuration = TimeSpan.FromHours(3);  // Kategoriler daha nadir değişir
        private static readonly TimeSpan FoodsCacheDuration = TimeSpan.FromHours(1);
        private static readonly TimeSpan ImagesCacheDuration = TimeSpan.FromMinutes(30);
        
        public FoodCacheService(IMemoryCache memoryCache, ILogger<FoodCacheService> logger, IFoodService foodService)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _foodService = foodService;
        }
        
        public List<FoodCategory> GetCategories()
        {
            if (!_memoryCache.TryGetValue(CategoriesCacheKey, out List<FoodCategory> categories))
            {
                _logger.LogInformation("Cache miss for food categories, loading from data source");
                categories = FoodCategorySeedData.GetFoodCategories();
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CategoriesCacheDuration)
                    .RegisterPostEvictionCallback((key, value, reason, state) => 
                    {
                        _logger.LogInformation($"Cache evicted for {key}: {reason}");
                    });
                    
                _memoryCache.Set(CategoriesCacheKey, categories, cacheOptions);
            }
            else
            {
                _logger.LogDebug("Cache hit for food categories");
            }
            
            return categories;
        }
        
        public List<Food> GetFoods()
        {
            if (!_memoryCache.TryGetValue(FoodsCacheKey, out List<Food> foods))
            {
                _logger.LogInformation("Cache miss for foods, loading from data source");
                foods = _foodService.GetAllFoodsAsync().Result.ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(FoodsCacheDuration)
                    .RegisterPostEvictionCallback((key, value, reason, state) => 
                    {
                        _logger.LogInformation($"Cache evicted for {key}: {reason}");
                    });
                    
                _memoryCache.Set(FoodsCacheKey, foods, cacheOptions);
            }
            else
            {
                _logger.LogDebug("Cache hit for foods");
            }
            
            return foods;
        }
        
        public List<Image> GetImages()
        {
            if (!_memoryCache.TryGetValue(ImagesCacheKey, out List<Image> images))
            {
                _logger.LogInformation("Cache miss for images, loading from data source");
                images = ImageSeedData.GetImages();
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(ImagesCacheDuration)
                    .RegisterPostEvictionCallback((key, value, reason, state) => 
                    {
                        _logger.LogInformation($"Cache evicted for {key}: {reason}");
                    });
                    
                _memoryCache.Set(ImagesCacheKey, images, cacheOptions);
            }
            else
            {
                _logger.LogDebug("Cache hit for images");
            }
            
            return images;
        }
        
        // Cache'i manuel olarak temizlemek için metod
        public void ClearCache()
        {
            _memoryCache.Remove(CategoriesCacheKey);
            _memoryCache.Remove(FoodsCacheKey);
            _memoryCache.Remove(ImagesCacheKey);
            _logger.LogInformation("Food cache cleared manually");
        }
    }
} 