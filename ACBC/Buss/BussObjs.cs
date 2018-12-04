using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    #region Sys
    public class BussCache
    {
        private string unique = "";
        public string Unique
        {
            get
            {
                return unique;
            }
            set
            {
                unique = value;
            }
        }
    }

    public class BussParam
    {
        public string GetUnique()
        {
            string needMd5 = "";
            string md5S = "";
            foreach (FieldInfo f in this.GetType().GetFields())
            {
                needMd5 += f.Name;
                needMd5 += f.GetValue(this).ToString();
            }
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(needMd5));
                var strResult = BitConverter.ToString(result);
                md5S = strResult.Replace("-", "");
            }
            return md5S;
        }
    }

    public class SessionUser
    {
        public string openid;
        public string checkPhone;
        public string checkCode;
        public string userType;
        public string userId;
    }

    public class SmsCodeRes
    {
        public int error_code;
        public string reason;
    }

    public class WsPayState
    {
        public string userId;
        public string scanCode;
    }

    public class ExchangeRes
    {
        public string reason;
        public ExchangeResult result;
        public int error_code;
    }
    public class ExchangeResult
    {
        public string update;
        public List<string[]> list;
    }

    public enum ScanCodeType
    {
        Shop,
        User,
        Null,
    }


    #endregion

    #region Params

    public class LoginParam
    {
        public string code;
    }

    public class UserRegParam
    {
        public string avatarUrl;
        public string city;
        public string country;
        public string gender;
        public string language;
        public string nickName;
        public string province;
    }

    public class HomeShopParam:BussParam
    {
        public string id;
    }

    public class GetShopInfoParam : BussParam
    {
        public string shopId;
    }

    public class GetShopGoodsDetailsParam 
    {
        //public string shopid;//商店Id
        public string barcode;//商品code
    }

    public class GetShopParam
    {
        public string shopId;
    }

    public class GetOrderListParam
    {
        public string userId;
       
    }

    public class GetOrderDetailsParam
    {
        public string ordercode;//订单编号
    }

    public class GetConfirmOrdeParam
    {
        public string  price;//商品小计
    }

    public class ConfirmOrdeParam
    {
        public string consignee;//收货人
        public string relname;//真实姓名
        public string idcard;//身份证
        public string phone;//电话
        public string addr;//收货地址
        public string price;//商品小计
        public string freight;//运费
        public string derate;//活动减免
        public string payable;//应付款
        public string couponcode;//流连优惠券code
        public string coupon;//流连优惠券钱数
        public string ordertype;//订单类型
        public string shopId;//商店id
        public string userId;//用户id
        public List<ConfirmOrdeParamList> list = new List<ConfirmOrdeParamList>();
    }

    public class ConfirmOrdeParamList
    {
        public string goodsname;//商品名
        public string goodsId;//商品id
        public string goodsnum;//商品数
        public string goodsprice;//商品钱
        public string barcode;//商品条码
    }

    #endregion

    #region DaoObjs

    public class User
    {
        public string userName;
        public string userId;
        public string openid;
        public string userImg;
        public string scanCode;
    }

    public class ConfirmOrdeItem
    {
        public string msg;//1：成功，2：失败
      
    }

    public class GetConfirmOrdeItem
    {
        public double price;//商品小计
        public double freight;//运费
        public double derate;//活动减免
        public double payable;//应付款
        public double coupon;//流连优惠券
    }

    public class HomeShopList: BussCache
    {
        public List<HomeShop> homeShopList = new List<HomeShop>();
    }
    public class HomeShop
    {
        public string id;
        public string shopId; //店铺id
        public string title;//标题
        public string createTime;//发表时间
        public string adress;//地址
        public string tel;//联系电话
        public string officeHours;//营业时间
        public string shopType;//商店类型
        public string imgUrl;//图片地址
    }
    public class HomeShopInfo : BussCache
    {
        public string id;
        public string shopId; //店铺id
        public string title;//标题
        public string createTime;//发表时间
        public string adress;//地址
        public string tel;//联系电话
        public string officeHours;//营业时间
        public string shopType;//商店类型
        public string imgUrl;//图片地址
        public string shopName;//店铺名
        public string content;//文章内容
        public string message;//商家留言
        public string mapUrl;//地图地址-无效
        public List<HomeShopGoods> homeShopGoodsList = new List<HomeShopGoods>();//新上架商品
    }
    public class HomeShopGoods
    {
        public string id;
        public string shopId;//店铺id
        public string goodsId;//商品id
        public string goodsName;//商品名称
        public string slt;//商品图片
        public string price;//商品价格
    }

    public class ShopInfo : BussCache
    {
        public List<ShopGoods> hotGoods = new List<ShopGoods>();
    }
    public class ShopInfoHead : BussCache
    {
        public string shopId;//商店id
        public string shopName;//商店名
        public string shopDesc;//商店描述
        public string shopImg;//商店图片
    }
    public class ShopGoods : BussCache
    {
        public string shopgoodsId;//商店商品编号
        public string goodsName;//商品名称
        public string goodsPrice;//商品价格
        public string goodsImg;//商品图片
        public string sign;//商品提示
    }

    public class ShopGoodsDetails 
    {
        public string barcode;//商品code
        public string shopId;//商店id
        public string goodsId;//商品id
        public string goodsName;//商品名称
        public string goodsnum;//商品个数
        public string num;//库存
        public string goodsPrice;//商品价格
        public string country;//原产地
        public string model;//规格
        public string gw;//净重
        public string slt;//图片
    }

    public class ShopMsg
    {
        public string shopid;//商店ID
        public string shopname;//商店名
        public string addr;//地址
        public string tel;//电话
        public string worktime;//营业时间
        public string shoptype;//商店类型
        public string img;//图片
        public string desc;//描述
    }

    public class OrderListInfo
    {
        public List<OrderListMsg> orderlistDpay = new List<OrderListMsg>();//待付款
        public List<OrderListMsg> orderlistPayed = new List<OrderListMsg>();//已付款
        public List<OrderListMsg> orderlistDeliver = new List<OrderListMsg>();//已发货
        public List<OrderListMsg> orderlistOver = new List<OrderListMsg>();//已完成


    }
    public class OrderListMsg
    {
        public string allprice;//合计
        public string ordertitle;//订单标题
        public string state;//状态
        public string freight;//运费
        public string ordercode;//订单码
        public int allnum;//数量
        public List<OrderGoodsMsg> orderGoods = new List<OrderGoodsMsg>();
    }
    public class OrderGoodsMsg
    {
       
        public string goodsName;//商品名称
        public string goodsPrice;//商品价格
        public string goodsImg;//商品图片
        public string num;//商品数量
    }

    public class GetOrderDetails
    {
        public string ordertype;//订单类型 1线上，2零售
        public string name;//联系人
        public string mobille;//联系人电话
        public string addr;//地址 
        public string idnumber;//身份证
        public string express;//快递名
        public string waybillno;//快递单号
        public string ordercode;//订单号
        public string ordertime;//下单时间
        public string paytime;//付款时间
        public string ordertotal;//订单金额
        public List<GetOrderDetailsList> list = new List<GetOrderDetailsList>();

    }
    public class GetOrderDetailsList
    {
        public string img;//图片
        public string name;//商品名
        public string num;//数量
        public string price;//价钱
    }

    #endregion
}
