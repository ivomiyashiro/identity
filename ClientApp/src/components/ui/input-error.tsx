export const InputError = ({ error }: { error: string | undefined }) => {
  if (!error) return null;

  return <p className="text-red-500 text-sm">{error}</p>;
};
