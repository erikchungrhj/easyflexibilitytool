namespace EasyFlexibilityTool.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Base;

    public class AngleMeasurementModel: BaseModel 
    {
        public Guid Id { get; set; }
        public float Angle { get; set; }
        [Display(Name = "Date")]
        public DateTime DateTimeStamp { get; set; }
        public string UserId { get; set; }
    }
}
