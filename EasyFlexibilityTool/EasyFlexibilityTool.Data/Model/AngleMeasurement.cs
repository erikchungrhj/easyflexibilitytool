namespace EasyFlexibilityTool.Data.Model
{
    using System;
    using Base;

    public class AngleMeasurement: BaseEntity
    {
        public float Angle { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid AngleMeasurementCategoryId { get; set; }
        public AngleMeasurementCategory Category { get; set; }
    }
}
