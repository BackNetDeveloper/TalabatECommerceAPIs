namespace APIsMainProject.ResponseModule
{
    public class ApiValidationErrorResponse : ApiExeption
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
