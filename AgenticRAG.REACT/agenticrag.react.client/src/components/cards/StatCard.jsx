export default function StatCard({ title, value, color }) {
    return (
        <div style={{
            flex: 1,
            background: color,
            color: "white",
            padding: 20,
            borderRadius: 10
        }}>
            <h4>{title}</h4>
            <h2>{value}</h2>
        </div>
    );
}