using lab3.Models;

namespace lab3.Services
{
    public interface ICachedBankDb
    {
        void AddInvestorToCache(string key, int rowsNumber = 20);
        IEnumerable<Investor> GetInvestor(string key, int rowsNumber = 20);
    }
}
