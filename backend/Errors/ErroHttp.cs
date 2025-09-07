namespace backend.Errors
{
    public class ErroHttp : Exception
    {
        public List<ErrorDetalhe> Errors { get; set; }

        public ErroHttp(List<ErrorDetalhe> errors)
        {
            Errors = errors;
        }
    }

    public class ErrorDetalhe
    {
        public string Message { get; set; }

        public ErrorDetalhe(string message)
        {
            Message = message;
        }
    }
}
