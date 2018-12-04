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
    public class OrderDao
    {
        /// <summary>
        /// 我的订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderListInfo GetOrderList(string userId)
        {
            OrderListInfo orderListInfo = new OrderListInfo();
           

            StringBuilder builder1 = new StringBuilder();
            builder1.AppendFormat(OrderListSql.SELECT_ORDERLIST_BY_USERID,userId);
            string sql1 = builder1.ToString();
            DataTable dt1 = DatabaseOperationWeb.ExecuteSelectDS(sql1, "T1").Tables[0];

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i< dt1.Rows.Count ; i++)
                { 
                    OrderListMsg orderListMsg = new OrderListMsg();

                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat(OrderListMsgSql.SELECT_ORDER_BY_USERID, userId,dt1.Rows[i]["ORDER_CODE"].ToString());
                    string sql = builder.ToString();
                    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];

                    if (dt.Rows[0]["ORDER_TYPE"].ToString() == "1")
                    {
                        orderListMsg.ordertitle = dt.Rows[0]["SHOP_NAME"].ToString() + "-O2O订单";
                    }
                    else
                    {
                        orderListMsg.ordertitle = dt.Rows[0]["SHOP_NAME"].ToString() + "-零售订单";
                    }

                    orderListMsg.allprice = "¥"+dt.Rows[0]["ORDER_TOTAL"].ToString();
                   
                    if (dt.Rows[0]["FREIGHT"].ToString() != "" && dt.Rows[0]["FREIGHT"].ToString() != null)
                    {
                        orderListMsg.freight ="¥"+ dt.Rows[0]["FREIGHT"].ToString();
                    }
                    else
                    {
                        orderListMsg.freight ="¥0.00";
                    }
                        orderListMsg.ordercode= dt.Rows[0]["ORDER_CODE"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        OrderGoodsMsg orderGoodsMsg = new OrderGoodsMsg();
                            orderGoodsMsg.goodsName = dr["GOODSNAME"].ToString();
                            orderGoodsMsg.goodsImg= dr["GOODS_IMG"].ToString();
                            orderGoodsMsg.goodsPrice= "¥" + dr["SKUUNITPRICE"].ToString() ;
                            orderGoodsMsg.num= dr["QUANTITY"].ToString();
                            orderListMsg.orderGoods.Add(orderGoodsMsg);
                            orderListMsg.allnum +=Convert.ToInt16(dr["QUANTITY"].ToString()) ;
                    }
                    switch (dt.Rows[0]["STATUS"].ToString())
                    {
                        case "0":
                            orderListMsg.state = "待付款";
                            orderListInfo.orderlistDpay.Add(orderListMsg);
                            break;
                        case "1":
                            orderListMsg.state = "已付款";
                            orderListInfo.orderlistPayed.Add(orderListMsg);
                            break;
                        case "3":
                            orderListMsg.state = "已发货";
                            orderListInfo.orderlistDeliver.Add(orderListMsg);
                            break;
                        case "4":
                            orderListMsg.state = "已完成";
                            orderListInfo.orderlistOver.Add(orderListMsg);
                            break;
                    }
                    
                }
            }

            return orderListInfo;
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetOrderDetails GetOrderDetails(string ordercode)
        {
            GetOrderDetails getOrderDetails = new GetOrderDetails();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(GetOrderDetailsSql.SELECT_ORDER_BY_ORDERCODE,ordercode);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt.Rows.Count>0)
            {
                getOrderDetails.name = dt.Rows[0]["CONSIGNEE_NAME"].ToString();
                getOrderDetails.mobille = dt.Rows[0]["CONSIGNEE_MOBILE"].ToString();
                getOrderDetails.addr= dt.Rows[0]["CONSIGNEE_ADDR"].ToString();
                getOrderDetails.idnumber = dt.Rows[0]["ID_NUMBER"].ToString();
                getOrderDetails.express = dt.Rows[0]["EXPRESS"].ToString();
                getOrderDetails.waybillno = dt.Rows[0]["WAYBILLNO"].ToString();
                getOrderDetails.ordercode = dt.Rows[0]["ORDER_CODE"].ToString();
                getOrderDetails.ordertime = dt.Rows[0]["ORDER_TIME"].ToString();
                getOrderDetails.paytime = dt.Rows[0]["PAY_TIME"].ToString();
                getOrderDetails.ordertype= dt.Rows[0]["ORDER_TYPE"].ToString();
                getOrderDetails.ordertotal= dt.Rows[0]["ORDER_TOTAL"].ToString();

                foreach (DataRow dr in dt.Rows)
                {
                    GetOrderDetailsList getOrderDetailsList = new GetOrderDetailsList();
                    getOrderDetailsList.name = dr["GOODSNAME"].ToString();
                    getOrderDetailsList.img = dr["SLT"].ToString();
                    getOrderDetailsList.num = dr["QUANTITY"].ToString();
                    getOrderDetailsList.price = dr["SKUUNITPRICE"].ToString();
                    getOrderDetails.list.Add(getOrderDetailsList);
                }
                
            }
            return getOrderDetails;
        }

        /// <summary>
        /// 生成订单详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetConfirmOrdeItem GetConfirmOrder(string  price)
        {
            GetConfirmOrdeItem  getConfirmOrdeItem = new GetConfirmOrdeItem();
            StringBuilder builder = new StringBuilder();
            getConfirmOrdeItem.freight = 0;//运费
            getConfirmOrdeItem.derate = 0;//活动减免
            getConfirmOrdeItem.price =Convert.ToDouble(price) ;
            getConfirmOrdeItem.coupon = 0;//流连优惠券
            getConfirmOrdeItem.payable = getConfirmOrdeItem.price - getConfirmOrdeItem.derate + getConfirmOrdeItem.freight- getConfirmOrdeItem.coupon;

            return getConfirmOrdeItem;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ConfirmOrdeItem ConfirmOrder(ConfirmOrdeParam confirmOrdeParam)
        {
            ConfirmOrdeItem confirmOrdeItem = new ConfirmOrdeItem();
            StringBuilder builder = new StringBuilder();
            string ordercode = "O2O" + confirmOrdeParam.shopId + DateTime.Now.ToString("yyyyMMddHHmmssff");
            string ordertime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            builder.AppendFormat(OrderDetailsSql.INSERT_INTO_ORDER_BY_CONFIRMORDERPARAM, confirmOrdeParam.shopId, confirmOrdeParam.userId,ordercode, confirmOrdeParam.ordertype, ordertime, confirmOrdeParam.payable, confirmOrdeParam.price, confirmOrdeParam.derate, confirmOrdeParam.couponcode, confirmOrdeParam.coupon, confirmOrdeParam.freight, confirmOrdeParam.consignee, confirmOrdeParam.phone, confirmOrdeParam.addr, confirmOrdeParam.relname, confirmOrdeParam.idcard);
            string sql = builder.ToString();
            List<ConfirmOrdeParamList> list = confirmOrdeParam.list;

            if (list.Count>0 && DatabaseOperationWeb.ExecuteDML(sql))
            {
               
                for (int i = 0; i < list.Count; i++)
                {
                    StringBuilder builder1 = new StringBuilder();
                    builder1.AppendFormat(OrderDetailsSql.INSERT_INTO_ORDER_GOODS_BY_CONFIRMORDERPARAM, ordercode, list[i].barcode, list[i].goodsId, list[i].goodsprice, list[i].goodsnum, list[i].goodsname);
                    string sql1 = builder1.ToString();
                    DatabaseOperationWeb.ExecuteDML(sql1);
                
                }
                confirmOrdeItem.msg = "1";
            }
            else
            {
                confirmOrdeItem.msg = "0";
            }

            return confirmOrdeItem;
        }

        private class OrderDetailsSql
        {
            public const string INSERT_INTO_ORDER_BY_CONFIRMORDERPARAM = ""
                + "INSERT INTO T_ORDER_LIST(SHOP_ID,USER_ID,ORDER_CODE,ORDER_TYPE,`STATUS`,ORDER_TIME,ORDER_TOTAL,GOODS_TOTAL,ORDER_REDUCTION,COUPON_CODE,COUPON_PRICE,FREIGHT,CONSIGNEE_NAME,CONSIGNEE_MOBILE,CONSIGNEE_ADDR,REAL_NAME,ID_NUMBER) "
                + " VALUES('{0}','{1}','{2}','{3}','0','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')";

            public const string INSERT_INTO_ORDER_GOODS_BY_CONFIRMORDERPARAM = ""
                + "INSERT INTO T_ORDER_GOODS(ORDER_CODE,BARCODE,GOODS_ID,SKUUNITPRICE,QUANTITY,GOODSNAME)"
                + "VALUES('{0}','{1}','{2}','{3}','{4}','{5}')";
        }

        private class GetOrderDetailsSql
        {
            public const string SELECT_ORDER_BY_ORDERCODE = ""
                + "SELECT A.ORDER_TOTAL,A.ORDER_TYPE,A.CONSIGNEE_NAME,A.CONSIGNEE_MOBILE,A.CONSIGNEE_ADDR,A.ID_NUMBER,A.EXPRESS,A.WAYBILLNO,A.ORDER_CODE,A.ORDER_TIME,A.PAY_TIME,B.GOODSNAME,SKUUNITPRICE,QUANTITY,SLT   "
                + " FROM  T_ORDER_LIST A,T_ORDER_GOODS B,T_GOODS_LIST C  "
                + " WHERE C.BARCODE=B.BARCODE AND A.ORDER_CODE=B.ORDER_CODE AND A.ORDER_CODE='{0}'";
        }

        private class OrderListMsgSql
        {
            public const string SELECT_ORDER_BY_USERID = ""
                + "SELECT B.ORDER_CODE,A.GOODSNAME,FREIGHT,SKUUNITPRICE,QUANTITY,B.ORDER_TYPE,B.`STATUS`,B.ORDER_TOTAL,C.SHOP_NAME,D.GOODS_IMG "
                + "FROM T_ORDER_GOODS A,T_ORDER_LIST B,T_BASE_SHOP C,T_BUSS_GOODS D "
                + "WHERE A.ORDER_CODE=B.ORDER_CODE "
                + "AND A.GOODS_ID=D.GOODS_ID "
                + " AND B.SHOP_ID=C.SHOP_ID "
                + " AND B.USER_ID={0}"
                + " AND B.ORDER_CODE='{1}'";
        }

        private class OrderListSql
        {
            public const string SELECT_ORDERLIST_BY_USERID = ""
                + "SELECT B.ORDER_CODE "
                + "FROM T_ORDER_GOODS A,T_ORDER_LIST B,T_BASE_SHOP C,T_BUSS_GOODS D "
                + "WHERE A.ORDER_CODE=B.ORDER_CODE "
                + "AND A.GOODS_ID=D.GOODS_ID "
                + " AND B.SHOP_ID=C.SHOP_ID "
                + " AND B.USER_ID='{0}'"
                + " GROUP BY B.ORDER_CODE";
        }
    }
}
