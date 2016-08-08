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

            quotes.Add(new Tuple<string, string>("In the beginning, God created man, but seeing him so feeble, He gave him the cat.", "Warren Eckstein"));
            quotes.Add(new Tuple<string, string>("A home without a cat- and a well-fed, well-petted and properly revered cat- may be a perfect home, perhaps, but how can it prove title?", "Pudd'nhead Wilson"));
            quotes.Add(new Tuple<string, string>("If animals could speak the dog would be a a blundering outspoken fellow, but the cat would have the rare grace of never saying a word too much.", "Mark Twain"));
            quotes.Add(new Tuple<string, string>("A cat is more intelligent than people believe, and can be taught any crime.", "Mark Twain, Notebook, 1895"));
            quotes.Add(new Tuple<string, string>("I simply can't resist a cat, particularly a purring one. They are the cleanest, cunningest, and most intelligent things I know, outside of the girl you love, of course.", "Abroad with Mark Twain and Eugene Field, Fisher"));
            quotes.Add(new Tuple<string, string>("Of all God's creatures there is only one that cannot be made the slave of the leash. That one is the cat. If man could be crossed with the cat it would improve man, but it would deteriorate the cat.", "Mark Twain Notebook, 1894"));
            quotes.Add(new Tuple<string, string>("You can keep a dog; but it is the cat who keeps people, because cats find humans useful domestic animals.", "George Mikes from'How to be decadent'"));
            quotes.Add(new Tuple<string, string>("Dogs come when they're called. Cats take a message and get back to you.", "Mary Bly"));
            quotes.Add(new Tuple<string, string>("For a man to truly understand rejection, he must first be ignored by a cat.", "Anon"));
            quotes.Add(new Tuple<string, string>("I love cats because I love my home and after a while they become its visible soul.", "Jean Cocteau"));
            quotes.Add(new Tuple<string, string>("There are two means of refuge from the misery of life - music and cats.", "Albert Schweitzer"));
            quotes.Add(new Tuple<string, string>("There are few things in life more heartwarming than to be welcomed by a cat.", "Tay Hohoff"));
            quotes.Add(new Tuple<string, string>(" God made the cat in order that humankind might have the pleasure of caressing the tiger.", "Fernand Mery"));
            quotes.Add(new Tuple<string, string>("There are few things in life more heartwarming than to be welcomed by a cat.", "Tay Hohoff"));
            quotes.Add(new Tuple<string, string>("Cats are smarter than dogs. You can't get eight cats to pull a sled through snow. ", "Jeff Valdez"));
            quotes.Add(new Tuple<string, string>("Way down deep, we're all motivated by the same urges. Cats have the courage to live by them.", "Jim Davis"));
            quotes.Add(new Tuple<string, string>("There is, incidentally, no way of talking about cats that enables one to come off as a sane person.", "Dan Greenberg"));
            quotes.Add(new Tuple<string, string>("The smallest feline is a masterpiece.", "Leonardo da Vinci"));
            quotes.Add(new Tuple<string, string>("In a cat's eye, all things belong to cats.", "English Proverb"));
            quotes.Add(new Tuple<string, string>("Beware of people who dislike cats.", "Irish Proverb"));
            quotes.Add(new Tuple<string, string>("You will always be lucky if you know how to make friends with strange cats.", "Colonial American Proverb"));
            quotes.Add(new Tuple<string, string>("With the qualities of cleanliness, affection, patience, dignity, and courage that cats have, how many of us, I ask you, would be capable of becoming cats?", "Fernand Mery"));
            quotes.Add(new Tuple<string, string>("I like pigs. Dogs look up to us. Cats look down on us. Pigs treat us as equals.", "Winston Churchill"));
            quotes.Add(new Tuple<string, string>("I have studied many philosophers and many cats. The wisdom of cats is infinitely superior.", "Hippolyte Taine"));
            quotes.Add(new Tuple<string, string>("A meow massages the heart.", "Stuart McMillan"));
            quotes.Add(new Tuple<string, string>("No matter how much cats fight, there always seems to be plenty of kittens.", "Abraham Lincoln"));
            quotes.Add(new Tuple<string, string>("Dogs believe they are human. Cats believe they are God.", "Unknown"));
            quotes.Add(new Tuple<string, string>("Time spent with cats is never wasted.", "Unknown"));
            quotes.Add(new Tuple<string, string>("Women and cats will do as they please, and men and dogs should relax and get used to the idea.", "Unknown"));
            quotes.Add(new Tuple<string, string>("No heaven will not ever be Heaven be; Unless my cats are there to welcome me.", "Unknown"));
            quotes.Add(new Tuple<string, string>("How we behave toward cats here below determines our status in heaven.", "Robert A. Heinlein"));
            quotes.Add(new Tuple<string, string>("Dogs have owners, cats have staff.", "Unknown"));
            quotes.Add(new Tuple<string, string>("Thousands of years ago, cats were worshipped as gods. Cats have never forgotten this.", "Anonymous"));
            quotes.Add(new Tuple<string, string>("There are many intelligent species in the universe. They are all owned by cats.", "Anonymous"));
            quotes.Add(new Tuple<string, string>("No amount of time can erase the memory of a good cat, and no amount of masking tape can ever totally remove his fur from your couch.", "Leo Dworken"));
            quotes.Add(new Tuple<string, string>("One cat just leads to another.", "Ernest Hemingway"));
            quotes.Add(new Tuple<string, string>("The cat has too much spirit to have no heart.", "Ernest Menaul"));
            quotes.Add(new Tuple<string, string>("As every cat owner knows, nobody owns a cat.", "Ellen Perry Berkeley"));
            quotes.Add(new Tuple<string, string>("People who hate cats, will come back as mice in their next life.", "Faith Resnick"));
            quotes.Add(new Tuple<string, string>("One reason we admire cats is for their proficiency in one-upmanship. They always seem to come out on top, no matter what they are doing, or pretend they do.", "Barbara Webster"));
            quotes.Add(new Tuple<string, string>("I have noticed that what cats most appreciate in a human being is not the ability to produce food which they take for granted--but his or her entertainment value.", "Geoffrey Household"));
            quotes.Add(new Tuple<string, string>("As anyone who has ever been around a cat for any length of time well knows cats have enormous patience with the limitations of the human kind.", "Cleveland Amory"));

            var index = new Random().Next(quotes.Count);
            return quotes[index];
        }
    }
}
