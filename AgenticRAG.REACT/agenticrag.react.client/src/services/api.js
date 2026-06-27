const API_BASE = "https://localhost:7001";

export async function getWeather() {
    const res = await fetch(`${API_BASE}/weatherforecast`);
    return res.json();
}