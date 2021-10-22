using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public class ContentDto
    {
        public string NewsId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Spot { get; set; }
        public string ContentType { get; set; }
        public string Text { get; set; }
        public DateTime PublishedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public CreatedAccount CreatedAccount { get; set; }
        public MainCategory MainCategory { get; set; }
        public PublishedAccount PublishedAccount { get; set; }
        public List<NewsKeywords> NewsKeywords { get; set; }
        public List<SourcesData> SourcesData { get; set; }
        public Story Story { get; set; }
        public string MainArtUrl { get; set; }
        public string TemplateTarget { get; set; }
    }
    public class Contents
    {
        public string _t { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
    }
    public class Story
    {
        public string _id { get; set; }
        public List<Contents> Contents { get; set; }
    }
    public class MainCategory
    {
        public string Title { get; set; }
        public string Slug { get; set; }
    }
    public class NewsKeywords
    {
        public string Keyword { get; set; }
    }
    
    public class SourcesData
    {
        public string _id { get; set; }
        public string Title { get; set; }
    }
    public class PublishedAccount
    {
        public string _id { get; set; }
        public string Email { get; set; }
    }
    public class CreatedAccount
    {
        public string _id { get; set; }
        public string Email { get; set; }
    }
}
