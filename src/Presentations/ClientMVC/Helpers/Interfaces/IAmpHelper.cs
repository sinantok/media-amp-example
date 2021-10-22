using System.Collections.Generic;

namespace ClientMVC.Helpers.Interfaces
{
    public interface IAmpHelper
    {
        string RemoveAttributesFromTag(string text, string tagName, params string[] attributeList);
        string RemoveTags(string text, params string[] tagNameList);
        string RemoveTagsKeepInnerHtml(string text, params string[] tagNameList);
        string RemoveImageTagsWithoutSrcAttribute(string text);
        string RemoveImageTagsHaveInvalidSrcAttribute(string text);
        string RemoveInlineStyles(string text);
        string RemoveTagAttributes(string text, string tagName, params string[] attributeList);
        string FixAspTags(string text);
        string FixColTags(string text);
        string FixTableTags(string text);
        string FixAnchorAttributes(string text);
        string FixDivElements(string text);
        string FixRemainingImageTags(string text);
        List<string> GetAttributesAsList(string attributes);
        string FixAmpValidationErrors(string text);
    }
}
