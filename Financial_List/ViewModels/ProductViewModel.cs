using System.ComponentModel.DataAnnotations;

namespace Financial_List.ViewModels
{
    public class ProductViewModel
    {
        public int ProductNo { get; set; }
        [MaxLength(100, ErrorMessage = "產品名稱不能超過100個字元")]
        [Required(ErrorMessage = "請輸入產品名稱")]
        public string ProductName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "價格必須為正數")]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "手續費率應介於0%到100%之間")]
        public decimal FeeRate { get; set; }
    }
}
