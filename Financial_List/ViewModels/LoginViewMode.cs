using System.ComponentModel.DataAnnotations;

namespace Financial_List.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
