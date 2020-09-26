using System.ComponentModel.DataAnnotations;

namespace Beam.Client.ViewModels
{
    public class NewRay
    {
        [Required]
        [Display(Name="Ray Text")]
        public string Text { get; set; } = "";
    }
}