import { useEffect, useState } from "react";
import "./App.css";
import AppProviders from "./context/AppProviders";
import { AppRouter } from "./routes/AppRouter";

function App() {
  return (
    <AppProviders>
      <AppRouter />
    </AppProviders>
  );
}

export default App;
