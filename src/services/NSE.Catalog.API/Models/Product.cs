using NSE.Core.DomainObjects;

namespace NSE.Catalog.API.Models
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public DateTime RegistrationData { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
    }
}
