import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../service/api"; // новий метод у api
import "./Registration.css"; // можна використати той самий стиль

const Register = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await register(username, email, password);
      alert("✅ Реєстрація успішна! Тепер увійдіть.");
      navigate("/login"); // після успішної реєстрації повертає на логін
    } catch (err) {
      console.error(err);
      setError("Не вдалося зареєструвати користувача");
    }
  };

  const goToLogin = () => {
    navigate("/login");
  };

  const goToTable = () => {
    navigate("/");
  };

  return (
    <div className="login-container">
      <h1> UrlService Application</h1>
      <div className="rgdetails-card">
        <h2>Registration</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          /><br/>
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          /><br/>
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          /><br/>
          <div className="rgbtn">
            <button type="submit">Register</button>
            <button type="button" onClick={goToLogin}>Login</button>
            <button type="button" onClick={goToTable}>Return</button>
          </div>
        </form>
        {error && <p style={{ color: "red" }}>{error}</p>}
      </div>
    </div>
  );
};

export default Register;
