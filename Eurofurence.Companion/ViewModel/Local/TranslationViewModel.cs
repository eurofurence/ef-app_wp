namespace Eurofurence.Companion.ViewModel.Local
{
    public class TranslationViewModel
    {
        public string this [string index] => Translations.GetString(index);
    }
}
