using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class PaymentStatesMapping
    {
        public static string Map(PaymentStatesEnum state)
        {
            return state switch
            {
                PaymentStatesEnum.Pending => "بالانتظار",
                PaymentStatesEnum.Completed => "مكتمل",
                PaymentStatesEnum.Failed => "فاشل",
                PaymentStatesEnum.Cancelled => "ملغى",
                PaymentStatesEnum.Refunded => "مرتجع",
                PaymentStatesEnum.Expired => "منتهي الصلاحية",
                _ => ""
            };
        }

        public static string Map(string state)
        {
            if(state == PaymentStatesEnum.Pending.ToString())
                return Map(PaymentStatesEnum.Pending);
            if(state == PaymentStatesEnum.Completed.ToString())
                return Map(PaymentStatesEnum.Completed);
            if(state == PaymentStatesEnum.Failed.ToString())
                return Map(PaymentStatesEnum.Failed);
            if(state == PaymentStatesEnum.Cancelled.ToString())
                return Map(PaymentStatesEnum.Cancelled);
            if(state == PaymentStatesEnum.Refunded.ToString())
                return Map(PaymentStatesEnum.Refunded);
            if(state == PaymentStatesEnum.Expired.ToString())
                return Map(PaymentStatesEnum.Expired);
            return "";
        }
    }
}
