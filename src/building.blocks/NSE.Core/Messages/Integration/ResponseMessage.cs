using FluentValidation.Results;

namespace NSE.Core.Messages.Integration
{
    public class ResponseMessage : Messagens
    {
        public ValidationResult ValidationResult { get; set; }

        public ResponseMessage(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}