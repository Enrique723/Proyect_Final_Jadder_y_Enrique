// src/index.js
import React from "react";
import { createRoot } from "react-dom/client"; // Importa createRoot desde "react-dom/client"
import App from "./App";
import "bootstrap/dist/css/bootstrap.min.css"; // Importa el archivo CSS de Bootstrap

const root = document.getElementById("root");
const appRoot = createRoot(root);

appRoot.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
