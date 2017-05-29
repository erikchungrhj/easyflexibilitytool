namespace EasyFlexibilityTool.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LeaderboardItemModel
    {
        public string UserId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public AngleMeasurementModel FirstMeasurement { get; set; }
        public AngleMeasurementModel BestMeasurement { get; set; }
        public DateTime LastMeasurementDate { get; set; }
        public double Progress => BestMeasurement.Angle - FirstMeasurement.Angle;
        [Display(Name = "Reward Points")]
        public double RewardPoints { get; set; }
    }
}
