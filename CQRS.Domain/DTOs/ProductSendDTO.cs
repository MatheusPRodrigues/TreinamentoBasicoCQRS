namespace CQRS.Domain.DTOs
{
    public class ProductSendDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
}
