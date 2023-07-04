using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Api.Core.Models;

namespace TranslationManagement.Api.Data.Repositories.Interfaces
{
    public interface ITranslatorRepository
    {
        Task<IList<TranslatorModel>> GetTranslatorsAsync();
        Task<TranslatorModel> GetTranslatorAsync(int translatorId);
        Task<IList<TranslatorModel>> GetTranslatorsByNameAsync(string name);
        Task AddTranslatorAsync(TranslatorModel translator);
        Task UpdateTranslatorAsync(TranslatorModel translator);
        Task<int> SaveChangesAsync();
    }
}
