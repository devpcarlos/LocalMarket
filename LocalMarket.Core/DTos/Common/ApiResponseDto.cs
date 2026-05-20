namespace LocalMarket.Core.DTos.Common
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public string Message { get; init; }
        public IEnumerable<string>? Errors { get; init; }

        private ApiResponseDto(bool success, T? data, string message, IEnumerable<string>? errors)
        {
            Success = success;
            Data = data;
            Message = message;
            Errors = errors;
        }

        public static ApiResponseDto<T> Ok(T data, string message = "Operación exitosa") =>
            new(true, data, message, null);

        public static ApiResponseDto<T> Fail(string message, IEnumerable<string>? errors = null) =>
            new(false, default, message, errors);
    }
}
