using Microsoft.EntityFrameworkCore;
using Models.Catalog;

namespace Persistence
{
    public interface IDataContext
    {
        DbSet<CarMaker> Makers { get; set; }
        DbSet<CarModel> Models { get; set; }
    }
}