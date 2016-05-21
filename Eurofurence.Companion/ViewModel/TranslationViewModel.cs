namespace Eurofurence.Companion.ViewModel
{
    public class TranslationViewModel
    {
        public string this [string index] => Translations.GetString(index);
    }
}
