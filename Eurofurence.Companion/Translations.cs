
using Windows.ApplicationModel.Resources; 

namespace Eurofurence.Companion 
{
    public static class Translations 
    {
        private static readonly ResourceLoader _resourceLoader; 

        static Translations() 
        {
            _resourceLoader = ResourceLoader.GetForViewIndependentUse("Translations");
        }

        public static string EventSchedule_Title 
			=> _resourceLoader.GetString("EventSchedule_Title"); 

        public static string Dealers_Title 
			=> _resourceLoader.GetString("Dealers_Title"); 

        public static string ConventionInformation_Title 
			=> _resourceLoader.GetString("ConventionInformation_Title"); 

        public static string Info_Title 
			=> _resourceLoader.GetString("Info_Title"); 

        public static string Main_Title 
			=> _resourceLoader.GetString("Main_Title"); 
    }
}
