namespace GlobalLogin.App.Repositories
{
    public class ResponseMessage<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
        public T Errors { get; set; }
        public string Message { get; set; }

        public static ResponseMessage<T> Fail(int code=500, string message = "Xəta baş verdi.", T errors = default)
        {
            return new ResponseMessage<T> { Code = code, Errors = errors, Message = message };
        }

        public static ResponseMessage<T> Success(T data = default)
        {
            return new ResponseMessage<T> { Code = 200, Data = data, Errors=default, Message = "Əməliyyat uğurla başa çatdı." };
        }

    }
}
