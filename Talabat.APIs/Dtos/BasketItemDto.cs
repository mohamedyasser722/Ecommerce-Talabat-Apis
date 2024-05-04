using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class BasketItemDto
    {

        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required, Range(0.1, double.MaxValue, ErrorMessage = "Price Must be Greater Than Zero")]
        public decimal Price { get; set; }
        [Required, Range(1, double.MaxValue, ErrorMessage = "Quantity Must be One Item at least")]
        public int Quantity { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
