namespace Filmify.Application.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data) => new ApiResponse<T> { Data = data, Success = true };
    public static ApiResponse<T> Fail(string message) => new ApiResponse<T> { Data = default, Success = false, Message = message };
}
