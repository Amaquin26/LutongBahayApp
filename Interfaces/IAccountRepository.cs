using LutongBahayApp.Models;

namespace LutongBahayApp.Interfaces
{
    public interface IAccountRepository
    {
        public bool AddMarketUser(Market market);
        public bool Save();
    }
}
