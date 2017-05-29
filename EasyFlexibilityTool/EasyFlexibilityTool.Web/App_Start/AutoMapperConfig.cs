namespace EasyFlexibilityTool.Web
{
    using System.Collections.Generic;
    using AutoMapper;
    using Data.Model;
    using Models;

    public static class AutoMapperConfig
    {
        // Left for the usage like: var mapper = config.CreateMapper();
        /*public static MapperConfiguration Get()
        {
            return new MapperConfiguration(config =>
            {
                MapAngleMeasurement(config);
            });
        }*/

        public static void AddMappings (this IMapperConfigurationExpression config)
        {
            MapAngleMeasurement(config);
            MapLeaderboardItem(config);
            MapAngleMeasurementCategory(config);
        }

        private static void MapAngleMeasurement(IMapperConfigurationExpression config)
        {
            config.CreateMap<AngleMeasurement, AngleMeasurementModel>();
        }

        private static void MapLeaderboardItem(IMapperConfigurationExpression config)
        {
            config.CreateMap<KeyValuePair<string, List<AngleMeasurement>>, LeaderboardItemModel>()
                .ForMember(m => m.UserName, o => o.MapFrom(e => e.Key))
                .ForMember(m => m.FirstMeasurement, o => o.MapFrom(e => e.Value[0]))
                .ForMember(m => m.BestMeasurement, o => o.MapFrom(e => e.Value[1]));
        }

        private static void MapAngleMeasurementCategory(IMapperConfigurationExpression config)
        {
            config.CreateMap<AngleMeasurementCategory, AngleMeasurementCategoryModel>();
        }
    }
}