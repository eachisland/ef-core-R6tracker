using System.Net;

namespace R6tracker.Util.Logger
{
    public class LogModel
    {
        public LogModel()
        {
            Id = Guid.NewGuid().ToString();
            Date = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string Message { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public DateTime Date { get; private set; }
        public HttpStatusCode HttpStatus { get; set; } = HttpStatusCode.OK;

        public override string ToString()
        {
            return $"{HttpStatus} - {Date}/{ServiceName}({MethodName}) - {Message} {Id}";
        }
    }
}
