namespace IdentityPlatform.Core.Common
{

    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public int StatusCode { get; set; }

        public static Result<T> Success(T data, int statusCode = 200)
            => new() { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static Result<T> Failure(string error, int statusCode = 400)
            => new() { IsSuccess = false, Errors = new List<string> { error }, StatusCode = statusCode };
    }
}
