import { SnackbarProvider } from "notistack";

const NotistackProvider = ({ children }) => {
  return <SnackbarProvider>{children}</SnackbarProvider>;
};

export default NotistackProvider;
