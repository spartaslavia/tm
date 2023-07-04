using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Data.DbContexts;
using TranslationManagement.Api.Data.Repositories.Interfaces;

namespace TranslationManagement.Api.Data.Repositories
{
    public class TranslatorRepository : ITranslatorRepository
    {
        private readonly AppDbContext _context;

        public TranslatorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<TranslatorModel>> GetTranslatorsAsync()
        {
            return await _context.Translators.ToListAsync();
        }

        public async Task<TranslatorModel> GetTranslatorAsync(int id)
        {
            return await _context.Translators.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTranslatorAsync(TranslatorModel translator)
        {
            await _context.Translators.AddAsync(translator);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateTranslatorAsync(TranslatorModel translator)
        {
            _context.Translators.Update(translator);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TranslatorModel>> GetTranslatorsByNameAsync(string name)
        {
            return await _context.Translators.Where(t => t.Name == name).ToListAsync();
        }
    }
}
