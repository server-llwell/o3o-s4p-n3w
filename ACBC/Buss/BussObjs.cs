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
        public string shopId;
        public string shopName;
        public string shopDesc;
        public string shopImg;
        public List<ShopGoods> hotGoods = new List<ShopGoods>();
    }

    public class ShopGoods
    {
        public string goodsId;
        public string goodsName;
        public string goodsPrice;
        public string goodsImg;
    }

    #endregion
}
