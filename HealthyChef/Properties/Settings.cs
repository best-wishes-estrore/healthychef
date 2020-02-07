namespace HealthyChef.Properties
{
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.

    /*
     * Use the 'SettingsGroupName' attribute to associate settings between projects.
     * This allows us to use a single connection string setting in the App.config
     * for use by all modules.
     * See:
     * http://forums.microsoft.com/msdn/showpost.aspx?postid=117187&siteid=1
     * http://forums.microsoft.com/msdn/showpost.aspx?postid=22844&siteid=1
     */
    //[global::System.Configuration.SettingsGroupName("BayshoreSolutions.WebModules.Properties.Settings")]
    internal sealed partial class Settings
    {
        public Settings()
        {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }

        public override object this[string propertyName]
        {
            get
            {
                if (propertyName == "WebModules")
                {
                    try
                    {
                        return (System.Configuration.ConfigurationManager.ConnectionStrings[propertyName].ConnectionString);
                    }
                    catch (System.Exception ex)
                    {
                        throw new System.Configuration.ConfigurationErrorsException(string.Format("'{0}' connection string is invalid or missing in application configuration.", propertyName), ex);
                    }
                }
                else return base[propertyName];
            }
            set
            {
                base[propertyName] = value;
            }
        }
    }
}
