using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    public class HomeBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.HomeApi;
        }
        public object Do_GetHomeShopList(BaseApi baseApi)
        {
            HomeDao homeDao = new HomeDao();
            List<HomeShop> homeShopList = homeDao.GetHomeShopList();

            if (homeShopList == null)
            {
                throw new ApiException(CodeMessage.InvalidHomePageShop, "InvalidHomePageShop");
            }
            return homeShopList;
        }
        public object Do_GetHomeShopInfo(BaseApi baseApi)
        {
            HomeShopParam homeShopParam = JsonConvert.DeserializeObject<HomeShopParam>(baseApi.param.ToString());
            if (homeShopParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            HomeDao homeDao = new HomeDao();
            HomeShopInfo homeShopInfo = homeDao.GetHomeShopInfo(homeShopParam.id);
            if (homeShopInfo == null)
            {
                throw new ApiException(CodeMessage.InvalidHomePageInfoShop, "InvalidHomePageInfoShop");
            }
            return homeShopInfo;
        }
    }
}
