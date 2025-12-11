namespace SistemaGestionAgricola.Models.DTOs.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
    
    public class ApiResponse
    {
        public static ApiResponse<T> SuccessResponse<T>(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }
        
        public static ApiResponse<T> ErrorResponse<T>(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }
        
        public static ApiResponse<object> SuccessResponse(string message = "Operación exitosa")
        {
            return new ApiResponse<object>
            {
                Success = true,
                Message = message,
                Data = null,
                Timestamp = DateTime.UtcNow
            };
        }
        
        public static ApiResponse<object> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}