using System.ComponentModel.DataAnnotations;

namespace Financial_List.ViewModels
{
    public class RegisterViewModel
    {
        [Key]
        [Required(ErrorMessage = "請輸入身分證字號")]
        [RegularExpression(@"^[A-Z]{1}[1-2]{1}[0-9]{8}$", ErrorMessage = "請輸入正確的身份證字號")]
        [MaxLength(10)]
        public string UserID { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "請輸入正確的 Email 格式")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入扣款帳號")]
        [StringLength(20)]
        public string Account { get; set; }
    }
}
