import { useState, useEffect } from "react";
import axios from "axios";

const API_URL = "https://localhost:7012/api/about";

const About = () => {
  const [text, setText] = useState("");
  const [editText, setEditText] = useState("");
  const [isAdmin, setIsAdmin] = useState(false); // визначаємо роль
  const [error, setError] = useState("");

  const token = localStorage.getItem("token");

  // Завантаження тексту при монтованні
  useEffect(() => {
    fetchAbout();
    checkAdmin();
    // eslint-disable-next-line
  }, []);

  const fetchAbout = async () => {
    try {
      const response = await axios.get(API_URL);
      setText(response.data.text);
      setEditText(response.data.text);
    } catch (err) {
      console.error(err);
      setError("Не вдалося завантажити опис");
    }
  };

  const checkAdmin = () => {
    // тут можна зробити реальну перевірку ролі через токен або API
    const role = localStorage.getItem("role");
    if (role === "Admin") setIsAdmin(true);
  };

  const handleSave = async () => {
    try {
      await axios.put(
        API_URL,
        { text: editText },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setText(editText);
      setError("");
    } catch (err) {
      console.error(err);
      setError("Не вдалося зберегти зміни");
    }
  };

  return (
    <div>
      <h2>About URL Shortener</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!isAdmin && <p>{text}</p>}

      {isAdmin && (
        <div>
          <textarea
            rows={10}
            cols={50}
            value={editText}
            onChange={(e) => setEditText(e.target.value)}
          />
          <br />
          <button onClick={handleSave}>Save</button>
        </div>
      )}
    </div>
  );
};

export default About;
