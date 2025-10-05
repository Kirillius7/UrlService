import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./components/Login";
import UrlsTable from "./components/UrlsTable";
import UrlInfo from "./components/UrlInfo";
import Register from "./components/Registration";


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<UrlsTable />} />
        <Route path="/login" element={<Login />} />
        <Route path="/url/:id" element={<UrlInfo />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </Router>
  );
}

export default App;