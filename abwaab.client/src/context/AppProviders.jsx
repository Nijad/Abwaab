import React from "react";
import { AuthProvider } from "./AuthContext";
import { NoftificationProvider } from "./NoftificationContext";
import ThemeProviderMUI from "./ThemeProviderMUI";
import NotistackProvider from "./NotistackProvider";
import AxiosInterceptor from "./AxiosInterceptor";

const AppProviders = ({ children }) => {
  return (
    <AuthProvider>
      <AxiosInterceptor>
        <NoftificationProvider>
          <NotistackProvider>
            <ThemeProviderMUI>{children}</ThemeProviderMUI>
          </NotistackProvider>
        </NoftificationProvider>
      </AxiosInterceptor>
    </AuthProvider>
  );
};

export default AppProviders;
