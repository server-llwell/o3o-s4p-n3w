using ACBC.Buss;
using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class UserDao
    {
        public User GetUser(string userId)
        {
            User user = null;

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(UserSqls.SELECT_USER_CODE_BY_USERID, userId);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                user = new User
                {
                    userId = dt.Rows[0]["USER_ID"].ToString(),
                    userImg = dt.Rows[0]["USER_IMG"].ToString(),
                    userName = dt.Rows[0]["USER_NAME"].ToString(),
                    openid = dt.Rows[0]["OPENID"].ToString(),
                    scanCode = dt.Rows[0]["SCAN_CODE"].ToString(),
                };
            }

            return user;
        }


        private class UserSqls
        {
            public const string SELECT_USER_CODE_BY_USERID = ""
                + "SELECT * "
                + "FROM T_BASE_USER "
                + "WHERE USER_ID = {0}";
        }
    }
}
