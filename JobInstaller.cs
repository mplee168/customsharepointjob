using System;
using System.Collections.Generic;
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
        public static void addConnectionString()
        {
            SPWebApplication webApp = SPWebApplication.Lookup(new Uri(@"http://apldevmoss:33768/sites/TopLevelSiteCollection"));
            SPWebConfigModification mod = new SPWebConfigModification("add[@name=\"MyConnectionString\"]", "configuration/connectionStrings");
            mod.Owner = "AnOwner"; //WebConfigModificationFeatureReceiver.OwnerId
            mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            mod.Value = String.Format("<add name=\"{0}\" connectionString=\"{1}\" providerName=\"{2}\" />", "MyConnectionString", "connstring goes here", "provider goes here");
            webApp.WebConfigModifications.Add(mod);
            webApp.Update();
            webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
        }

        public static void removeConnectionString()
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

    }
}