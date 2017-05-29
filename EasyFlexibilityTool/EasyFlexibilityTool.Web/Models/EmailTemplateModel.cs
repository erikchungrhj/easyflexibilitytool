using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyFlexibilityTool.Web.Models
{
    public class EmailTemplateModel
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
        public string SendCondition { get; set; }
        public string TemplateContent { get; set; }
    }
}
