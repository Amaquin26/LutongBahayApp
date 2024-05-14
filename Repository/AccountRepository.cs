using LutongBahayApp.Data;
using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;

namespace LutongBahayApp.Repository
{
    public class AccountRepository(ApplicationDbContext context) : IAccountRepository
    {
        private readonly ApplicationDbContext _context = context;

        public bool AddMarketUser(Market market)
        {
            _context.Add(market);
            return Save();
        }



        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }
    }
}
