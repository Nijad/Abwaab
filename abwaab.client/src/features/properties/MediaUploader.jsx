import React, { useState, useRef, useEffect } from "react";
import { Typography, CircularProgress } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import { mediaApi, propertyApi } from "../../api";
import { useSnackbar } from "notistack";

const MediaUploader = ({
  imageUrl = null,
  propertyId,
  mediaInfo,
  onSuccess,
}) => {
  const [isDragging, setIsDragging] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  const [uploadedImage, setUploadedImage] = useState(null);
  const { enqueueSnackbar } = useSnackbar();
  const fileInputRef = useRef(null);
  const signalRef = useRef(null);

  const ALLOWED_TYPES = ["image/png", "image/jpeg", "image/jpg", "image/webp"];
  const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB

  const handleDragOver = (e) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(true);
  };

  const handleDragLeave = (e) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
  };

  const validateFile = (file) => {
    if (!ALLOWED_TYPES.includes(file.type)) {
      return "نوع الملف غير مدعوم. يرجى اختيار صورة بصيغة PNG, JPG, JPEG, أو WEBP";
    }
    if (file.size > MAX_FILE_SIZE) {
      return "حجم الملف كبير جداً. الحد الأقصى 5 ميجابايت";
    }
    return null;
  };

  const uploadFile = async (file) => {
    const validationError = validateFile(file);
    if (validationError) {
      //   onUploadError?.(validationError);
      enqueueSnackbar(validationError, { variant: "error" });
      return;
    }
    setIsUploading(true);
    try {
      const formData = new FormData();
      formData.append("File", file);
      formData.append("PropertyId", propertyId);
      formData.append("MediaTypeId", mediaInfo.mediaTypeId);
      formData.append("MediaTypeName", mediaInfo.mediaTypeName);
      formData.append("IsCover", true);

      const response = await mediaApi.upload(formData);

      setUploadedImage(URL.createObjectURL(file));
      enqueueSnackbar(response.data.message, { variant: "success" });
      onSuccess?.(response.data);
    } catch (error) {
      console.error("Upload error:", error);
      enqueueSnackbar(error.title, { variant: "error" });
      //   onUploadError?.(error.message || "حدث خطأ أثناء رفع الصورة");
    } finally {
      setIsUploading(false);
    }
  };

  const handleDrop = (e) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
    const files = e.dataTransfer.files;
    if (files && files.length > 0) {
      const file = files[0]; // Only take the first file
      uploadFile(file);
    }
  };

  const handleFileSelect = (e) => {
    const files = e.target.files;
    if (files && files.length > 0) {
      const file = files[0];
      uploadFile(file);
    }
  };

  const handleClick = () => {
    fileInputRef.current?.click();
  };

  useEffect(() => {
    if (imageUrl) {
      setTimeout(() => {
        setUploadedImage(imageUrl);
      }, 0);
    }
  }, [imageUrl]);

  return (
    <div
      onClick={handleClick}
      onDragOver={handleDragOver}
      onDragLeave={handleDragLeave}
      onDrop={handleDrop}
      className={`md:col-span-1 ${
        !uploadedImage ? "border-2 border-dashed" : ""
      } rounded-2xl p-6 flex flex-col items-center justify-center text-center cursor-pointer transition-all h-full min-h-[300px] w-full relative overflow-hidden ${
        isDragging
          ? "border-sky-500 bg-sky-50"
          : uploadedImage
          ? "border-green-300 bg-green-50"
          : "border-sky-300 bg-neutral-50 hover:bg-neutral-100"
      }`}
    >
      <input
        ref={fileInputRef}
        type="file"
        accept="image/png,image/jpeg,image/jpg,image/webp"
        onChange={handleFileSelect}
        className="hidden"
      />

      {isUploading ? (
        <div className="flex flex-col items-center gap-3">
          <CircularProgress size={40} className="text-sky-500" />
          <Typography className="font-bold text-neutral-800">
            جاري الرفع...
          </Typography>
        </div>
      ) : uploadedImage || imageUrl ? (
        <>
          <img
            src={uploadedImage}
            alt="Cover"
            className="absolute inset-0 w-full h-full object-cover rounded-2xl"
          />
          <div className="absolute inset-0 bg-black/10 hover:bg-black/40 flex flex-col items-center justify-center rounded-2xl">
            <CloudUploadIcon className="text-white text-4xl mb-2" />
            <Typography className="font-bold text-white mb-1">
              تغيير صورة الغلاف
            </Typography>
            <Typography variant="caption" className="text-white/80">
              اضغط أو اسحب صورة جديدة
            </Typography>
          </div>
        </>
      ) : (
        <>
          <div className="w-10 h-10 rounded-full bg-white shadow flex items-center justify-center mb-3">
            <AddIcon className="text-neutral-600" />
          </div>
          <Typography className="font-bold text-neutral-800 mb-1">
            إضافة صورة الغلاف
          </Typography>
          <Typography variant="caption" className="text-neutral-400">
            مطلوبة
          </Typography>
          {isDragging && (
            <Typography
              variant="caption"
              className="text-sky-600 mt-2 font-medium"
            >
              أفلت الصورة هنا
            </Typography>
          )}
        </>
      )}
    </div>
  );
};

export default MediaUploader;
