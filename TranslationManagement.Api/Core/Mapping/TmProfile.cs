using AutoMapper;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Core.Models.Dto;

namespace TranslationManagement.Api.Core.Mapping
{
    public class TmProfile : Profile
    {
        public TmProfile()
        {
            CreateMap<TranslationJob, TranslationJobDto>();
            CreateMap<TranslationJobDto, TranslationJob>();
            CreateMap<TranslatorModel, TranslatorDto>();
            CreateMap<TranslatorDto, TranslatorModel>();
        }
    }
}
