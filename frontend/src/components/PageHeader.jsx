export default function PageHeader({ title, subtitle, action }) {
  return (
    <div className="flex flex-col md:flex-row md:items-end justify-between gap-4 mb-6">
      <div>
        <h2 className="text-3xl font-black text-on-surface uppercase tracking-tight">
          {title}
        </h2>
        {subtitle && <p className="text-secondary mt-1">{subtitle}</p>}
      </div>
      {action}
    </div>
  );
}
