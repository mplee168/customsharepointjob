using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

// If you change the namespace, adjust TimerJobInstall.cs and feature.xml too
namespace SharePointJobs {
  class TimerJobInstaller:SPFeatureReceiver {
    const string JOB_NAME = "TimerJob";

    /// <summary>
    /// Occurs after a Feature is installed.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
    public override void FeatureInstalled (SPFeatureReceiverProperties properties) {
    }

    /// <summary>
    /// Occurs when a Feature is uninstalled.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
    public override void FeatureUninstalling (SPFeatureReceiverProperties properties) {
    }

    /// <summary>
    /// Occurs after a Feature is activated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
    public override void FeatureActivated (SPFeatureReceiverProperties properties) {
      // register the the current web
      //SPSite site = properties.Feature.Parent as SPSite;

        SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
      // make sure the job isn't already registered
      //foreach (SPJobDefinition job in site.WebApplication.JobDefinitions) {
        foreach (SPJobDefinition job in webApp.JobDefinitions)
        {
        if (job.Name == JOB_NAME)
          job.Delete();
      }

      // install the job
      //TimerJob myTimerJob = new TimerJob(JOB_NAME, site.WebApplication);
        TimerJob myTimerJob = new TimerJob(JOB_NAME, webApp);
     
        /*
      SPMinuteSchedule schedule = new SPMinuteSchedule();
      schedule.BeginSecond = 0;
      schedule.EndSecond = 59;
      schedule.Interval = 1;
      myTimerJob.Schedule = schedule;
      */

        
      SPSchedule schedule = null;
      SPDailySchedule dailySchedule = new SPDailySchedule();
      dailySchedule.BeginHour = 0;
      dailySchedule.BeginMinute = 0;
      dailySchedule.BeginSecond = 0;
      dailySchedule.EndHour = 0;
      dailySchedule.EndMinute =1;
      dailySchedule.EndSecond = 0;
      schedule = dailySchedule;
      myTimerJob.Schedule = schedule;
        
      myTimerJob.Update();
    }

    /// <summary>
    /// Occurs when a Feature is deactivated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
    public override void FeatureDeactivating (SPFeatureReceiverProperties properties) {
        /*
      SPSite site = properties.Feature.Parent as SPSite;

      // delete the job
      foreach (SPJobDefinition job in site.WebApplication.JobDefinitions) {
        if (job.Name == JOB_NAME)
          job.Delete();
      }
         */

        SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
        // make sure the job isn't already registered
        //foreach (SPJobDefinition job in site.WebApplication.JobDefinitions) {
        foreach (SPJobDefinition job in webApp.JobDefinitions)
        {
            if (job.Name == JOB_NAME)
                job.Delete();
        }

    }

    public void addConnectionString()
    {
        SPWebApplication webApp = SPWebApplication.Lookup(new Uri("http://litware"));
        SPWebConfigModification mod = new SPWebConfigModification("add[@name=\"MyConnectionString\"]", "configuration/connectionStrings");
        mod.Owner = "AnOwner"; //WebConfigModificationFeatureReceiver.OwnerId
        mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
        mod.Value = String.Format("<add name=\"{0}\" connectionString=\"{1}\" providerName=\"{2}\">", "MyConnectionString", "connstring goes here", "provider goes here");
        webApp.WebConfigModifications.Add(mod);
        webApp.Update();
        webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications(); 
    }

    public void remoteConnectionString()
    {
        SPWebConfigModification configModFound = null;
        SPWebApplication webApp = SPWebApplication.Lookup(new Uri("http://litware"));
        //Collection<SPWebConfigModification> modsCollection = webApp.WebConfigModifications;
        var modsCollection = webApp.WebConfigModifications;
        for (int i = 0; i < modsCollection.Count; i++)
        {

            if (modsCollection[i].Owner == "AnOwner" && modsCollection[i].Name == "add[@name=\"MyConnectionString\"]")
                configModFound = modsCollection[i];
        }

        if (configModFound != null)
        {
            modsCollection.Remove(configModFound);
            webApp.Update();
            webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
        } 
    }
  }

    public class WebConfigModifier
    {
        public static void addConnectionString(string url)
        {
            SPWebApplication webApp = SPWebApplication.Lookup(new Uri(@"http://apldevmoss:33768/sites/TopLevelSiteCollection"));
            SPWebConfigModification mod = new SPWebConfigModification("add[@name=\"MyConnectionString\"]", "configuration/connectionStrings");
            mod.Owner = "AnOwner"; // WebConfigModificationFeatureReceiver.OwnerId;
            mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            mod.Value = String.Format("<add name=\"{0}\" connectionString=\"{1}\" providerName=\"{2}\" />", "MyConnectionString", "connstring goes here", "provider goes here");
            webApp.WebConfigModifications.Add(mod);
            webApp.Update();
            webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
        }

        public static void removeConnectionString(string url)
        {
            SPWebConfigModification configModFound = null;
            SPWebApplication webApp = SPWebApplication.Lookup(new Uri("http://apldevmoss:33768/sites/TopLevelSiteCollection"));
            //Collection<SPWebConfigModification> modsCollection = webApp.WebConfigModifications;
            var modsCollection = webApp.WebConfigModifications;
            for (int i = 0; i < modsCollection.Count; i++)
            {

                if (modsCollection[i].Owner == "AnOwner" && modsCollection[i].Name == "add[@name=\"MyConnectionString\"]")
                    configModFound = modsCollection[i];
            }

            if (configModFound != null)
            {
                modsCollection.Remove(configModFound);
                webApp.Update();
                webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            }
        }

        public static void DisplayConnectionString(string url)
        {

            SPWebApplication webApp = SPWebApplication.Lookup(new Uri("http://apldevmoss:33768/sites/TopLevelSiteCollection"));

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("/", webApp.Name) as System.Configuration.Configuration;

            
            ConnectionStringSettingsCollection connectionStrings = config.ConnectionStrings.ConnectionStrings;

            /*
            // Get the connectionStrings section.
            ConnectionStringsSection connectionStringsSection =
                WebConfigurationManager.GetSection("connectionStrings")
                as ConnectionStringsSection;
            

            // Get the connectionStrings key,value pairs collection.
            ConnectionStringSettingsCollection connectionStrings =
                connectionStringsSection.ConnectionStrings;
            */
            // Get the collection enumerator.
            //IEnumerator connectionStringsEnum =
            //    connectionStrings.GetEnumerator();

           var connectionStringsEnum =
                connectionStrings.GetEnumerator();

            // Loop through the collection and 
            // display the connectionStrings key, value pairs.
            int i = 0;
            Console.WriteLine("[Display the connectionStrings] for url: " + url);
            while (connectionStringsEnum.MoveNext())
            {
                string name = connectionStrings[i].Name;
                Console.WriteLine("Name: {0} Value: {1}",
                name, connectionStrings[name]);
                i += 1;
            }

            Console.WriteLine();

        }

        public static void DisplayAppSettings(string url)
        {

            SPWebApplication webApp = SPWebApplication.Lookup(new Uri("http://apldevmoss:33768/sites/TopLevelSiteCollection"));

            // Get the configuration object for a Web application
            // running on the local server. 
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("/configTest", webApp.Name)
                as System.Configuration.Configuration;

            // Get the appSettings.
            KeyValueConfigurationCollection appSettings = config.AppSettings.Settings;


            // Loop through the collection and
            // display the appSettings key, value pairs.
            Console.WriteLine("[appSettings for app at: {0}]", url);
            foreach (string key in appSettings.AllKeys)
            {
                Console.WriteLine("Name: {0} Value: {1}",
                key, appSettings[key].Value);
            }

            Console.WriteLine();
        }
    }
}