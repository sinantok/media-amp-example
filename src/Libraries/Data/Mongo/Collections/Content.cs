using Data.Mongo.Attributes;
using Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Data.Mongo.Collections
{
    [BsonCollection("contents")]
    [BsonIgnoreExtraElements]
    public class Content : MongoBaseDocument
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
}