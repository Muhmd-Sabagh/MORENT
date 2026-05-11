namespace MORENT.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T? DataObject { get; private set; }
        public List<string> ErrorMessages { get ; private set; } = new List<string>();

        public static Result<T> Success(T data, string message = "")
        {
            return new Result<T>
            {
                IsSuccess = true,
                DataObject = data,
                Message = message
            };
        }

        public static Result<T> Failure(string message, List<string>? errorMessages = null)
        {
            return new Result<T>
            {
                IsSuccess = false,
                DataObject = default,
                Message = message,
                ErrorMessages = errorMessages ?? new List<string>()
            };
        }
    }
}
