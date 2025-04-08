using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Financial_List.Models
{
    public class LikeList
    {
        [Key]
        public int SN { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public int ProductNo { get; set; }

        [Range(1, 999, ErrorMessage = "訂購數量必須介於1到999之間")]
        [Required(ErrorMessage = "請輸入訂購數量")]       
        public int OrderAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Account { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalFee { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        // 導覽屬性
        [ForeignKey("UserID")]
        public User User { get; set; }

        [ForeignKey("ProductNo")]
        public Product Product { get; set; }
    }
}
