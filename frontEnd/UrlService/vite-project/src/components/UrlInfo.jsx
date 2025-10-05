/*import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getUrlById } from "../service/api";

const UrlInfo = () => {
  const { id } = useParams(); // беремо id з URL
  const [url, setUrl] = useState(null);
  const [error, setError] = useState("");

  const token = localStorage.getItem("token"); // токен для авторизації

  useEffect(() => {
    fetchUrl();
    // eslint-disable-next-line
  }, [id]);

  const fetchUrl = async () => {
    try {
      const data = await getUrlById(id, token);
      setUrl(data);
    } catch (err) {
      console.error(err);
      setError("Не вдалося завантажити деталі URL");
    }
  };

  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!url) return <p>Завантаження...</p>;

  return (
    <div>
      <h2>URL Details</h2>
      <p>
        <strong>Original URL:</strong>{" "}
        <a href={url.originalUrl} target="_blank" rel="noopener noreferrer">
          {url.originalUrl}
        </a>
      </p>
      <p>
        <strong>Short Code:</strong> {url.shortCode}
      </p>
      <p>
        <strong>Created By:</strong> {url.createdBy}
      </p>
      <p>
        <strong>Created Date:</strong> {new Date(url.createdDate).toLocaleString()}
      </p>
      {/* Тут можна додати інші поля, якщо вони є в API */ /*} 
      
    </div>
  );
};

export default UrlInfo;*/

import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getUrlById } from "../service/api";
import { redirectToOriginal } from "../service/api";
import { useNavigate } from "react-router-dom"; // <-- додано useNavigate
import "./UrlInfo.css"
const UrlInfo = () => {
  const { id } = useParams(); // беремо id з URL
  const [url, setUrl] = useState(null);
  const [error, setError] = useState("");
  const navigate = useNavigate(); // <-- додано
  const token = localStorage.getItem("token"); // токен для авторизації
  //const API_URL = "https://localhost:7012/api"; 

  useEffect(() => {
    fetchUrl();
    // eslint-disable-next-line
  }, [id]);

  const goToTable = () => {
    navigate("/");
  };
  const fetchUrl = async () => {
    try {
      const data = await getUrlById(id, token);
      setUrl(data);
    } catch (err) {
      console.error(err);
      setError("Не вдалося завантажити деталі URL");
    }
  };

  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!url) return <p>Завантаження...</p>;

  return (
    <div>
      <h1> UrlService Application</h1>
      <h2>Short URLs Details Table</h2>
      <table border="1" cellPadding="5" className="dtils-card">
        <thead>
          <tr>
            <th>Original URL</th>
            <th>Short Code</th>
            <th>Created By</th>
            <th>Created Date</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
              <a href={url.originalUrl} target="_blank" rel="noopener noreferrer">
                {url.originalUrl}
              </a>
            </td>
            <td>
              <a
                href="#"
                onClick={(e) => {
                  e.preventDefault();
                  redirectToOriginal(url.shortCode); // викликаємо функцію з api.js
                }}
              >
                {url.shortCode}
              </a>
            </td>
            <td>{url.createdBy}</td>
            <td>{new Date(url.createdDate).toLocaleString()}</td>
          </tr>
        </tbody>
      </table>
      <div className="tblbtn">
        <button onClick={goToTable}>Return</button>
      </div>
    </div>
  );
};

export default UrlInfo;

