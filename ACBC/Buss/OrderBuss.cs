using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    public class OrderBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.OrderApi;
        }

        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="baseApi"></param>
        /// <returns></returns>
        public object Do_GetOrderList(BaseApi baseApi)
        {
            GetOrderListParam getOrderListParam = JsonConvert.DeserializeObject<GetOrderListParam>(baseApi.param.ToString());
            if (getOrderListParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            OrderDao orderDao = new OrderDao();
            OrderListInfo orderListInfo = orderDao.GetOrderList(getOrderListParam.userId);

            return orderListInfo;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="baseApi"></param>
        /// <returns></returns>
        public object Do_GetOrderDetails(BaseApi baseApi)
        {
            GetOrderDetailsParam getOrderDetailsParam = JsonConvert.DeserializeObject<GetOrderDetailsParam>(baseApi.param.ToString());
            if (getOrderDetailsParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            OrderDao orderDao = new OrderDao();
            GetOrderDetails getOrderDetails = orderDao.GetOrderDetails(getOrderDetailsParam.ordercode);

            return getOrderDetails;
        }

        /// <summary>
        /// 生成订单详情
        /// </summary>
        /// <param name="baseApi"></param>
        /// <returns></returns>
        public object Do_GetConfirmOrder(BaseApi baseApi)
        {
            GetConfirmOrdeParam  getConfirmOrdeParam= JsonConvert.DeserializeObject<GetConfirmOrdeParam>(baseApi.param.ToString());
            if (getConfirmOrdeParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            OrderDao orderDao = new OrderDao();
            GetConfirmOrdeItem getConfirmOrder = orderDao.GetConfirmOrder(getConfirmOrdeParam.price);

            return getConfirmOrder;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="baseApi"></param>
        /// <returns></returns>
        public object Do_ConfirmOrder(BaseApi baseApi)
        {
            ConfirmOrdeParam confirmOrdeParam = JsonConvert.DeserializeObject<ConfirmOrdeParam>(baseApi.param.ToString());
            if (confirmOrdeParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            if (confirmOrdeParam.price == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }
            OrderDao orderDao = new OrderDao();
            ConfirmOrdeItem confirmOrder = orderDao.ConfirmOrder(confirmOrdeParam);

            return confirmOrder;
        }
    }
}
