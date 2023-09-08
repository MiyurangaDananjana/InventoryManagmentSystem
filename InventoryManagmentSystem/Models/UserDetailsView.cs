namespace InventoryManagmentSystem.Models
{
    public class UserDetailsView
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EPFNumber { get; set; }
        public string Nic { get; set; }
        public string Address { get; set; }
        public string SessionKey { get; set; }
    }
}