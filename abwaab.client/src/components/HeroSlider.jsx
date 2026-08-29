import React, { useState, useEffect, useCallback } from "react";

const slidesData = [
  {
    id: 1,
    tag: "أفضل العقارات المميزة",
    title: "بيتك القادم أقرب مما تتخيل",
    description:
      "اكتشف عقارات موثوقة في أفضل المناطق، وقارن الأسعار واحجز موعد للمعاينة بسهولة.",
    buttonText: "استكشف العقارات",
    bgImage:
      "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?auto=format&fit=crop&w=1600&q=80",
  },
  {
    id: 2,
    tag: "خيارات حصرية",
    title: "اختيارات فاخرة تناسب أسلوب حياتك",
    description:
      "نوفر لك أفضل الشقق والفلل مع تفاصيل كاملة لتجد منزلك الأحلام في وقت قياسي.",
    buttonText: "عرض العقارات المتاحة",
    bgImage:
      "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?auto=format&fit=crop&w=1600&q=80",
  },
  {
    id: 3,
    tag: "تجربة سهلة ومباشرة",
    title: "من البحث إلى المعاينة... بخطوات أبسط",
    description:
      "تصفح بالصور والفيديوهات، حدد الموقع على الخريطة، واحجز موعد المعاينة مباشرة.",
    buttonText: "ابدأ البحث الآن",
    bgImage:
      "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1600&q=80",
  },
];

export const HeroSlider = () => {
  const [currentIndex, setCurrentIndex] = useState(0);

  const handleNext = useCallback(() => {
    setCurrentIndex((prevIndex) => (prevIndex + 1) % slidesData.length);
  }, []);

  const handlePrev = useCallback(() => {
    setCurrentIndex((prevIndex) =>
      prevIndex === 0 ? slidesData.length - 1 : prevIndex - 1
    );
  }, []);

  // 5-second automatic sliding timer
  useEffect(() => {
    const timer = setInterval(() => {
      handleNext();
    }, 5000);

    return () => clearInterval(timer);
  }, [handleNext]);

  return (
    <div className="relative w-full h-[500px] md:h-[600px] overflow-hidden bg-navy-900 font-sans">
      {/* Slide Track */}
      {slidesData.map((slide, index) => {
        const isActive = index === currentIndex;
        return (
          <div
            key={slide.id}
            className={`absolute inset-0 transition-opacity duration-1000 ease-in-out ${
              isActive ? "opacity-100 z-10" : "opacity-0 z-0"
            }`}
          >
            {/* Background Image & Gradient Overlay */}
            <div
              className="absolute inset-0 bg-cover bg-center scale-105 transition-transform duration-10000"
              style={{ backgroundImage: `url(${slide.bgImage})` }}
            >
              <div className="absolute inset-0 bg-gradient-to-r from-navy-800/60 via-navy-800/70 to-navy-800/70" />
            </div>

            {/* Content Box (LTR Aligned) */}
            <div className="relative h-full max-w-7xl mx-auto px-6 md:px-16 flex flex-col justify-center items-start text-left text-white">
              <div className="max-w-2xl space-y-4">
                {/* Tag / Chip */}
                <span className="inline-block bg-teal-500 text-slate-950 text-xs md:text-sm font-semibold px-4 py-1.5 rounded-full shadow-md">
                  {slide.tag}
                </span>

                {/* Main Heading */}
                <h1 className="text-3xl md:text-5xl font-extrabold leading-tight text-white drop-shadow-md">
                  {slide.title}
                </h1>

                {/* Description */}
                <p className="text-slate-200 text-sm md:text-lg font-light leading-relaxed max-w-xl">
                  {slide.description}
                </p>

                {/* Action Button */}
                <div className="pt-2">
                  <button className="bg-slate-900 hover:bg-slate-800 text-white font-medium text-sm md:text-base px-6 py-3 rounded-lg shadow-lg border border-slate-700 transition-all transform hover:-translate-y-0.5">
                    {slide.buttonText}
                  </button>
                </div>
              </div>
            </div>
          </div>
        );
      })}

      {/* Navigation Arrows */}
      <button
        onClick={handlePrev}
        className="absolute left-4 top-1/2 -translate-y-1/2 z-20 w-10 h-10 md:w-12 md:h-12 rounded-full bg-white/90 hover:bg-white text-slate-800 shadow-xl flex items-center justify-center transition-transform hover:scale-110 focus:outline-none"
        aria-label="Previous Slide"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-5 w-5 md:h-6 md:w-6"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          strokeWidth={2.5}
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M15 19l-7-7 7-7"
          />
        </svg>
      </button>

      <button
        onClick={handleNext}
        className="absolute right-4 top-1/2 -translate-y-1/2 z-20 w-10 h-10 md:w-12 md:h-12 rounded-full bg-white/90 hover:bg-white text-slate-800 shadow-xl flex items-center justify-center transition-transform hover:scale-110 focus:outline-none"
        aria-label="Next Slide"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-5 w-5 md:h-6 md:w-6"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          strokeWidth={2.5}
        >
          <path strokeLinecap="round" strokeLinejoin="round" d="M9 5l7 7-7 7" />
        </svg>
      </button>

      {/* Bottom Dot Indicators */}
      <div className="absolute bottom-6 left-1/2 -translate-x-1/2 z-20 flex items-center gap-2">
        {slidesData.map((_, idx) => (
          <button
            key={idx}
            onClick={() => setCurrentIndex(idx)}
            className={`transition-all duration-300 rounded-full focus:outline-none ${
              currentIndex === idx
                ? "w-8 h-2.5 bg-teal-400"
                : "w-2.5 h-2.5 bg-white/60 hover:bg-white"
            }`}
            aria-label={`Go to slide ${idx + 1}`}
          />
        ))}
      </div>
    </div>
  );
};
