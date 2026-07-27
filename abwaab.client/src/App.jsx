import { useEffect, useState } from "react";
import "./App.css";
import AppProviders from "./context/AppProviders";
import { AppRouter } from "./routes/AppRouter";

function App() {
  return (
    <AppProviders>
      <AppRouter />
      <div>
        <h1 id="" className="text-blue-900 text-3xl font-bold underline">
          ABWAAB For Real Estates Trading
        </h1>
        <h4 className="text-3xl">COMING SOON!</h4>
      </div>
    </AppProviders>
  );
}

export default App;
