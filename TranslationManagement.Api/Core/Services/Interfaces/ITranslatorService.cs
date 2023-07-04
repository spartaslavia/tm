using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Api.Core.Models;

namespace TranslationManagement.Api.Core.Services.Interfaces
{
    public interface ITranslatorService
    {
        Task<TranslatorModel> GetTranslatorAsync(int id);
        Task<IList<TranslatorModel>> GetTranslatorsByNameAsync(string name);
        Task<IList<TranslatorModel>> GetTranslatorsAsync();
        Task<TranslatorModel> AddTranslatorAsync(TranslatorModel translator);
        Task UpdateTranslatorStatusAsync(int translatorId, TranslatorStatuses newStatus);
    }
}