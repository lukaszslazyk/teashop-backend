namespace Teashop.Backend.UI.Api.Commons.Models
{
    public class ApiError
    {
        public string ErrorType { get; set; }
        public string Message { get; set; }
        public object Details { get; set; }
    }
}
