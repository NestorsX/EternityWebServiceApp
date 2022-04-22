using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите никнейм")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Длина пароля должна быть не менее 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите роль")]
        public int RoleId { get; set; }
    }
}
