using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Core.Models.Dto;
using TranslationManagement.Api.Core.Services.Interfaces;

namespace TranslationManagement.Api.Controlers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslatorManagementController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITranslatorService _translatorService;

        public TranslatorManagementController(IMapper mapper, ITranslatorService translatorService)
        {
            _mapper = mapper;
            _translatorService = translatorService;
        }

        [HttpGet("{id}")]
        public async Task<TranslatorDto> GetTranslator(int id)
        {
            var one = await _translatorService.GetTranslatorAsync(id);
            return _mapper.Map<TranslatorDto>(one);
        }

        [HttpGet]
        public async Task<IList<TranslatorDto>> GetTranslators()
        {
            var list = await _translatorService.GetTranslatorsAsync();
            return _mapper.Map<IList<TranslatorDto>>(list);
        }

        [HttpGet("name/{name}")]
        public async Task<IList<TranslatorDto>> GetTranslatorsByName(string name)
        {
            var list = await _translatorService.GetTranslatorsByNameAsync(name);
            return _mapper.Map<IList<TranslatorDto>>(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddTranslator(TranslatorModel translator)
        {
            await _translatorService.AddTranslatorAsync(translator);
            return CreatedAtAction(nameof(GetTranslator), new { id = translator.Id }, translator);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTranslatorStatus(int translatorId, TranslatorStatuses newStatus )
        {
            await _translatorService.UpdateTranslatorStatusAsync(translatorId, newStatus);
            return NoContent();
        }
    }
}