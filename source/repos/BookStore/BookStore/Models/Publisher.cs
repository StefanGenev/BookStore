using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Непопълнено име")]
        [DisplayName("Име")]
        public string Name { get; set; }
    }
}
