import { useState, useEffect } from "react";
import { getUrls, createUrl, deleteUrl } from "../service/api";
import { useNavigate } from "react-router-dom";
import "./UrlsTable.css"
const UrlsTable = () => {
  const [urls, setUrls] = useState([]);
  const [newUrl, setNewUrl] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const token = localStorage.getItem("token"); // токен користувача

  useEffect(() => { // завантаження URL після рендеру
  if (token) { // тільки якщо користувач авторизований
    fetchUrls();
  }
}, [token]);

  const fetchUrls = async () => { // виклик API і збереження отриманого URL у стан urls
    try {
      const data = await getUrls(token);
      setUrls(data);
    } catch (err) {
      console.error(err);
      setError("Не вдалося завантажити URL");
    }
  };

  // Додавання нового URL
  const handleAddUrl = async (e) => {
    e.preventDefault();
    if (!newUrl) return;

    try {
      const created = await createUrl(newUrl, token);
      setUrls([...urls, created]);
      setNewUrl("");
      setError("");
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || "Помилка при додаванні URL");
    }
  };

  // Видалення URL
  const handleDelete = async (id) => {
    try {
      await deleteUrl(id, token);
      setUrls(urls.filter((url) => url.id !== id)); // фільтрація для видалення посилань зі списку
    } catch (err) {
      console.error(err);
      setError("Не вдалося видалити URL");
    }
  };

  const handleLogout = () => { // вихід і очищення токена
    localStorage.removeItem("token");
    setToken(null);
  };

  const goToRegister = () => {
    navigate("/register");
  };
  const goToLogin = () => {
    navigate("/login");
  };
  return (
    <div className="urlsTable">
      <h1>UrlService Application</h1>
      <h2>Short URLs Table</h2>

      {/* Додавання нового URL */}
      {token ? (
        <form onSubmit={handleAddUrl}>
          <input
            type="text"
            placeholder="Enter URL"
            value={newUrl}
            onChange={(e) => setNewUrl(e.target.value)}
          />
          <button type="submit">Add URL</button>
        </form>
      ) : (
        <p style={{ color: "red" }}>
          🔒 You must be logged in to add a new URL.
        </p>
      )}

      {error && <p style={{ color: "red" }}>{error}</p>}

      {/* Таблиця URL */}
      <table border="1" cellPadding="5" className="table-details-card">
        <thead>
          <tr>
            <th>Original URL</th>
            <th>Short Code</th>
            {token && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {urls.map((url) => (
            <tr key={url.id}>
              <td>
                <a href={url.originalUrl} target="_blank" rel="noopener noreferrer">
                  {url.originalUrl}
                </a>
              </td>
              <td>
                {token ? (
                  <a
                    onClick={() => navigate(`/url/${url.id}`)}
                    style={{ cursor: "pointer", color: "blue" }}
                  >
                    {url.shortCode}
                  </a>
                ) : (
                  <span style={{ color: "gray", cursor: "not-allowed" }}>
                    {url.shortCode}
                  </span>
                )}
              </td>
              {token && (
                <td>
                  <button onClick={() => handleDelete(url.id)}>Delete</button>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
      <div className="btns">
        <button onClick ={goToRegister}>Register</button>
        <button onClick ={goToLogin}>Log in</button>
        <button onClick ={handleLogout}>Log out</button>
      </div>
    </div>
  );
};

export default UrlsTable;
