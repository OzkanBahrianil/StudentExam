namespace StudentProject.Areas.Admin.Models
{
    public class RoleAssignViewModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public bool Exists { get; set; }
    }
}
