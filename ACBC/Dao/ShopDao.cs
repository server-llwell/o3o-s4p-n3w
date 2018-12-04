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
            if (dt.Rows.Count > 0)
            {
               
                foreach (DataRow dr in dt.Rows)
                {
                    ShopGoods shopGoods = new ShopGoods
                    {

                        shopgoodsId = dr["SHOP_GOODS_ID"].ToString(),
                        goodsImg = dr["GOODS_IMG"].ToString(),
                        goodsName = dr["GOODS_NAME"].ToString(),
                        goodsPrice = dr["GOODS_PRICE"].ToString(),
                       
                    };
                    if (dr["RECOMMEND"].ToString() == "1")
                    {
                        shopGoods.sign = "掌柜推荐";
                    }
                    else if (dr["HOT"].ToString() == "1")
                    {
                        shopGoods.sign = "热销商品";
                    }
                    shopInfo.hotGoods.Add(shopGoods);
                } 
            }

            return shopInfo;
        }

        public ShopInfoHead GetShopInfoHead(string shopId)
        {
            ShopInfoHead shopInfoHead = new ShopInfoHead();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(ShopHeadSql.SELECT_SHOP_INFOHEAD_BY_SHOPID, shopId);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt.Rows.Count>0)
            {
                shopInfoHead.shopId = dt.Rows[0]["SHOP_ID"].ToString();
                shopInfoHead.shopImg = dt.Rows[0]["IMG"].ToString();
                shopInfoHead.shopName = dt.Rows[0]["SHOP_NAME"].ToString();
                shopInfoHead.shopDesc = dt.Rows[0]["DESC"].ToString();
            }

            return shopInfoHead;

        }


        public ShopGoodsDetails GetShopGoodsDetails(string barcode)
        {
            ShopGoodsDetails shopGoodsDetails = new ShopGoodsDetails();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(ShopGoodsDetailsSql.SELECT_SHOPGOODS_BY_GOODSID, barcode);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt.Rows.Count > 0)
            {
                shopGoodsDetails.barcode = barcode;
                shopGoodsDetails.slt = dt.Rows[0]["GOODS_IMG"].ToString();
                shopGoodsDetails.shopId = dt.Rows[0]["SHOP_ID"].ToString();
                shopGoodsDetails.goodsId= dt.Rows[0]["GOODS_ID"].ToString();
                shopGoodsDetails.goodsName = dt.Rows[0]["GOODS_NAME"].ToString();
                shopGoodsDetails.goodsPrice = dt.Rows[0]["GOODS_PRICE"].ToString();
                shopGoodsDetails.gw = dt.Rows[0]["GW"].ToString();
                shopGoodsDetails.model = dt.Rows[0]["MODEL"].ToString();
                shopGoodsDetails.country = dt.Rows[0]["COUNTRY"].ToString();
                shopGoodsDetails.num = dt.Rows[0]["GOODS_STOCK"].ToString();
                shopGoodsDetails.goodsnum = "1";
            }
           
            return shopGoodsDetails;
        }

        public ShopMsg GetShopMsg(string shopid)
        {
            ShopMsg shopMsg = new ShopMsg();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(ShopMsgSql.SELECT_SHOPMSG_BY_SHOPID,shopid);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql,"T").Tables[0];
            if (dt.Rows.Count > 0)
            {
                shopMsg.shopid = dt.Rows[0]["SHOP_ID"].ToString();
                shopMsg.shopname = dt.Rows[0]["SHOP_NAME"].ToString();
                shopMsg.desc = dt.Rows[0]["DESC"].ToString();
                shopMsg.tel = dt.Rows[0]["TEL"].ToString();
                shopMsg.worktime = dt.Rows[0]["OFFICE_HOURS"].ToString();
                shopMsg.img = dt.Rows[0]["IMG"].ToString();
                shopMsg.addr = dt.Rows[0]["ADDR"].ToString();
                if(dt.Rows[0]["SHOP_TYPE"].ToString()=="0")
                {
                    shopMsg.shoptype = "全部";
                }
                else if (dt.Rows[0]["SHOP_TYPE"].ToString() == "1")
                {
                    shopMsg.shoptype = "仅支持零售";
                }
                else
                {
                    shopMsg.shoptype = "仅支持O2O";
                }
                
            }
            return shopMsg;

        }


        private class ShopSqls
        {
            public const string SELECT_SHOP_INFO_BY_SHOPID = ""
                + "SELECT B.RECOMMEND,B.HOT,C.GOODS_NAME,C.GOODS_PRICE,C.GOODS_IMG,B.SHOP_GOODS_ID "
                + "FROM T_BASE_SHOP A,T_BUSS_SHOP_GOODS B,T_BUSS_GOODS C "
                + "WHERE A.SHOP_ID = B.SHOP_ID "
                + "AND B.GOODS_ID = C.GOODS_ID "
                + "AND (B.HOT = '1' OR B.RECOMMEND = '1') "
                + "AND A.SHOP_ID = '{0}' "
                + "ORDER BY B.RECOMMEND DESC "
                + "LIMIT 4";
        }

        private class ShopHeadSql
        {
            public const string SELECT_SHOP_INFOHEAD_BY_SHOPID = ""
                + "SELECT SHOP_ID,SHOP_NAME,IMG,`DESC` "
                + "FROM T_BASE_SHOP "
                + "WHERE SHOP_ID='{0}'";
        }

        private class ShopGoodsDetailsSql
        {
            public const string SELECT_SHOPGOODS_BY_GOODSID = ""
                + "SELECT C.GOODS_STOCK,B.SHOP_ID,B.GOODS_ID,C.GOODS_NAME,C.GOODS_IMG,COUNTRY,MODEL,A.GW,C.GOODS_PRICE"
                + " FROM T_GOODS_LIST A,T_BUSS_SHOP_GOODS B,T_BUSS_GOODS C"
                + " WHERE A.BARCODE=C.BARCODE "
                + " AND B.GOODS_ID=A.ID "
                + " AND A.BARCODE='{0}'";
        }

        private class ShopMsgSql
        {
            public const string SELECT_SHOPMSG_BY_SHOPID = ""
                + "SELECT SHOP_ID,SHOP_NAME,ADDR,TEL,OFFICE_HOURS,SHOP_TYPE,IMG,`DESC` "
                + "FROM T_BASE_SHOP "
                + "WHERE SHOP_ID='{0}'";
        }
    }
}
