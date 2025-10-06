import { useState } from "react";
import { useNavigate } from "react-router-dom"; // хук-навігація для переходу на інші сторінки
import { login } from "../service/api"; // функція для запиту до api для авторизації
import './Login.css'
const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault(); // блокування перезавантаження сторінки
    try {
      const data = await login(username, password);
      localStorage.setItem("token", data.token); // зберігаємо JWT-токен у браузері
      alert(`✅ Авторізація успішна! Вітаємо, ${username}!`);
      navigate("/"); // перехід на таблицю URL
    } catch (err) {
      setError("Неправильний логін або пароль");
    }
  };

  const goToTable = () => {
    navigate("/");
  };
    const goToRegister = () => {
    navigate("/register");
  };

  return (
    <div className="login-container">
      <h1> UrlService Application</h1>
    <div className="details-card">
      <h2>Log in</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        /><br/>
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        /><br/>
        <div className="lgbtn">
          <button type="submit">Login</button>      
          <button onClick={goToRegister}>Register</button>
          <button onClick={goToTable}>Return</button>
        </div>
      </form>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
    </div>
  );
};

export default Login;
