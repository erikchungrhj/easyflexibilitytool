using EasyFlexibilityTool.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFlexibilityTool.Data.Model
{
    public class EmailTemplate : BaseEntity
    {
        public string TemplateName { get; set; }
        public string SendCondition { get; set; }
        public string TemplateContent { get; set; }
    }
}
