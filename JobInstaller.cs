using System;
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
  }
}