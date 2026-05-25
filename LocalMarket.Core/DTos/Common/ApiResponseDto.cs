
namespace LocalMarket.Core.DTos.Common
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ApiResponseDto<T> OK(T? data, string message = "Operacion exitosa") =>
            new() {
                Success = true, Data = data, Message = message };
    }
}
