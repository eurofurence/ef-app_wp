using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class WikiTextToHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is string && targetType == typeof(string))
            {
                var text = (string)value;

                // Normalize line breaks
                text = text
                    .Replace("\\\\", "<br>")
                    .Replace("\n\n", "<br><br>");

                // Preceed first item of a list with 2 linebreaks.
                text = (new Regex("^([^ ]+.*$)(\\n^)(  \\*)", RegexOptions.Multiline))
                    .Replace(text, "$1<br><br>$2$3");
                // Append last item of a list with 2 linebreaks.
                text = (new Regex("(^  \\*[^\\n]+$\\n^)(?!  \\* )", RegexOptions.Multiline))
                    .Replace(text, "$1<br><br>");
                // List item.
                text = (new Regex("^  \\* (.*)$", RegexOptions.Multiline))
                    .Replace(text, "<li>$1</li>");
                // Bold Items
                text = (new Regex("\\*\\*([^\\*]*)\\*\\*"))
                    .Replace(text, "<b>$1</b>");
                // Italics
                text = (new Regex("\\*([^\\*]*)\\*"))
                    .Replace(text, "<i>$1</i>");

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