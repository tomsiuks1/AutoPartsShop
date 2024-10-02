using Models.Catalog;

namespace Persistence.CatalogItems
{
    public static class CatalogMaker
    {
        public static List<CarMaker> GetMakers()
        {
            var makers = new List<CarMaker>();

            makers.AddRange(new List<CarMaker>
            {
                new CarMaker
                {
                    Id = Guid.NewGuid(),
                    Name = "BMW",
                },
                new CarMaker
                {
                    Id = Guid.NewGuid(),
                    Name = "Audi",
                },
            });

            return makers;
        }
    }
}
