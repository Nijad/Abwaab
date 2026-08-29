import React, { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  IconButton,
  Box,
  Typography,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import ArrowBackIosNewIcon from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";

export const PropertyMediaGalleryModal = ({
  open,
  onClose,
  mediaItems = [
    {
      id: 1,
      type: "image",
      url: "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?auto=format&fit=crop&w=1200&q=80",
      alt: "Property Exterior",
    },
    {
      id: 2,
      type: "image",
      url: "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?auto=format&fit=crop&w=1200&q=80",
      alt: "Living Room View",
    },
    {
      id: 3,
      type: "image",
      url: "https://images.unsplash.com/photo-1616594039964-ae9021a400a0?auto=format&fit=crop&w=1200&q=80",
      alt: "Master Bedroom",
    },
    {
      id: 4,
      type: "image",
      url: "https://images.unsplash.com/photo-1556911220-e15b29be8c8f?auto=format&fit=crop&w=1200&q=80",
      alt: "Kitchen",
    },
    {
      id: 5,
      type: "image",
      url: "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
      alt: "Balcony View",
    },
    {
      id: 5,
      type: "image",
      url: "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
      alt: "Balcony View",
    },
    {
      id: 5,
      type: "image",
      url: "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
      alt: "Balcony View",
    },
    {
      id: 5,
      type: "image",
      url: "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
      alt: "Balcony View",
    },
    {
      id: 5,
      type: "image",
      url: "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
      alt: "Balcony View",
    },
  ],
}) => {
  const [currentIndex, setCurrentIndex] = useState(1); // Default to 2nd image matching design

  const currentMedia = mediaItems[currentIndex];

  const handleNext = () => {
    setCurrentIndex((prev) => (prev === mediaItems.length - 1 ? 0 : prev + 1));
  };

  const handlePrev = () => {
    setCurrentIndex((prev) => (prev === 0 ? mediaItems.length - 1 : prev - 1));
  };

  return (
    <React.Fragment>
      {/* <Dialog
        open={open}
        onClose={onClose}
        maxWidth="lg"
        fullWidth
        sx={{ ".MuiPaper-root": { borderRadius: "18px" } }}
      > */}
      {/* Header */}
      {/* <DialogTitle className="flex items-center justify-between pb-2 pt-1 px-2">
          <Typography
            variant="h6"
            className="font-bold text-slate-900 text-lg md:text-xl"
          >
            صور وفيديو العقار
          </Typography>
          <Box className="flex items-center justify-between gap-3">
            <Typography variant="body2" className="text-slate-500 font-medium">
              {currentIndex + 1} من {mediaItems.length}
            </Typography>
            <IconButton
              onClick={onClose}
              size="small"
              className="text-slate-500 hover:text-slate-900 p-1"
            >
              <CloseIcon fontSize="small" />
            </IconButton>
          </Box>
        </DialogTitle> */}

      {/* Main Content Area */}
      {/* <DialogContent className="px-2 py-0 flex flex-col justify-between"> */}
      {/* Main Preview */}
      <Box className="relative my-3 flex-1 flex items-center justify-center min-h-[350px] md:min-h-[480px] bg-slate-100 rounded-2xl overflow-hidden">
        {currentMedia?.type === "video" ? (
          <video
            src={currentMedia.videoUrl}
            controls
            autoPlay
            className="w-full h-full max-h-[60vh] object-cover rounded-2xl"
          />
        ) : (
          <img
            src={currentMedia?.url}
            alt={currentMedia?.alt || "Property Media"}
            className="w-full h-full max-h-[60vh] object-cover rounded-2xl transition-all duration-300"
          />
        )}

        {/* Left Navigation Arrow (Previous in RTL) */}
        <IconButton
          onClick={handlePrev}
          className="!absolute left-4 top-1/2 -translate-y-1/2 !bg-white/70 hover:!bg-white !text-navy-600 shadow-md p-2"
        >
          <ArrowBackIosNewIcon className="text-sm" />
        </IconButton>

        {/* Right Navigation Arrow (Next in RTL) */}
        <IconButton
          onClick={handleNext}
          className="!absolute right-4 top-1/2 -translate-y-1/2 !bg-white/70 hover:!bg-white !text-navy-600 shadow-md p-2"
        >
          <ArrowForwardIosIcon className="text-sm" />
        </IconButton>
      </Box>

      {/* Thumbnail Preview Row */}
      <Box className="flex justify-between flex-nowrap overflow-x-auto pt-2 pb-3">
        {mediaItems.map((item, index) => {
          const isSelected = index === currentIndex;
          return (
            <Box
              key={item.id}
              onClick={() => setCurrentIndex(index)}
              className={`min-w-36 mx-1 relative h-20 md:h-24 rounded-xl overflow-hidden cursor-pointer transition-all ${
                isSelected
                  ? "ring-4 ring-teal-500 scale-[0.98]"
                  : "opacity-80 hover:opacity-100"
              }`}
            >
              <img
                src={item.url}
                alt={item.alt || `Thumbnail ${index + 1}`}
                className="w-full h-full object-cover"
              />

              {/* Video Play Icon Overlay */}
              {item.type === "video" && (
                <Box className="absolute inset-0 bg-black/30 flex items-center justify-center">
                  <Box className="w-8 h-8 rounded-full bg-white flex items-center justify-center shadow-lg">
                    <PlayArrowIcon className="text-slate-900 text-lg ml-0.5" />
                  </Box>
                </Box>
              )}
            </Box>
          );
        })}
      </Box>
      {/* </DialogContent> */}
      {/* </Dialog> */}
    </React.Fragment>
  );
};
