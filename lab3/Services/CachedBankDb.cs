using lab3.Models;
using Microsoft.Extensions.Caching.Memory;

namespace lab3.Services
{
    public class CachedBankDb
    {
        private readonly BankDeposits1Context _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly int _saveTime;
        public CachedBankDb(BankDeposits1Context dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _saveTime = 2 * 17 + 240;
        }
        public void AddInvestorToCache(string key, int rowsNumber = 100)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Investor> cachedUser))
            {
                cachedUser = _dbContext.Investors.Take(rowsNumber).ToList();

                if (cachedUser != null)
                {
                    _memoryCache.Set(key, cachedUser, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Investor занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Investor уже находится в кеше");
            }
        }
        public IEnumerable<Investor> GetInvestor(string key, int rowsNumber = 100)
        {
            IEnumerable<Investor> investors;  
            if (!_memoryCache.TryGetValue(key, out investors))
            {
                investors = _dbContext.Investors.Take(rowsNumber).ToList();
                if (investors != null)
                {
                    _memoryCache.Set(key, investors,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return  investors;
        }
    }
}
