
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

		public static string GetString(string index) => _resourceLoader.GetString(index).Replace("\\n", "\n");


        public static string EventSchedule_Title 
			=> GetString("EventSchedule_Title"); 

        public static string Dealers_Title 
			=> GetString("Dealers_Title"); 

        public static string ConventionInformation_Title 
			=> GetString("ConventionInformation_Title"); 

        public static string Info_Title 
			=> GetString("Info_Title"); 

        public static string Main_Title 
			=> GetString("Main_Title"); 

        public static string About_Title 
			=> GetString("About_Title"); 

        public static string About_Disclaimer_Content 
			=> GetString("About_Disclaimer_Content"); 

        public static string About_Disclaimer_Title 
			=> GetString("About_Disclaimer_Title"); 

        public static string ContextManager_Update_Initializing 
			=> GetString("ContextManager_Update_Initializing"); 

        public static string ContextManager_Update_Synchronizing 
			=> GetString("ContextManager_Update_Synchronizing"); 

        public static string ContextManager_Update_Done 
			=> GetString("ContextManager_Update_Done"); 

        public static string ContextManager_Update_DownloadingImageContent 
			=> GetString("ContextManager_Update_DownloadingImageContent"); 

        public static string ContextManager_Update_Downloading_arg0 
			=> GetString("ContextManager_Update_Downloading_arg0"); 

        public static string Login_Error_CredentialsRejected 
			=> GetString("Login_Error_CredentialsRejected"); 

        public static string Login_Error_RegistrationNumberInvalid 
			=> GetString("Login_Error_RegistrationNumberInvalid"); 

        public static string Login_Error_UsernamePasswordNotProvided 
			=> GetString("Login_Error_UsernamePasswordNotProvided"); 

        public static string PrivateMessages_HasRead 
			=> GetString("PrivateMessages_HasRead"); 

        public static string PrivateMessages_HasUnread 
			=> GetString("PrivateMessages_HasUnread"); 

        public static string PrivateMessages_NoMessages 
			=> GetString("PrivateMessages_NoMessages"); 
    }
}
