import axios from "axios"; // бібліотека для відправки запитів (CRUD)

const API_URL = "https://localhost:7012/api"; // порт API

// Авторизація
export const login = async (userName, password) => {
  const response = await axios.post( // передача за портом методу дані у форматі JSON
    `${API_URL}/account/login`, 
    { userName, password },
    { headers: { "Content-Type": "application/json" } } // наочний приклад формату передачі JSON
  );
  return response.data; // повернення токена, імені та ролі користувача
};

// Отримати всі URL
export const getUrls = async (token) => {
  const response = await axios.get(`${API_URL}/urls`, {
    headers: { Authorization: `Bearer ${token}` } // перевірка токена у AddJwtBearer() на валідність
  });
  return response.data;
};

// Створити новий URL
export const createUrl = async (originalUrl, token) => {
  const response = await axios.post(
    `${API_URL}/urls`,
    { originalUrl },
    { headers: { Authorization: `Bearer ${token}` } }
  );
  return response.data;
};

// Видалити URL
export const deleteUrl = async (id, token) => {
  const response = await axios.delete(`${API_URL}/urls/${id}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

// Отримати URL за id
export const getUrlById = async (id, token) => {
  const response = await axios.get(`${API_URL}/urls/${id}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

// Регістрація на сервері
export const register = async (username, email, password) => {
  const response = await axios.post(`${API_URL}/account/register`, {
    username,
    email,
    password,
  });
  return response.data;
};

// Перехід на сайт за скороченим посиланням
export const redirectToOriginal = (shortCode) => {
  const url = `${API_URL}/urls/go/${shortCode}`;
  window.open(url, "_blank"); // відкриває сайт за посиланням у новій вкладці
};