namespace Heren.Localization.Demo.Api.ViewModels.Response
{
    public class ErrorViewModel
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public ErrorViewModel() : this(null, null)
        {
        }

        public ErrorViewModel(string errorMessage) : this(errorMessage, null)
        {
        }

        public ErrorViewModel(string errorMessage, string errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}