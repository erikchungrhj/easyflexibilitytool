namespace EasyFlexibilityTool.Data.Model.Base
{
    using System;

    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
