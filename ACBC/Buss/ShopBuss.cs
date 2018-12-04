using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;

namespace ACBC.Buss
{
    public class ShopBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.ShopApi;
        }

        public object Do_GetShopInfo(BaseApi baseApi)
        {
            GetShopInfoParam getShopInfoParam = JsonConvert.DeserializeObject<GetShopInfoParam>(baseApi.param.ToString());
            if (getShopInfoParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
           
            ShopDao shopDao = new ShopDao();

            ShopInfo shopInfo = Utils.GetCache<ShopInfo>(getShopInfoParam);
            if (shopInfo == null)
            {
                shopInfo = shopDao.GetShopInfo(getShopInfoParam.shopId);

                if (shopInfo == null)
                {
                    throw new ApiException(CodeMessage.InvalidShopId, "InvalidShopId");
                }
                else
                {
                    shopInfo.Unique = getShopInfoParam.GetUnique();
                    Utils.SetCache(shopInfo);
                }
            }

            return shopInfo;
        }

        public object Do_GetShopInfoHead(BaseApi baseApi)
        {
            GetShopInfoParam getShopInfoParam = JsonConvert.DeserializeObject<GetShopInfoParam>(baseApi.param.ToString());
            if (getShopInfoParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            Utils.ClearCache();
            ShopDao shopDao = new ShopDao();

            ShopInfoHead shopInfoHead = Utils.GetCache<ShopInfoHead>(getShopInfoParam);
            if (shopInfoHead == null)
            {
                shopInfoHead = shopDao.GetShopInfoHead(getShopInfoParam.shopId);

                if (shopInfoHead == null)
                {
                    throw new ApiException(CodeMessage.InvalidShopId, "InvalidShopId");
                }
                else
                {
                    shopInfoHead.Unique = getShopInfoParam.GetUnique();
                    Utils.SetCache(shopInfoHead);
                }
            }

            return shopInfoHead;
        }

      


        public object Do_GetShopGoodsDetails(BaseApi baseApi)
        {
            GetShopGoodsDetailsParam getShopGoodsDetailsParam = JsonConvert.DeserializeObject<GetShopGoodsDetailsParam>(baseApi.param.ToString());
            if (getShopGoodsDetailsParam == null )
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            ShopDao shopDao = new ShopDao();

            ShopGoodsDetails shopGoodsDetails = shopDao.GetShopGoodsDetails(getShopGoodsDetailsParam.barcode);

                if (shopGoodsDetails == null)
                {
                    throw new ApiException(CodeMessage.InvalidShopId, "InvalidShopId");
                }
              

            return shopGoodsDetails;

        }

        public object Do_GetShopMsg(BaseApi baseApi)
        {
            GetShopParam getShopParam = JsonConvert.DeserializeObject<GetShopParam>(baseApi.param.ToString());
            if (getShopParam==null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            ShopDao shopDao = new ShopDao();
            ShopMsg shopMsg = shopDao.GetShopMsg(getShopParam.shopId);
            if (shopMsg == null)
            {
                throw new ApiException(CodeMessage.InvalidShopId, "InvalidShopId");
            }

            return shopMsg;
        }

    }
}
