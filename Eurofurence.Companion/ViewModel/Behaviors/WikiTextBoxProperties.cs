using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;

namespace Eurofurence.Companion.ViewModel.Behaviors
{
    public static class WikiTextBoxProperties
    {
        public static string GetWikiText(TextBlock wb)
        {
            return wb.GetValue(WikiTextProperty) as string;
        }
        public static void SetWikiText(TextBlock wb, string html)
        {
            wb.SetValue(WikiTextProperty, html);
        }

        public static readonly DependencyProperty WikiTextProperty =
            DependencyProperty.RegisterAttached("WikiText", typeof(string), typeof(WikiTextBoxProperties), new PropertyMetadata("", OnWikiTextChanged));

        private static void OnWikiTextChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var txtBox = depObj as TextBlock;
            if (txtBox == null)
                return;
            if (!(e.NewValue is string))
                return;


            var html = e.NewValue as string;

            while (html.Contains("\r\n\r\n"))
            {
                html = html.Replace("\r\n\r\n", "\r\n");
            }

            var lines = html.Split(new char[] { '\n' });

            int i = 0;
            foreach (var line in lines)
            {
                i++;
                var run = new Run() { Text = line };
                if (line.StartsWith("#"))
                {
                    int strength = 1;
                    while (line.Length > strength)
                    {
                        if (line[strength] != '#') break;
                        strength++;
                    }

                    run.Text = run.Text.Replace("#", "").Trim();

                    run.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 600 };

                    switch (strength)
                    {
                        case 3:  run.FontSize = 20; break;
                        case 4:  run.FontSize = 26; break;
                    }
                }

                if (i < lines.Length) run.Text += "\n\n";

                txtBox.Inlines.Add(run);
            }
        }

    }
}
