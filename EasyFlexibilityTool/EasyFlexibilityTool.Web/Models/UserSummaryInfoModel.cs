using EasyFlexibilityTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyFlexibilityTool.Web.Models
{
    public class UserSummaryInfoModel
    {
        public string UserName { get; set; }
        public int NumberOfDaysSinceLastUpdate { get; set; }
        public double ProgressRate { get; set; }
        public double CurrentDegrees { get; set; }
        public double FirstDegrees { get; set; }
        
        public UserSummaryInfoModel(ApplicationDbContext dbContext, Data.Model.ApplicationUser user)
        {
            UserName = user.UserName;
            try
            {
                var angleMeasurements = dbContext.AngleMeasurements.Where(am => am.UserId == user.Id).OrderBy(am => am.DateTimeStamp).ToList();
                var firstMeasurement = angleMeasurements.First();
                var lastMeasurement = angleMeasurements.Last();
                NumberOfDaysSinceLastUpdate = (DateTime.Now - lastMeasurement.DateTimeStamp).Days;
                ProgressRate = (lastMeasurement.Angle - firstMeasurement.Angle) / angleMeasurements.Count;
                CurrentDegrees = lastMeasurement.Angle;
                FirstDegrees = firstMeasurement.Angle;
            }
            catch { }
        }

        public string getAdjustedTemplate(string templateString)
        {
            return templateString.Replace("{UserName}", UserName)
                .Replace("{NumberOfDaysSinceLastUpdate}", NumberOfDaysSinceLastUpdate.ToString())
                .Replace("{ProgressRate}", ProgressRate.ToString("0.00"))
                .Replace("{CurrentDegrees}", CurrentDegrees.ToString("0.00"))
                .Replace("{FirstDegrees}", FirstDegrees.ToString("0.00"));
        }
    }
}
