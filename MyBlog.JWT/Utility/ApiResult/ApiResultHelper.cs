
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.JWT.Utility.ApiResult
{
    public class ApiResultHelper
    {
        public static ApiResult Success(dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Data = data,
                Msg = "操作成功",
                Total = 0
            };
        }
        public static ApiResult Success(dynamic data, int total)
        {
            return new ApiResult
            {
                Code = 200,
                Data = data,
                Msg = "操作成功",
                Total = total
            };
        }
        public static ApiResult Error(string message)
        {
            return new ApiResult
            {
                Code = 500,
                Data = null,
                Msg = message,
                Total = 0
            };
        }
    }
}
