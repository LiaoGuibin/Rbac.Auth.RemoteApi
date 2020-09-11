using System;
using System.Collections.Generic;
using System.Text;

namespace Rbac.Auth.RemoteApi
{
    internal class D2JsonResultModel
    {
        public int code;
        public string msg;
        public object data;

        public D2JsonResultModel() { }

        public D2JsonResultModel(object data, string msg = "", int code = 0)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
    }
}
