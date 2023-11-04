using System.ComponentModel.DataAnnotations;

namespace Limedika.Contracts
{
    public record Client
    {
        public int Id { get; init; }
        [StringLength(100)]
        public required string Name { get; init; }
        [StringLength(100)]
        public required string Address { get; init; }
        [StringLength(10)]
        public required string PostCode { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}
