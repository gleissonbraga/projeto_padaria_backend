namespace backend.Errors
{
    public class ErroHttp : Exception
    {
        public List<string> Messages { get; set; }

        public ErroHttp(List<string> messages)
        { 
            Messages = messages;
        }
    }
}
