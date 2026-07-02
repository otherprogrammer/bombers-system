const palettes = {
  green: "bg-green-100 text-green-800 border-green-200",
  red: "bg-red-100 text-red-700 border-red-200",
  yellow: "bg-yellow-100 text-yellow-800 border-yellow-200",
  blue: "bg-blue-100 text-blue-800 border-blue-200",
  gray: "bg-gray-100 text-gray-600 border-gray-200",
};

export default function Badge({ children, color = "gray" }) {
  return (
    <span
      className={`inline-flex items-center px-2.5 py-1 rounded-full text-[11px] font-bold uppercase border ${palettes[color]}`}
    >
      {children}
    </span>
  );
}
