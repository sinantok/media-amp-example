using ClientMVC.Helpers.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClientMVC.Helpers
{
    public class AmpHelper : IAmpHelper
    {
        private readonly string _urlRegexPattern = @"(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+";

        private readonly ILogger<AmpHelper> _logger;

        public AmpHelper(ILogger<AmpHelper> logger)
        {
            _logger = logger;
        }

        public string RemoveAttributesFromTag(string text, string tagName, params string[] attributeList)
        {
            var returnValue = text;
            try
            {
                HtmlNode.ElementsFlags["ımg"] = HtmlElementFlag.Closed;
                HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);

                foreach (var tag in htmlDocument.DocumentNode.Descendants(tagName))
                {
                    foreach (var attribute in attributeList)
                    {
                        if (!string.IsNullOrWhiteSpace(tag.GetAttributeValue(attribute, string.Empty)))
                        {
                            tag.Attributes[attribute].Remove();
                        }
                    }
                }

                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string RemoveTags(string text, params string[] tagNameList)
        {
            var returnValue = text;
            try
            {
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);

                foreach (var tagName in tagNameList)
                {
                    IList<HtmlNode> formNodes;
                    if (!string.IsNullOrEmpty(tagName) && tagName.Contains(':'))
                    {
                        formNodes = htmlDocument.DocumentNode.Descendants().Where(n => n.Name.StartsWith(tagName)).ToList();
                    }
                    else
                    {
                        formNodes = htmlDocument.DocumentNode.SelectNodes($"//{tagName}")?.ToList();
                    }

                    if (formNodes == null)
                        continue;

                    foreach (var node in formNodes)
                    {
                        node.Remove();
                    }
                }

                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string RemoveTagsKeepInnerHtml(string text, params string[] tagNameList)
        {
            var returnValue = text;
            try
            {
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);

                foreach (var tagName in tagNameList)
                {
                    IList<HtmlNode> formNodes;
                    if (!string.IsNullOrEmpty(tagName) && tagName.Contains(':'))
                    {
                        formNodes = htmlDocument.DocumentNode.Descendants().Where(n => n.Name.StartsWith(tagName)).ToList();
                    }
                    else
                    {
                        formNodes = htmlDocument.DocumentNode.SelectNodes($"//{tagName}")?.ToList();
                    }

                    if (formNodes == null)
                        continue;

                    foreach (var node in formNodes)
                    {
                        node.ParentNode.RemoveChild(node, true);
                    }
                }

                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string RemoveImageTagsWithoutSrcAttribute(string text)
        {
            var returnValue = text;
            try
            {
                HtmlNode.ElementsFlags["ımg"] = HtmlElementFlag.Closed;
                HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.Descendants("img").ToList();
                foreach (var item in nodeCollection.Where(x => string.IsNullOrEmpty(x.GetAttributeValue("src", string.Empty))))
                {
                    item.Remove();
                }
                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string RemoveImageTagsHaveInvalidSrcAttribute(string text)
        {
            var returnValue = text;
            try
            {
                var urlRegex = new Regex(_urlRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                HtmlNode.ElementsFlags["ımg"] = HtmlElementFlag.Closed;
                HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);

                var invalidTags = from tags in htmlDocument.DocumentNode.Descendants("img").ToList()
                                  let srcAttribute = tags.GetAttributeValue("src", string.Empty)
                                  where !string.IsNullOrWhiteSpace(srcAttribute)
                                  && urlRegex.IsMatch(srcAttribute)
                                  select tags;

                foreach (var item in invalidTags)
                {
                    item.Remove();
                }

                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string RemoveInlineStyles(string text)
        {
            var returnValue = text;

            try
            {
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.ChildNodes;

                RemoveInlineStylesRecursive(nodeCollection);

                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public string RemoveTagAttributes(string text, string tagName, params string[] attributeList)
        {
            var returnValue = text;

            try
            {
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);

                var selectedTags = htmlDocument.DocumentNode.SelectNodes($"//{tagName}");
                if (selectedTags != null)
                {
                    foreach (var tag in selectedTags)
                    {
                        if (attributeList == null)
                        {
                            tag.Attributes.RemoveAll();
                        }
                        else
                        {
                            foreach (var attribute in attributeList)
                            {
                                tag.Attributes.Remove(attribute);
                            }
                        }
                    }

                    returnValue = htmlDocument.DocumentNode.OuterHtml;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public string FixAspTags(string text)
        {
            try
            {
                text = Regex.Replace(text, "<![^>]+>", string.Empty, RegexOptions.Singleline, TimeSpan.FromMilliseconds(10));
                text = Regex.Replace(text, "<\\?[^>]+>", string.Empty, RegexOptions.Singleline, TimeSpan.FromMilliseconds(10));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return text;
        }

        public string FixColTags(string text)
        {
            var returnValue = text;
            try
            {
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.Descendants("col").ToList();
                if (nodeCollection.Any())
                {
                    List<string> attributeList = GetAttributesAsList("width");

                    foreach (HtmlNode node in nodeCollection.ToList())
                    {
                        foreach (HtmlAttribute attribute in node.Attributes.ToList())
                        {
                            if (attributeList.Contains(attribute.Name))
                            {
                                attribute.Remove();
                            }
                        }
                    }
                }
                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string FixTableTags(string text)
        {
            var returnValue = text;
            try
            {
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.Descendants("table").ToList();
                if (nodeCollection.Any())
                {
                    foreach (HtmlNode node in nodeCollection.ToList())
                    {
                        node.Attributes["cols"]?.Remove();
                        node.Attributes["border"]?.Remove();
                    }
                }
                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string FixAnchorAttributes(string text)
        {
            var returnValue = text;
            try
            {
                var urlRegex = new Regex(_urlRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline, TimeSpan.FromMilliseconds(10));
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.Descendants("a").ToList();
                if (nodeCollection.Any())
                {
                    List<string> attributeList = GetAttributesAsList("id;class;style;href;target");
                    foreach (HtmlNode node in nodeCollection.ToList())
                    {
                        foreach (HtmlAttribute attribute in node.Attributes.ToList())
                        {
                            if (attributeList.Contains(attribute.Name))
                            {
                                if (string.IsNullOrEmpty(node.Attributes["href"]?.Value))
                                {
                                    node.Attributes["href"]?.Remove();
                                }
                                else
                                {
                                    try
                                    {
                                        var url = node.Attributes["href"].Value;
                                        if (string.IsNullOrWhiteSpace(url) || !url.Contains("http"))
                                            continue;

                                        var result = urlRegex.Match(url);
                                        if (result.Success)
                                        {
                                            node.Attributes["href"].Value = result.Groups[0].Value.Trim();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message, ex);
                                    }
                                }
                            }
                            else
                            {
                                attribute.Remove();
                            }
                        }
                    }
                }
                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string FixDivElements(string text)
        {
            var returnValue = text;
            try
            {
                HtmlNode.ElementsFlags["dıv"] = HtmlElementFlag.Closed;
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.Descendants("dıv").ToList();
                if (nodeCollection.Any())
                {
                    foreach (HtmlNode node in nodeCollection)
                    {
                        node.Name = "div";
                    }
                }
                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public string FixRemainingImageTags(string text)
        {
            var returnValue = text;
            try
            {
                HtmlNode.ElementsFlags["ımg"] = HtmlElementFlag.Closed;
                HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
                HtmlDocument htmlDocument = new HtmlDocument { OptionAutoCloseOnEnd = true };
                htmlDocument.LoadHtml(text);
                var nodeCollection = htmlDocument.DocumentNode.Descendants("img").ToList();
                if (nodeCollection.Any())
                {
                    foreach (HtmlNode node in nodeCollection.ToList())
                    {
                        node.Name = "amp-img";

                        node.Attributes["border"]?.Remove();

                        if (node.Attributes["height"] != null && node.Attributes["width"] != null)
                        {
                            node.Attributes.Add("layout", "responsive");
                        }
                        else
                        {
                            if (node.Attributes["width"] != null)
                            {
                                node.Attributes["width"].Value = "auto";
                            }
                            else
                            {
                                node.Attributes.Add("width", "auto");
                            }
                            node.Attributes.Add("height", node.Attributes["height"] == null ? "320" : node.Attributes["height"].Value);
                            node.Attributes.Add("layout", "fixed-height");

                            if (node.Attributes["class"] != null)
                            {
                                node.Attributes["class"].Value = $"{node.Attributes["class"].Value} amp-unknown-size";
                            }
                            else
                            {
                                node.Attributes.Add("class", "amp-unknown-size");
                            }


                            if (node.Attributes["tabindex"] != null)
                            {
                                node.Attributes["tabindex"].Value = $"{node.Attributes["tabindex"].Value}0";
                            }
                            else
                            {
                                node.Attributes.Add("tabindex", "0");
                            }

                            if (node.Attributes["role"] != null)
                            {
                                node.Attributes["role"].Value = $"{node.Attributes["role"].Value}button";
                            }
                            else
                            {
                                node.Attributes.Add("role", "button");
                            }

                            if (node.Attributes["on"] != null)
                            {
                                node.Attributes["on"].Value = $"{node.Attributes["on"].Value}tap:lightbox1";
                            }
                            else
                            {
                                node.Attributes.Add("on", "tap:lightbox1");
                            }
                        }

                        node.Attributes["align"]?.Remove();
                    }
                }
                returnValue = htmlDocument.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public List<string> GetAttributesAsList(string attributes)
        {
            if (!string.IsNullOrWhiteSpace(attributes))
            {
                char[] splitOperators = { ',', ';' };
                return new List<string>(attributes.Split(splitOperators, StringSplitOptions.RemoveEmptyEntries));

            }
            return null;
        }

        private void RemoveInlineStylesRecursive(HtmlNodeCollection nodeCollection)
        {
            foreach (HtmlNode node in nodeCollection.ToList())
            {
                foreach (HtmlAttribute attribute in node.Attributes.ToList())
                {
                    if (attribute.Name.ToLower(CultureInfo.CreateSpecificCulture("tr-TR")) == "style")
                    {
                        attribute.Remove();
                    }
                }

                if (node.ChildNodes.Any())
                {
                    RemoveInlineStylesRecursive(node.ChildNodes);
                }
            }
        }

        //AmpValidator
        public string FixAmpValidationErrors(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            string returnValue = text;
            try
            {
                returnValue = RemoveTags(returnValue, "form", "w:wrap", "v:shapetype", "v:shape", "v:path", "o:lock", "v:imagedata");
                
                returnValue = RemoveTagsKeepInnerHtml(returnValue, "u3:", "u1:", "o:p", "table3", "table1", "linkz");
                
                returnValue = RemoveImageTagsWithoutSrcAttribute(returnValue);
                
                returnValue = RemoveImageTagsHaveInvalidSrcAttribute(returnValue);
                
                returnValue = RemoveAttributesFromTag(returnValue, "input", "onclick");
                
                returnValue = FixRemainingImageTags(returnValue);
                
                returnValue = FixDivElements(returnValue);
                
                returnValue = FixAnchorAttributes(returnValue);
                
                returnValue = FixTableTags(returnValue);
                
                returnValue = FixAspTags(returnValue);
                
                returnValue = FixColTags(returnValue);
                
                returnValue = RemoveInlineStyles(returnValue);
                
                var cleanTag = new Dictionary<string, string[]>
                {
                    {"br", null},
                    {"p", new [] {"sizcache", "sizset"}},
                    {"strong", new []{"sizcache", "sizset"}},
                    {"tr",new []{ "sizcache", "sizset" }},
                    {"td",new []{ "sizcache", "sizset" }},
                    {"tbody",new []{ "sizcache", "sizset" }},
                    {"table",new []{ "sizcache", "sizset" }},
                    {"amp-img",new []{ "vspace", "hspace" }},
                    {"span", new []{"times", "new"}},
                    {"ul", new []{ "type" }}
                };

                foreach (var tag in cleanTag)
                {
                    returnValue = RemoveTagAttributes(returnValue, tag.Key, tag.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }
    }
}
