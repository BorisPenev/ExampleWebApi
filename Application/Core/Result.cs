namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }

        public string Error { get; set; }

        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };

        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };

        public static Result<T> Failure(IEnumerable<string> errors) => new Result<T> { IsSuccess = false, Errors = errors };
    }
}
