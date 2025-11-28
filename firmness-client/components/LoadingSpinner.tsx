const LoadingSpinner = () => {
  return (
    <div className="min-h-screen flex flex-col justify-center items-center bg-gray-50">
      <div className="animate-spin rounded-full h-16 w-16 border-t-4 border-b-4 border-blue-600"></div>
      <p className="mt-4 text-lg text-gray-600">Cargando...</p>
    </div>
  );
};

export default LoadingSpinner;
