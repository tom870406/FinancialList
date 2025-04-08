namespace Financial_List.ViewModels
{
    public class UserLikeListViewModel
    {
        public int SN { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal FeeRate { get; set; }
        public int OrderAmount { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalAmount { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
    }
}
