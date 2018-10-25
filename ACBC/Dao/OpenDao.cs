using ACBC.Buss;
using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class OpenDao
    {
        public User GetUser(string openID)
        {
            User user = null;

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(OpenSqls.SELECT_USER_BY_OPENID, openID);
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

        public bool UserReg(UserRegParam userRegParam, string openID)
        {
            string scanCode = "";
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(openID));
                var strResult = BitConverter.ToString(result);
                scanCode = strResult.Replace("-", "");
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(OpenSqls.INSERT_USER,
                userRegParam.nickName,
                userRegParam.avatarUrl,
                openID,
                scanCode);
            string sqlInsert = builder.ToString();

            return DatabaseOperationWeb.ExecuteDML(sqlInsert);
        }

        private class OpenSqls
        {
            public const string SELECT_USER_BY_OPENID = ""
                + "SELECT * "
                + "FROM T_BASE_USER "
                + "WHERE OPENID = '{0}'";
            public const string INSERT_USER = ""
                + "INSERT INTO T_BASE_USER "
                + "(USER_NAME,USER_IMG,OPENID,SCAN_CODE)"
                + "VALUES( "
                + "'{0}','{1}','{2}','{3}')";
        }
    }
}
