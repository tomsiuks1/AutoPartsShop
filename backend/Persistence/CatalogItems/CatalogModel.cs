using Models.Catalog;

namespace Persistence.CatalogItems
{
    public static class CatalogModel
    {
        public static List<CarModel> GetModels()
        {
            var makers = new List<CarModel>();

            makers.AddRange(new List<CarModel>
            {
                new CarModel
                {
                    Id = Guid.NewGuid(),
                    Name = "320",
                },
                new CarModel
                {
                    Id = Guid.NewGuid(),
                    Name = "330",
                },
                new CarModel
                {
                    Id = Guid.NewGuid(),
                    Name = "328",
                },
                new CarModel
                {
                    Id = Guid.NewGuid(),
                    Name = "A4",
                },
            });

            return makers;
        }
    }
}
