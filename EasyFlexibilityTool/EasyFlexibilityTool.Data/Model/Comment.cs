using EasyFlexibilityTool.Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFlexibilityTool.Data.Model
{
    public class Comment : BaseEntity
    {
        public string ImageId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string CommentContent { get; set; }
    }
}
