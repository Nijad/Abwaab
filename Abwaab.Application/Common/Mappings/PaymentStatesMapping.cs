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
    }
}
