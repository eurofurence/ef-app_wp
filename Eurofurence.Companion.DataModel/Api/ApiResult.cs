namespace Eurofurence.Companion.DataModel.Api
{
    public class ApiResult<T>
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public T Value { get; set; }

        public static explicit operator T(ApiResult<T> r) => r.Value;
        public static explicit operator bool(ApiResult<T> r) => r.IsSuccessful;
    }
}
