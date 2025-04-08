using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial_List.Models
{
    public class Product
    {
        [Key]
        public int ProductNo { get; set; }

        [Required(ErrorMessage ="請輸入產品名稱")]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "請輸入產品價格")]
        [Range(0, double.MaxValue,ErrorMessage ="價格請輸入正數")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "請輸入手續費率")]
        [Range(0, 1, ErrorMessage = "手續費率應介於0到1之間")]
        public decimal? FeeRate { get; set; }

        // 導覽屬性
        public ICollection<LikeList> LikeLists { get; set; }
    }
}
