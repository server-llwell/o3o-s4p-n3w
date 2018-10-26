using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;

namespace ACBC.Buss
{
    public class UserBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.UserApi;
        }

        public object Do_GetScanCode(BaseApi baseApi)
        {
            UserDao userDao = new UserDao();
            User user = userDao.GetUser(Utils.GetUserId(baseApi.token));
            if(user == null)
            {
                throw new ApiException(CodeMessage.InvalidUserCode, "InvalidUserCode");
            }
            return new { user.scanCode };
        }
    }
}
