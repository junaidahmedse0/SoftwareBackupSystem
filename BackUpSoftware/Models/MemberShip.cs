using System.ComponentModel.DataAnnotations;

namespace BackUpSoftware.Models
{
    public class MemberShip
    {

        public int Id { get; set; }
     
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}