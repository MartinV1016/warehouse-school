namespace WarehouseAPI.DTOs
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Request completed succesfully.")
        {
            return new ApiResponse<T> { IsSuccess = true, Data = data, Message = message };

        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T> { IsSuccess = false, Data = default, Message = message };
        }
    }
}
