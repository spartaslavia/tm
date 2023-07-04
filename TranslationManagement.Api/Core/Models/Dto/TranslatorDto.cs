namespace TranslationManagement.Api.Core.Models.Dto
{
    public class TranslatorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double HourlyRate { get; set; }
        public TranslatorStatuses Status { get; set; }
    }
}
