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
    public class HomeDao
    {
        public List<HomeShop> GetHomeShopList()
        {
            List<HomeShop> list = new List<HomeShop>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(HomeSqls.SELECT_HOMESHOPLIST);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    HomeShop homeShop = new HomeShop
                    {
                        id = dr["id"].ToString(),
                        shopId = dr["shopId"].ToString(),
                        title = dr["title"].ToString(),
                        createTime = dr["createTime"].ToString(),
                        adress = dr["adress"].ToString(),
                        tel = dr["tel"].ToString(),
                        officeHours = dr["officeHours"].ToString(),
                        shopType = dr["shopType"].ToString(),
                        imgUrl = dr["imgUrl"].ToString()
                    };
                    list.Add(homeShop);
                }
            }
            return list;
        }

        public HomeShopInfo GetHomeShopInfo(string id)
        {
            HomeShopInfo homeShopInfo = null;

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(HomeSqls.SELECT_HOMESHOP_BY_ID, id);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                homeShopInfo = new HomeShopInfo
                {
                    id = dt.Rows[0]["id"].ToString(),
                    shopId = dt.Rows[0]["shopId"].ToString(),
                    title = dt.Rows[0]["title"].ToString(),
                    createTime = dt.Rows[0]["createTime"].ToString(),
                    adress = dt.Rows[0]["adress"].ToString(),
                    tel = dt.Rows[0]["tel"].ToString(),
                    officeHours = dt.Rows[0]["officeHours"].ToString(),
                    shopType = dt.Rows[0]["shopType"].ToString(),
                    imgUrl = dt.Rows[0]["imgUrl"].ToString(),
                    shopName = dt.Rows[0]["shopName"].ToString(),
                    content = dt.Rows[0]["content"].ToString(),
                    message = dt.Rows[0]["message"].ToString(),
                    mapUrl = dt.Rows[0]["mapUrl"].ToString()
                };
                StringBuilder builder1 = new StringBuilder();
                builder1.AppendFormat(HomeSqls.SELECT_HOMESHOPGOODS_BY_SHOPID, homeShopInfo.shopId);
                string sql1 = builder1.ToString();
                DataTable dt1 = DatabaseOperationWeb.ExecuteSelectDS(sql1, "T").Tables[0];
                if (dt1 != null)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        HomeShopGoods homeShopGoods = new HomeShopGoods
                        {
                            id = dr["id"].ToString(),
                            shopId = dr["shopId"].ToString(),
                            goodsId = dr["goodsId"].ToString(),
                            goodsName = dr["goodsName"].ToString(),
                            slt = dr["slt"].ToString(),
                            price = dr["price"].ToString()
                        };
                        homeShopInfo.homeShopGoodsList.Add(homeShopGoods);
                    }
                }
            }
            return homeShopInfo;
        }

        private class HomeSqls
        {
            public const string SELECT_HOMESHOPLIST = ""
                + "SELECT * "
                + "FROM T_HOME_SHOP "
                + "WHERE FLAG = '1'";
            public const string SELECT_HOMESHOP_BY_ID = ""
                + "SELECT * "
                + "FROM T_HOME_SHOP "
                + "WHERE ID = '{0}'";
            public const string SELECT_HOMESHOPGOODS_BY_SHOPID = ""
                + "SELECT * "
                + "FROM T_HOME_GOODS "
                + "WHERE SHOPID = '{0}'";
        }
    }
    
}