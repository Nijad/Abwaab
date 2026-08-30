import { Close } from "@mui/icons-material";
import { IconButton } from "@mui/material";
import { useEffect, useRef } from "react";
import { mediaApi } from "../../api";
import { useSnackbar } from "notistack";

const MediaDelete = ({ id, onDeleted }) => {
  const { enqueueSnackbar } = useSnackbar();
  const signalRef = useRef(null);
  const deleteImage = async (e) => {
    e.stopPropagation();
    if (signalRef.current) {
      signalRef.current.abort();
    }
    signalRef.current = new AbortController();
    try {
      const response = await mediaApi.delete(id, signalRef.current.signal);
      enqueueSnackbar(response.data.message, { variant: "success" });
      onDeleted?.(id);
    } catch (error) {
      console.error("Upload error:", error);
      enqueueSnackbar(error.title, { variant: "error" });
      //   onUploadError?.(error.message || "حدث خطأ أثناء رفع الصورة");
    }
    // finally {
    // }
  };
  useEffect(() => {
    return () => {
      if (signalRef.current) {
        signalRef.current.abort();
      }
    };
  }, []);
  return (
    <IconButton
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
      onClick={(e) => deleteImage(e)}
      size="small"
    >
      <Close />
    </IconButton>
  );
};

export default MediaDelete;
