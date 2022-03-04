namespace Cativesoft.Utilities.Responses
{
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse()
        {

        }

        public ApiResponse(bool isSuccess = true, string message = null, string errorCode = null)
            : base(null, isSuccess, message, errorCode)
        {
        }
    }

    public class ApiResponse<TData>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public TData Data { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(TData data, bool isSuccess = true, string message = null, string errorCode = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCode;
            Data = data;
        }
    }
}
