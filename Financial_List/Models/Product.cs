using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial_List.Models
{
    public class Product
    {
        [Key]
        public int ProductNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 1)]
        public decimal FeeRate { get; set; }

        // 導覽屬性
        public ICollection<LikeList> LikeLists { get; set; }
    }
}
