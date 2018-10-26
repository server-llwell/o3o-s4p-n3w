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
    public class ShopDao
    {
        public ShopInfo GetShopInfo(string shopId)
        {
            ShopInfo shopInfo = new ShopInfo();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(ShopSqls.SELECT_SHOP_INFO_BY_SHOPID, shopId);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                shopInfo.shopId = dt.Rows[0]["SHOP_ID"].ToString();
                shopInfo.shopImg = dt.Rows[0]["IMG"].ToString();
                shopInfo.shopName = dt.Rows[0]["SHOP_NAME"].ToString();
                shopInfo.shopDesc = dt.Rows[0]["DESC"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    ShopGoods shopGoods = new ShopGoods
                    {
                        goodsId = dr["GOODS_ID"].ToString(),
                        goodsImg = dr["GOODS_IMG"].ToString(),
                        goodsName = dr["GOODS_NAME"].ToString(),
                        goodsPrice = dr["GOODS_PRICE"].ToString(),
                    };
                    shopInfo.hotGoods.Add(shopGoods);
                } 
            }

            return shopInfo;
        }

        private class ShopSqls
        {
            public const string SELECT_SHOP_INFO_BY_SHOPID = ""
                + "SELECT * "
                + "FROM T_BASE_SHOP A,T_BUSS_SHOP_GOODS B,T_BUSS_GOODS C "
                + "WHERE A.SHOP_ID = B.SHOP_ID "
                + "AND B.GOODS_ID = C.GOODS_ID "
                + "AND B.HOT = 1 "
                + "AND A.SHOP_ID = {0} "
                + "LIMIT 4";
        }
    }
}
