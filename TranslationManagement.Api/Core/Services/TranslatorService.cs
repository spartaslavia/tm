using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Core.Services.Interfaces;
using TranslationManagement.Api.Data.Repositories.Interfaces;

namespace TranslationManagement.Api.Core.Services
{
    public class TranslatorService : ITranslatorService
    {
        private readonly ITranslatorRepository _translatorRepository;

        public TranslatorService(ITranslatorRepository translatorRepository)
        {
            _translatorRepository = translatorRepository;
        }

        public async Task<TranslatorModel> GetTranslatorAsync(int id)
        {
            return await _translatorRepository.GetTranslatorAsync(id);
        }

        public async Task<IList<TranslatorModel>> GetTranslatorsAsync()
        {
            return await _translatorRepository.GetTranslatorsAsync();
        }
        
        public async Task<IList<TranslatorModel>> GetTranslatorsByNameAsync(string name)
        {
            return await _translatorRepository.GetTranslatorsByNameAsync(name);
        }

        public async Task<TranslatorModel> AddTranslatorAsync(TranslatorModel translator)
        {
            await _translatorRepository.AddTranslatorAsync(translator);
            await _translatorRepository.SaveChangesAsync();

            return translator;
        }

        public async Task UpdateTranslatorStatusAsync(int translatorId, TranslatorStatuses newStatus)
        {
            if (!Enum.IsDefined(typeof(JobStatus), newStatus))
            {
                throw new ArgumentException("Invalid status.");
            }

            var translator = await _translatorRepository.GetTranslatorAsync(translatorId);
            translator.Status = newStatus;
            await _translatorRepository.SaveChangesAsync();
        }
    }
}
