namespace TranslationManagement.Api.Core.Models
{
    public class TranslatorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HourlyRate { get; set; }
        public TranslatorStatuses Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
