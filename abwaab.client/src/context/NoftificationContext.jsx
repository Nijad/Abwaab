import React, { createContext, useState } from "react";

const NotificationContext = createContext({});
export const NoftificationProvider = ({ children }) => {
  const [notification, setNotification] = useState();
  return (
    <NotificationContext.Provider value={{ notification, setNotification }}>
      {children}
    </NotificationContext.Provider>
  );
};

export default NotificationContext;
