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

    }
}
