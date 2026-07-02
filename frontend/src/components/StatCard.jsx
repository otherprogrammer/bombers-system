export default function StatCard({ label, value, sublabel, icon: Icon, color = "primary" }) {
  const colorMap = {
    primary: "text-primary border-l-primary",
    tertiary: "text-tertiary border-l-tertiary",
    secondary: "text-secondary border-l-secondary",
    outline: "text-outline border-l-outline",
  };

  return (
    <div
      className={`bg-surface-container-lowest border border-outline-variant border-l-4 p-5 shadow-sm flex items-center justify-between rounded ${colorMap[color]}`}
    >
      <div>
        <p className="text-[11px] font-bold uppercase tracking-widest text-on-surface-variant mb-1">
          {label}
        </p>
        <p className={`text-4xl font-black ${colorMap[color].split(" ")[0]}`}>
          {value}
        </p>
        {sublabel && (
          <p className="text-xs text-secondary mt-1">{sublabel}</p>
        )}
      </div>
      {Icon && <Icon className={`${colorMap[color].split(" ")[0]} opacity-30`} size={44} />}
    </div>
  );
}
