namespace ExpenseTrackingApp.DataTransferObjects.Responses
{
    public class BaseResponseModel<T>
    {
        public bool Succeeded { get; protected set; }
        public T Data { get; protected set; }
        public string Message { get; protected set; }
        public List<string> Errors { get; set; }
        public BaseResponseModel() { }
        public BaseResponseModel(T data, string message)
        {
            Succeeded = true;
            Data = data;
            Message = message;
        }
        public BaseResponseModel(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public static BaseResponseModel<T> Success()
        {
            var result = new BaseResponseModel<T> { Succeeded = true };
            return result;
        }
        public static BaseResponseModel<T> Success(string message)
        {
            var result = new BaseResponseModel<T> { Succeeded = true, Message = message };
            return result;
        }
        public static BaseResponseModel<T> Success(T data, string message)
        {
            var result = new BaseResponseModel<T> { Succeeded = true, Data = data, Message = message };
            return result;
        }
        public static BaseResponseModel<T> Success(T data)
        {
            var result = new BaseResponseModel<T> { Succeeded = true, Data = data, Message = "Successful" };
            return result;
        }
        public static BaseResponseModel<T> Fail()
        {
            var result = new BaseResponseModel<T> { Succeeded = false };
            return result;
        }
        public static BaseResponseModel<T> Fail(string message)
        {
            var result = new BaseResponseModel<T> { Succeeded = false, Message = message };
            return result;
        }
    }
}
