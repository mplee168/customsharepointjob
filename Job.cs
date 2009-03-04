using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Diagnostics;
using System.IO;

// If you change the namespace, adjust TimerJobInstall.cs and feature.xml too
namespace SharePointJobs {
  public class TimerJob : SPJobDefinition
  {
    /// <summary>
    /// Initializes a new instance of the TaskLoggerJob class.
    /// </summary>
    public TimerJob ()
	  : base(){
    }

    /// <summary>
    /// Initializes a new instance of the TaskLoggerJob class.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="service">The service.</param>
    /// <param name="server">The server.</param>
    /// <param name="targetType">Type of the target.</param>
    public TimerJob (string jobName, SPService service, SPServer server, SPJobLockType targetType)
	  : base (jobName, service, server, targetType) {
    }
    
    /// <summary>
    /// Initializes a new instance of the TaskLoggerJob class.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="webApplication">The web application.</param>
    public TimerJob (string jobName, SPWebApplication webApplication)
	  : base (jobName, webApplication, null, SPJobLockType.Job) {
      this.Title = jobName;
    }

    /// <summary>
    /// Executes the specified content db id.
    /// </summary>
    /// <param name="contentDbId">The content db id.</param>
    public override void Execute (Guid contentDbId) 
    {
        // TODO: Implement job logic here

        /*
           // get a reference to the current site collection's content database
           SPWebApplication webApplication = this.Parent as SPWebApplication;
           SPContentDatabase contentDb = webApplication.ContentDatabases[contentDbId];

           // get a reference to the "Tasks" list in the RootWeb of the first site collection in the content database
           SPList taskList = contentDb.Sites[0].RootWeb.Lists["Tasks"];
         */

#if (DEBUG)
        System.Diagnostics.Trace.Assert(false);
#endif

        //WriteToEvtLog("Writing to file", EventLogEntryType.Warning);
        using (var sw = File.AppendText(@"F:\temp\SPTimerJobDate.txt"))
        {
            sw.WriteLine(DateTime.Now);
        }
        //WriteToEvtLog("Finish Writing to file", EventLogEntryType.Warning);
    }

    static string sLog = "Application";
    static string sSource = "TimerJob";

    public static void WriteToEvtLog(string evt, EventLogEntryType evtType)
    {

        if (!EventLog.SourceExists(sSource))
            EventLog.CreateEventSource(sSource, sLog);

        EventLog.WriteEntry(sSource, evt, evtType);

    }

  }
}