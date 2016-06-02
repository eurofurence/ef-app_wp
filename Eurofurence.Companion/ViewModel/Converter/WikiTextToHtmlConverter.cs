using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class WikiTextToHtmlConverter : IValueConverter
    {

        private static Regex _regexPreceedFirstListItemWithLineBreaks = new Regex("^(?!  \\* )([^\\n]+)(\\n^)(  \\*)", RegexOptions.Multiline);
        private static Regex _regexSucceedLastListItemWithLineBreaks = new Regex("(^  \\*[^\\n]+$\\n^)(?!  \\* )", RegexOptions.Multiline);
        private static Regex _regexParseListItems = new Regex("^  \\* (.*)$", RegexOptions.Multiline);
        private static Regex _regexBoldItems = new Regex("\\*\\*([^\\*]*)\\*\\*");
        private static Regex _regexItalics = new Regex("\\*([^\\*]*)\\*");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is string && targetType == typeof(string))
            {
                var text = (string)value;

                // Normalize line breaks
                text = text
                    .Replace("\\\\", "<br>")
                    .Replace("\n\n", "<br><br>");

                text = _regexPreceedFirstListItemWithLineBreaks.Replace(text, "$1<br><br>$2$3");
                text = _regexSucceedLastListItemWithLineBreaks.Replace(text, "$1<br><br>");
                text = _regexParseListItems.Replace(text, "<li>$1</li>");
                text = _regexBoldItems.Replace(text, "<b>$1</b>");
                text = _regexItalics.Replace(text, "<i>$1</i>");

                return text;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}