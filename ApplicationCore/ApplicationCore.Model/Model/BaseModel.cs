using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model.Model
{
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string Note { get; set; }
    }
}
