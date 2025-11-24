namespace Cartify.Application.Contracts.Admin
{
    public class ApiResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}


