namespace EasyFlexibilityTool.Web.Models
{
    using System;
    using Data.Model;

    public class TrainingReminderModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int AngleMeasurementsCount { get; set; }
        public AngleMeasurement LastAngleMeasurement { get; set; }
        public AngleMeasurement FirstAngleMeasurement { get; set; }
        public string CategoryName { get; set; }
        public float ProgressRate => AngleMeasurementsCount > 1 ? (LastAngleMeasurement.Angle - FirstAngleMeasurement.Angle)/AngleMeasurementsCount : 0;
        public DateTime? FullSplitEstimationDate
        {
            get
            {
                DateTime? result = null;

                if (AngleMeasurementsCount > 1 && ProgressRate > 0)
                {
                    var restAngle = 180 - LastAngleMeasurement.Angle;
                    var trainingsCount = Math.Ceiling(restAngle/ProgressRate);
                    var daysCount = trainingsCount*2;

                    result = DateTime.UtcNow.Date.AddDays(daysCount);
                }

                return result;
            }
        }
        public DateTime? NextTrainingDate
        {
            get
            {
                DateTime? result = null;

                if (AngleMeasurementsCount > 0)
                {
                    result = LastAngleMeasurement.DateTimeStamp.Date.AddDays(2);
                }

                return result;
            }
        }
    }
}
