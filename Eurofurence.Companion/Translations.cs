
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

		public static string GetString(string index) => _resourceLoader.GetString(index);


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

        public static string About_Title 
			=> _resourceLoader.GetString("About_Title"); 

        public static string About_Disclaimer_Content 
			=> _resourceLoader.GetString("About_Disclaimer_Content"); 

        public static string About_Disclaimer_Title 
			=> _resourceLoader.GetString("About_Disclaimer_Title"); 

        public static string ContextManager_Update_Initializing 
			=> _resourceLoader.GetString("ContextManager_Update_Initializing"); 

        public static string ContextManager_Update_Synchronizing 
			=> _resourceLoader.GetString("ContextManager_Update_Synchronizing"); 

        public static string ContextManager_Update_Done 
			=> _resourceLoader.GetString("ContextManager_Update_Done"); 

        public static string ContextManager_Update_DownloadingImageContent 
			=> _resourceLoader.GetString("ContextManager_Update_DownloadingImageContent"); 

        public static string ContextManager_Update_Downloading_arg0 
			=> _resourceLoader.GetString("ContextManager_Update_Downloading_arg0"); 

        public static string Login_Error_CredentialsRejected 
			=> _resourceLoader.GetString("Login_Error_CredentialsRejected"); 

        public static string Login_Error_RegistrationNumberInvalid 
			=> _resourceLoader.GetString("Login_Error_RegistrationNumberInvalid"); 

        public static string Login_Error_UsernamePasswordNotProvided 
			=> _resourceLoader.GetString("Login_Error_UsernamePasswordNotProvided"); 
    }
}
