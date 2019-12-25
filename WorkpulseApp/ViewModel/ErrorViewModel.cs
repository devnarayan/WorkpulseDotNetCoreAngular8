using System;

namespace TestGraphApi.Models
{
    public class ErrorViewModel : IDisposable
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Controller { get; set; }
        public string Action { get; set; }
        public string ErrorDetail { get; set; }
        public string ExceptionMessage { get; set; }

        public DateTime ErrorDate { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}