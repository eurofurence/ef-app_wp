using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.Views
{
    public sealed partial class StaticLoadingPage : Page
    {
        public StaticLoadingPage()
        {
            InitializeComponent();
            Opacity = 0;

            var quote = GetRandomQuote();
            E_TextBlock_Quote.Text = $"\"{quote.Item1}\"";
            E_TextBlock_Author.Text = $" - {quote.Item2}";
            E_TextBlock_Glyph.Text = ((char)(new Random().Next(80, 122))).ToString();
        }

        public Task PlayAsync(Storyboard sb)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            EventHandler<object> lambda = null;

            lambda = (s, e) =>
            {
                sb.Completed -= lambda;
                tcs.TrySetResult(null);
            };
            sb.Completed += lambda;
            sb.Begin();

            return tcs.Task;
        }


        public async Task PageFadeInAsync()
        {
            await PlayAsync(pageFadeIn);
        }

        public async Task PageFadeOutAsync()
        {
            await PlayAsync(pageFadeOut);
        }


        private Tuple<string, string> GetRandomQuote()
        {
            var quotes = new List<Tuple<string, string>>();
            var quote = new Action<string>((s) => quotes.Add(new Tuple<string,string>(s, "Egyptian Proverb")));

            quote("If you search for the laws of harmony, you will find knowledge.");
            quote("The best and shortest road towards knowledge of truth is Nature.");
            quote("People bring about their own undoing through their tongues.");
            quote("To teach one must know the nature of those whom one is teaching.");
            quote("In every vital activity it is the path that matters.");
            quote("The way of knowledge is narrow.");
            quote("Each truth you learn will be, for you, as new as if it had never been written.");
            quote("If you defy an enemy by doubting his courage you double it.");
            quote("The nut doesn't reveal the tree it contains.");
            quote("For knowledge... you should know that peace is an indispensable condition of getting it.");
            quote("Peace is the fruit of activity, not of sleep.");
            quote("One foot isn't enough to walk with.");
            quote("Our senses serve to affirm, not to know.");
            quote("All seed answer light, but the color is different.");
            quote("The plant reveals what is in the seed.");
            quote("Seek peacefully, you will find.");
            quote("Always watch and follow nature.");
            quote("Judge by cause, not by effect.");
            quote("Experience will show you, a Master can only point the way.");
            quote("There grows no wheat where there is no grain.");

            var index = new Random().Next(quotes.Count);
            return quotes[index];
        }
    }
}
