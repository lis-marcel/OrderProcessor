namespace OrderProcessor.Common
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public static OperationResult Succeeded(string message = "Operation successful", object? data = null)
        {
            return new OperationResult { Success = true, Message = message, Data = data};
        }

        public static OperationResult Failed(string message = "Operation failed")
        {
            return new OperationResult { Success = false, Message = message };
        }
    }
}
