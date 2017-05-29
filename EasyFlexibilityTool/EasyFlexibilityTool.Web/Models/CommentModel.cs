using EasyFlexibilityTool.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyFlexibilityTool.Web.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string ImageId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string CommentContent { get; set; }
    }
}
