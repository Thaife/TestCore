using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public class ServiceRespon
    {
        public ServiceRespon() { }
        public ServiceRespon(object data)
        {
            Data = data;
        }
        public bool Success { get; set; } = true;
        public object Data { get; set; }
        public ServiceResponCode ServiceResponCode { get;set;}
        public string UserMessage { get; set; }
        public string DevMessage { get; set; }
        public DateTime ServerTime { get; set; }

        public ServiceRespon OnSuccess(object Data)
        {
            this.Success = true;
            this.ServiceResponCode = ServiceResponCode.Success;
            this.Data = Data;
            return this;
        }
    }
}
