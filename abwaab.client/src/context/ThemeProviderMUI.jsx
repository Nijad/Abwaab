import { createTheme, ThemeProvider } from "@mui/material";
import { CacheProvider } from "@emotion/react";
import createCache from "@emotion/cache";
import { prefixer } from "stylis";
import rtlPlugin from "@mui/stylis-plugin-rtl";

const rtlCache = createCache({
  key: "muirtl",
  stylisPlugins: [prefixer, rtlPlugin],
});
const baseTheme = createTheme();
const theme = createTheme({
  palette: {
    navy: {
      main: "#0D2A4A",
      contrastText: "#ffffff",
    },
    teal: {
      main: "#087A78",
      contrastText: "#ffffff",
    },
    sky: {
      main: "#217BA7",
      contrastText: "#ffffff",
    },
    neutral: {
      main: "#172733",
      contrastText: "#ffffff",
    },
    success: {
      main: "#237A4B",
      contrastText: "#ffffff",
    },
    warning: {
      main: "#986A00",
      contrastText: "",
    },
    error: {
      main: "#B4232F",
      contrastText: "#ffffff",
    },
  },
  typography: {
    fontFamily: '"Vazirmatn", sans-serif;',
    body1: {
      fontSize: "1.2rem",
      "@media (min-width:600px)": {
        fontSize: "1.5rem",
      },
      [baseTheme.breakpoints.up("md")]: {
        fontSize: "1.0rem",
      },
    },
  },
  direction: "rtl",
});
const ThemeProviderMUI = ({ children }) => {
  return (
    <CacheProvider value={rtlCache}>
      <ThemeProvider theme={theme}>{children}</ThemeProvider>
    </CacheProvider>
  );
};

export default ThemeProviderMUI;
