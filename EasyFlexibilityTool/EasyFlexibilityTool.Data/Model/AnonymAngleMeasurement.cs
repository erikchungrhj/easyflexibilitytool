namespace EasyFlexibilityTool.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Base;

    public class AnonymAngleMeasurement: BaseEntity
    {
        public float Angle { get; set; }
        public DateTime DateTimeStamp { get; set; }
        [Required, StringLength(254), Index(IsUnique = true)]
        public string Email { get; set; }
        public Guid AngleMeasurementCategoryId { get; set; }
        public AngleMeasurementCategory Category { get; set; }
    }
}
