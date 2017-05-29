namespace EasyFlexibilityTool.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AngleMeasurementPostModel
    {
        [Range(1, 359)]
        public float Angle { get; set; }
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; }
        [Required]
        public string PhotoData { get; set; }
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
        [Required]
        public DateTime? Date { get; set; }
    }
}
