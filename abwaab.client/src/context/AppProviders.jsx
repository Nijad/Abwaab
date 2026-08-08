import React from "react";
import { AuthProvider } from "./AuthContext";
import { NoftificationProvider } from "./NoftificationContext";
import ThemeProviderMUI from "./ThemeProviderMUI";
import NotistackProvider from "./NotistackProvider";

const AppProviders = ({ children }) => {
  return (
    <AuthProvider>
      <NoftificationProvider>
        <NotistackProvider>
          <ThemeProviderMUI>{children}</ThemeProviderMUI>
        </NotistackProvider>
      </NoftificationProvider>
    </AuthProvider>
  );
};

export default AppProviders;
