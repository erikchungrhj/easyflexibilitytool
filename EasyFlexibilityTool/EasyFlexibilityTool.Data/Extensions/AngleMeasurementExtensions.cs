namespace EasyFlexibilityTool.Data.Extensions
{
    using System.Collections.Generic;
    using Model;

    public static class AngleMeasurementExtensions
    {
        public static List<double> GetAngleDiff(this List<AngleMeasurement> measurements)
        {
            List<double> result = null;

            if (measurements.Count > 1)
            {
                result = new List<double>();
                for (var i = 1; i < measurements.Count; i++)
                {
                    result.Add(measurements[i].Angle - measurements[i-1].Angle);
                }
            }

            return result;
        }
    }
}
