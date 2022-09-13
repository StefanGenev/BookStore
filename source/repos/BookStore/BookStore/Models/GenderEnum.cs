using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Models
{
    public enum Gender : short
    {
        [Display(Name = "Мъж")]
        Male = 1,
        [Display(Name = "Жена")]
        Female,
    }
}
