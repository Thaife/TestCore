using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public enum ServiceResponCode: int
    {
        Success = 0,
        NotPermission = 1,
        Error = 99,
        Exception = 999
    }
}
