namespace xgca.core.Response
{
    public class ErrorField
    {
        public ErrorField(string fieldId, string message)
        {
            this.FieldId = fieldId;
            this.Message = message;
        }

        public string FieldId { get; private set; }
        public string Message { get; private set; }
    }
}
