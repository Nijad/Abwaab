import React, { useState } from "react";
import {
  Box,
  Typography,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  // Chip,
  Paper,
  CircularProgress,
} from "@mui/material";
import WarningAmberIcon from "@mui/icons-material/WarningAmber";

// Pricing plans data schema
const PLANS = [
  {
    id: "properties_count",
    label: "عدد العقارات شهريًا",
    tooltip:
      "إجمالي عدد العقارات التي يمكنك إدراجها وإدارتها على المنصة شهرياً.",
    free: "1",
    pro: "5",
    max: "15",
  },
  {
    id: "promotions_count",
    label: "مرات الترويج شهريًا",
    tooltip:
      "عدد المرات التي يمكنك فيها تمييز وترويج عقاراتك لظهرها في أعلى نتائج البحث.",
    free: "—",
    pro: "2",
    max: "6",
  },
  {
    id: "photos_per_property",
    label: "الصور لكل عقار",
    tooltip: "الحد الأقصى لعدد الصور عالية الجودة التي يمكنك رفعها لكل عقار.",
    free: "5",
    pro: "12",
    max: "20",
  },
  {
    id: "videos_per_property",
    label: "الفيديو لكل عقار",
    tooltip: "إمكانية رفع مقطع فيديو يستعرض العقار بشكل تفصيلي.",
    free: "—",
    pro: "1",
    max: "1",
  },
  // {
  //   id: "video_max_duration",
  //   label: "مدة الفيديو القصوى",
  //   tooltip: "أقصى مدة مسموح بها لمقطع الفيديو المرفوع.",
  //   free: "—",
  //   pro: "60 ثانية",
  //   max: "3 دقائق",
  // },
  // {
  //   id: "video_max_size",
  //   label: "حجم الفيديو الأقصى",
  //   tooltip: "الحد الأقصى لحجم ملف الفيديو للمعاينة السريعة.",
  //   free: "—",
  //   pro: "50 MB",
  //   max: "50 MB",
  // },
];

const Subscriptions = ({ onApiSubmit }) => {
  const [selectedPlan, setSelectedPlan] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [activePlanId, setActivePlanId] = useState("free");
  const [billingCycle, setBillingCycle] = useState("monthly"); // 'monthly' | 'yearly'
  const [activeTooltip, setActiveTooltip] = useState(null);

  const toggleTooltip = (id) => {
    setActiveTooltip(activeTooltip === id ? null : id);
  };

  // Step 1: Open confirmation dialog
  const handleOpenSubscribeDialog = (plan) => {
    setSelectedPlan(plan);
    setIsModalOpen(true);
  };

  const handleCloseDialog = () => {
    if (!isLoading) {
      setIsModalOpen(false);
      setSelectedPlan(null);
    }
  };

  // Step 2: Handle API call to send subscription data to backend
  const handleConfirmSubscription = async () => {
    if (!selectedPlan) return;

    try {
      setIsLoading(true);

      const payload = {
        planId: selectedPlan.id,
        planName: selectedPlan.name,
        price: selectedPlan.price,
        subscribedAt: new Date().toISOString(),
      };

      if (onApiSubmit) {
        await onApiSubmit(payload);
      } else {
        // Mock API call fallback
        await new Promise((resolve) => setTimeout(resolve, 1500));
      }

      setActivePlanId(selectedPlan.id);
      setIsModalOpen(false);
      alert(`تم الاشتراك بنجاح في خطة ${selectedPlan.name}`);
    } catch (error) {
      console.error("Subscription API Error:", error);
      alert("حدث خطأ أثناء إتمام عملية الاشتراك.");
    } finally {
      setIsLoading(false);
      setSelectedPlan(null);
    }
  };

  return (
    <div className="min-h-screen bg-neutral-50 py-10 px-4 md:px-12 font-sans">
      <div className="max-w-6xl mx-auto space-y-12">
        {/* SECTION 1: Page Title */}
        <Box className="text-start space-y-2">
          <Typography
            variant="h4"
            className="font-semibold text-neutral-900 text-2xl md:text-[32px]"
          >
            اختر الخطة التي تناسب حجم أعمالك
          </Typography>
          <Typography
            variant="body1"
            className="text-neutral-500 text-sm md:text-base"
          >
            ابدأ بالخطة المجانية، ثم قم بالترقية عند حاجتك إلى نشر عقارات أكثر
            أو ترويجها والوصول إلى حدود وسائط أعلى.
          </Typography>
        </Box>

        {/* Main Comparison Table Container */}
        <div className="w-full max-w-6xl bg-white rounded-xl shadow-xl overflow-hidden border border-neutral-200">
          <div className="overflow-x-auto scrollbar-thin scrollbar-thumb-neutral-300">
            <table className="w-full border-collapse text-center min-w-[720px] table-fixed">
              <colgroup>
                <col className="w-[31%]" />
                <col className="w-[23%]" />
                <col className="w-[23%]" />
                <col className="w-[23%]" />
              </colgroup>
              <thead>
                <tr className="divide-x divide-x-reverse divide-neutral-200 border-b border-neutral-200">
                  {/* Rightmost Header: Features Title */}
                  <th className="p-6 text-right align-center bg-white">
                    <div className="space-y-1">
                      <h1 className="text-lg md:text-2xl font-extrabold text-neutral-900">
                        المزايا
                      </h1>
                      <p className="text-xs md:text-sm font-normal text-neutral-500 leading-relaxed">
                        مقارنة الحدود الشهرية وحدود الوسائط لكل عقار.
                      </p>
                    </div>
                  </th>

                  {/* Free Plan Header */}
                  <th className="p-6 align-bottom bg-white relative">
                    <div className="flex flex-col items-center justify-end h-full space-y-1">
                      <div className="mb-2">
                        <span className=" text-white text-xs font-medium px-3 py-1  inline-block"></span>
                      </div>
                      <span className="text-lg md:text-2xl font-bold text-neutral-900">
                        المجانية
                      </span>
                      <span className="text-xs text-neutral-500 font-normal">
                        للبداية وتجربة المنصة
                      </span>
                      <div className="pt-2">
                        <span className="text-lg md:text-2xl font-extrabold text-neutral-900">
                          0
                        </span>
                        <span className="text-sm font-bold text-neutral-900 mr-1">
                          ل.س
                        </span>
                      </div>
                      <span className="text-xs text-neutral-400 font-normal">
                        شهرياً
                      </span>
                    </div>
                  </th>

                  {/* Pro Plan Header (Highlighted Sky Blue Column) */}
                  <th className="p-6 align-bottom bg-sky-50/70 relative border-x border-neutral-200">
                    <div className="flex flex-col items-center justify-end h-full space-y-1">
                      {/* Top Badge */}
                      <div className="mb-2">
                        <span className="bg-[#2995cd] text-white text-xs font-medium px-3 py-1 rounded-full shadow-sm inline-block">
                          الأكثر اختيارًا
                        </span>
                      </div>
                      <span className="text-lg md:text-2xl font-extrabold text-[#1170a3]">
                        Pro
                      </span>
                      <span className="text-xs text-neutral-500 font-normal">
                        للمالك النشط
                      </span>
                      <div className="pt-2">
                        <span className="text-lg md:text-2xl font-extrabold text-neutral-900">
                          {billingCycle === "yearly" ? "120,000" : "150,000"}
                        </span>
                        <span className="text-sm font-bold text-neutral-900 mr-1">
                          ل.س
                        </span>
                      </div>
                      <span className="text-xs text-neutral-400 font-normal">
                        شهرياً
                      </span>
                    </div>
                  </th>

                  {/* Max Plan Header */}
                  <th className="p-6 align-bottom bg-white relative">
                    <div className="flex flex-col items-center justify-end h-full space-y-1">
                      <div className="mb-2">
                        <span className=" text-white text-xs font-medium px-3 py-1  inline-block"></span>
                      </div>
                      <span className="text-lg md:text-2xl font-extrabold text-neutral-900">
                        Max
                      </span>
                      <span className="text-xs text-neutral-500 font-normal">
                        للمكاتب والناشرين
                      </span>
                      <div className="pt-2">
                        <span className="text-lg md:text-2xl font-extrabold text-neutral-900">
                          {billingCycle === "yearly" ? "240,000" : "300,000"}
                        </span>
                        <span className="text-sm font-bold text-neutral-900 mr-1">
                          ل.س
                        </span>
                      </div>
                      <span className="text-xs text-neutral-400 font-normal">
                        شهرياً
                      </span>
                    </div>
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-neutral-200 text-neutral-800 text-sm md:text-base">
                {PLANS.map((row) => (
                  <tr
                    key={row.id}
                    className="divide-x divide-x-reverse divide-neutral-200 hover:bg-neutral-50/50 transition-colors"
                  >
                    {/* Feature Title + Info Button */}
                    <td className="py-4 px-6 text-left font-medium text-neutral-900 bg-white">
                      <div className="flex items-center justify-between gap-2">
                        <span className="truncate">{row.label}</span>

                        <div className="relative group">
                          {/* Interactive Tooltip Popover */}
                          <button
                            type="button"
                            onClick={() => toggleTooltip(row.id)}
                            className="w-5 h-5 rounded-full border border-neutral-400 text-neutral-500 flex items-center justify-center text-xs font-bold hover:bg-neutral-100 hover:border-neutral-600 transition-all focus:outline-none"
                            aria-label={`معلومات إضافية عن ${row.label}`}
                          >
                            ؟
                          </button>
                          {activeTooltip === row.id && (
                            <div className="absolute right-0 bottom-full mb-2 w-56 p-2.5 bg-neutral-900 text-white text-xs rounded-lg shadow-xl z-20 font-normal leading-relaxed">
                              {row.tooltip}
                              <div className="absolute right-2 top-full border-4 border-transparent border-t-neutral-900"></div>
                            </div>
                          )}
                        </div>
                      </div>
                    </td>

                    {/* Free Value */}
                    <td className="py-4 px-6 font-semibold text-neutral-800 bg-white">
                      {row.free}
                    </td>

                    {/* Pro Value (Highlighted Column) */}
                    <td className="py-4 px-6 font-bold text-neutral-900 bg-sky-50/70 border-x border-neutral-200">
                      {row.pro}
                    </td>

                    {/* Max Value */}
                    <td className="py-4 px-6 font-semibold text-neutral-800 bg-white">
                      {row.max}
                    </td>
                  </tr>
                ))}

                {}
                <tr className="divide-x divide-x-reverse divide-neutral-200 border-t-2 border-neutral-200">
                  {/* Feature Column Footer (Payment Notice) */}
                  <td className="p-6 text-right align-middle bg-white">
                    <div className="space-y-2">
                      <div className="flex items-center gap-2">
                        <h3 className="text-lg font-bold text-neutral-900">
                          الدفع الإلكتروني
                        </h3>
                      </div>
                      <p className="text-xs text-neutral-500 leading-relaxed max-w-xs">
                        يتم تحويلك إلى بوابة دفع آمنة. لا يحفظ أبواب بيانات
                        البطاقة.
                      </p>
                    </div>
                  </td>

                  {/* Free Plan Action */}
                  <td className="p-6 align-middle bg-white">
                    <button
                      onClick={() => setSelectedPlan("free")}
                      className={`w-full py-3 px-4 rounded-md border text-sm font-semibold transition-all duration-200 ${
                        selectedPlan === "free"
                          ? "border-neutral-300 bg-neutral-50 text-neutral-500 cursor-default"
                          : "border-neutral-300 text-neutral-700 hover:bg-neutral-100"
                      }`}
                    >
                      الخطة الحالية
                    </button>
                  </td>

                  {/* Pro Plan Action (Blue Teal Button) */}
                  <td className="p-6 align-middle bg-sky-50/70 border-x border-neutral-200">
                    <button
                      onClick={() => setSelectedPlan("pro")}
                      className="w-full py-3 px-3 rounded-lg bg-[#3092c5] hover:bg-[#257ea3] text-white font-bold text-sm shadow-md hover:shadow-lg transition-all transform active:scale-[0.98] flex items-center justify-center gap-1"
                    >
                      <span>اشترك —</span>
                      <span>
                        {billingCycle === "yearly" ? "120,000" : "150,000"}
                      </span>
                      <span>ل.س</span>
                    </button>
                  </td>

                  {/* Max Plan Action (Dark Navy Button) */}
                  <td className="p-6 align-middle bg-white">
                    <button
                      onClick={() => setSelectedPlan("max")}
                      className="w-full py-3 px-3 rounded-lg bg-[#0d2137] hover:bg-[#163353] text-white font-bold text-sm shadow-md hover:shadow-lg transition-all transform active:scale-[0.98] flex items-center justify-center gap-1"
                    >
                      <span>اشترك —</span>
                      <span>
                        {billingCycle === "yearly" ? "240,000" : "300,000"}
                      </span>
                      <span>ل.س</span>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        {/* SECTION 4: Confirmation Dialog */}
        <Dialog
          open={isModalOpen}
          onClose={handleCloseDialog}
          maxWidth="xs"
          fullWidth
          dir="rtl"
          PaperProps={{ className: "rounded-2xl p-2 shadow-2xl font-sans" }}
        >
          <DialogTitle className="font-bold text-neutral-900 text-center pb-2">
            تأكيد الاشتراك في الخطة
          </DialogTitle>

          <DialogContent className="space-y-4 py-2">
            {selectedPlan && (
              <Box className="bg-neutral-50 p-4 rounded-xl border border-neutral-200 space-y-2 text-center">
                <Typography className="font-bold text-neutral-800 text-lg">
                  خطة {selectedPlan.name}
                </Typography>
                <Typography className="text-2xl font-extrabold text-sky-600">
                  {selectedPlan.price} {selectedPlan.currency}
                </Typography>
                <Typography
                  variant="caption"
                  className="text-neutral-500 block"
                >
                  سيتم خصم قيمة الاشتراك بشكل {selectedPlan.period}
                </Typography>
              </Box>
            )}
            <Typography
              variant="body2"
              className="text-neutral-600 text-center text-xs"
            >
              بالنقر على "تأكيد"، فإنك توافق على الشروط والأحكام الخاصة بخدمة
              الاشتراكات.
            </Typography>
          </DialogContent>

          <DialogActions className="p-4 pt-2 flex items-center justify-center gap-3">
            <Button
              variant="contained"
              onClick={handleConfirmSubscription}
              disabled={isLoading}
              className="bg-neutral-900 hover:bg-neutral-800 text-white font-bold rounded-xl px-6 py-2 min-w-[110px]"
            >
              {isLoading ? (
                <CircularProgress size={20} color="inherit" />
              ) : (
                "تأكيد"
              )}
            </Button>
            <Button
              variant="outlined"
              onClick={handleCloseDialog}
              disabled={isLoading}
              className="border-neutral-300 text-neutral-600 font-bold rounded-xl px-6 py-2"
            >
              إلغاء
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    </div>
  );
};
export default Subscriptions;
