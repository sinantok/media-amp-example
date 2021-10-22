using AutoMapper;
using Data.Mongo.Collections;
using Models.Entities;
namespace CmsApi.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Content, ContentDto>()
                .ForMember(d => d.NewsId, o => o.MapFrom(s => s.NewsId))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Spot, o => o.MapFrom(s => s.Spot))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.PublishedTime, o => o.MapFrom(s => s.PublishedTime))
                .ForMember(d => d.CreatedTime, o => o.MapFrom(s => s.CreatedTime))
                .ForMember(d => d.CreatedAccount, o => o.MapFrom(s => s.CreatedAccount))
                .ForMember(d => d.MainCategory, o => o.MapFrom(s => s.MainCategory))
                .ForMember(d => d.PublishedAccount, o => o.MapFrom(s => s.PublishedAccount))
                .ForMember(d => d.NewsKeywords, o => o.MapFrom(s => s.NewsKeywords))
                .ForMember(d => d.SourcesData, o => o.MapFrom(s => s.SourcesData))
                .ForMember(d => d.Story, o => o.MapFrom(s => s.Story))
                .ForMember(d => d.MainArtUrl, o => o.MapFrom(s => s.MainArtUrl))
                .ForMember(d => d.TemplateTarget, o => o.MapFrom(s => s.TemplateTarget));
        }
    }
}

