import React, { useState, useRef, useEffect } from "react";
import { Typography, CircularProgress, IconButton } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import { mediaApi, propertyApi } from "../../api";
import { useSnackbar } from "notistack";
import { Close } from "@mui/icons-material";
import LabelTag from "../../components/LabelTag";
import { PROPERTY_MEDIA } from "../../dataTypes/propertis";
import MediaDelete from "./MediaDelete";

const MediaUploader = ({
  image = null,
  propertyId,
  isCover = false,
  required = false,
  mediaInfo,
  onUploaded,
  onDeleted,
}) => {
  const [isDragging, setIsDragging] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  // const [image, setUploadedImage] = useState(null);
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
      formData.append("IsCover", isCover);

      const response = await mediaApi.upload(formData);

      // setUploadedImage({
      //   id: response.data.id,
      //   filePath: `${import.meta.env.VITE_API_BASE_URL}${
      //     response.data.filePath
      //   }`,
      // });
      enqueueSnackbar(response.data.message, { variant: "success" });
      const obj = { ...PROPERTY_MEDIA };
      obj.filePath = response.data.filePath;
      obj.isCover = isCover;
      obj.mediaId = response.data.id;
      obj.mediaTypeId = mediaInfo.mediaTypeId;
      obj.mediaTypeName = mediaInfo.mediaTypeName;
      onUploaded?.(obj);
    } catch (error) {
      console.error("Upload error:", error);
      enqueueSnackbar(error.detail, { variant: "error" });
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
    if (image) {
      setTimeout(() => {
        // setUploadedImage(image);
      }, 0);
    }
    return () => {
      // setUploadedImage(null);
    };
  }, [image]);

  return (
    <React.Fragment>
      <div
        onClick={(e) => handleClick(e)}
        onDragOver={(e) => handleDragOver(e)}
        onDragLeave={(e) => handleDragLeave(e)}
        onDrop={(e) => handleDrop(e)}
        className={`md:col-span-1 ${
          !image ? "border-2 border-dashed" : ""
        } rounded-2xl p-6 flex flex-col items-center justify-center text-center cursor-pointer transition-all h-full  w-full relative overflow-hidden ${
          isDragging
            ? "border-sky-500 bg-sky-50"
            : image
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
        ) : image ? (
          <>
            <img
              src={image.filePath}
              alt="Cover"
              className="absolute inset-0 w-full h-full object-cover rounded-2xl"
            />
            <div className="absolute h-full inset-0 bg-black/10  flex flex-col items-center justify-center rounded-2xl">
              <div className="flex justify-end w-full p-3">
                {/* <IconButton
                  color="error"
                  sx={{
                    backgroundColor: "#ffffff83",
                    // position: "absolute",
                    // right: "3%",
                    // top: "3%",
                    // alignContent: "end",
                  }}
                  title="حذف الصورة"
                  className="hover:!bg-white"
                  onClick={(e) => handleDeleteImage(e)}
                >
                  <Close />
                </IconButton> */}
                <MediaDelete
                  id={image.id}
                  onDeleted={onDeleted}
                  key={image.id}
                />
              </div>
              <div className=" w-full content-center top-0  h-full">
                <CloudUploadIcon className="text-white text-4xl mb-2" />
                <Typography className="font-bold text-white mb-1">
                  تغيير صورة الغلاف
                </Typography>
                <Typography variant="caption" className="text-white/80">
                  اضغط أو اسحب صورة جديدة
                </Typography>
              </div>
              <div className="w-full px-3 py-1">
                <LabelTag
                  label={"صورة الغلاف"}
                  classes="bg-navy-600 px-3 rounded-full text-white"
                />
              </div>
            </div>
          </>
        ) : (
          <>
            <div className="w-10 h-10 rounded-full bg-white shadow flex items-center justify-center mb-3">
              <AddIcon className="text-neutral-600" />
            </div>
            <Typography className="font-bold text-neutral-800 mb-1">
              إضافة صورة {isCover && "الغلاف"}
            </Typography>
            {required && (
              <Typography variant="caption" className="text-neutral-400">
                مطلوبة
              </Typography>
            )}
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
    </React.Fragment>
  );
};

export default MediaUploader;
