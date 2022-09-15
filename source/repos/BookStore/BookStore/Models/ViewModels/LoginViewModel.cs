using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Непопълнен имейл")]
        [DisplayName("Имейл")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Непопълнена парола")]
        [DisplayName("Парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомни потребител")]
        public bool RememberUser { get; set; }
    }
}
