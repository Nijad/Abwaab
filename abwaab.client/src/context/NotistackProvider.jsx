import styled from "@emotion/styled";
import { SnackbarProvider, MaterialDesignContent } from "notistack";

const StyledMaterialDesignContent = styled(MaterialDesignContent)(() => ({
  "&.notistack-MuiContent-success": { backgroundColor: "#237A4B" },
  "&.notistack-MuiContent-error": { backgroundColor: "#B4232F" },
  "&.notistack-MuiContent-info": { backgroundColor: "#3598C9" },
  "&.notistack-MuiContent-warning": { backgroundColor: "#986A00" },
}));
const NotistackProvider = ({ children }) => {
  return (
    <SnackbarProvider
      anchorOrigin={{ horizontal: "center", vertical: "top" }}
      Components={{
        success: StyledMaterialDesignContent,
        error: StyledMaterialDesignContent,
      }}
    >
      {children}
    </SnackbarProvider>
  );
};

export default NotistackProvider;
