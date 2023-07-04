namespace WebAPI.Models.Interfaces
{
    public interface IAddressInterface
    {
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
