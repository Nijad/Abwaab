import React from "react";
import { AuthProvider } from "./AuthContext";
import { NoftificationProvider } from "./NoftificationContext";

const AppProviders = ({ children }) => {
  return (
    <AuthProvider>
      <NoftificationProvider>{children}</NoftificationProvider>
    </AuthProvider>
  );
};

export default AppProviders;
