namespace APIsMainProject.ResponseModule
{
    // Base Response Consist Of =>[StatuseCode + Message]
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromTheStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetMessageFromTheStatusCode(int statuscode)
        {
            // This Shape Of Swith Called Match Pattern
            return statuscode switch
            {
                400 => "You Made A Bad Request",
                401 => "You Are Not Authorized",
                404 => "Resources Not Found",
                500 => "Internal Server Error",
                _=>null
            };
        }
    }
}
