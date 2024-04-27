using System.ComponentModel.DataAnnotations;


namespace OnlinePharmacy.DTOs

{
    public class ProductDto
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Qty { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
