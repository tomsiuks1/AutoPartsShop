
using Models.Enums;

namespace Models.Vehicles
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public FuelType Fuel { get; set; }
        public TransmissionType Transmission { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
