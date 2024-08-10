using ApplicationCore.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public class SessonData : BaseModel
    {
        public Guid SessionID { get; set; }
        public Guid DatabaseID { get; set; }
        public string language { get; set; }
        public UserInfor User { get; set; }
    }
}
