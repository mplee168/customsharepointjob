using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePointJobs
{
	public class PendingReminder
	{
        public PendingReminder() { }

        public void SendReminder()
        {
            using (SIREntities context = new SIREntities())
            {
                var incidents = new List<Incident>();

                foreach (Incident i in context.Incident)
                {
                    if (DateTime.Now.Subtract(i.LastModifiedDate).Days > 13)
                    {
                        incidents.Add(i);
                    }
                }

                foreach (Incident i in incidents)
                    Console.WriteLine(i.IncidentID + "," + i.LastModifiedDate);

            }
        }

        public int Count
        {
            get
            {
                using (SIREntities context = new SIREntities())
                {
                    return context.Incident.Count();
                }

            }
        }

        public void Insert()
        {
            using (SIREntities context = new SIREntities())
            {
                context.AddToIncident(new Incident()
                {
                    IncidentID = System.Guid.NewGuid(),
                    CreatedBy = "pxlee",
                    CreatedDate = DateTime.Now,
                    HeatTicketNumber = "",
                    ChangeRequestNumber = "",
                    ImpactID = 1,
                    IncidentNumber = "SD-888",
                    IssueChronology = "Chrono",
                    IssueDescription = "Description",
                    IssueDeviceID = "",
                    IssueLocation = "",
                    IssueSummary = "Summary",
                    IssueRootCause = "root",
                    IssueResolutionDate = DateTime.Now,
                    IssueStartDate = DateTime.Now,
                    OtherSystemsAffected = false,
                    OtherSystemsAffectedValue = "",
                    ProcessImprovements = "Improvement",
                    TravelRequired = false,
                    ServiceDeskTicketNumber = "888",
                    ServiceGroupID = 1,
                    SeverityID = 1,
                    Status = "Pending",
                    TechnicianEmail = "abc@abc.com",
                    TechnicianName = "pxlee",
                    TechnicianPhone = "333-333-3333",
                    VisibilityID = 1,
                    LastModifiedBy = "pxlee",
                    LastModifiedDate = DateTime.Now
                });

                context.SaveChanges();
            }
        }

        public void Change()
        {
            using (SIREntities context = new SIREntities())
            {
                var incident = context.Incident.First(i => i.ServiceDeskTicketNumber.Contains("888"));
                if (incident != null)
                {
                    incident.TechnicianName = "Pong Lee";
                }
                context.SaveChanges();
            }
        }

        public void Delete()
        {
            using (SIREntities context = new SIREntities())
            {
                var incident = context.Incident.First(i => i.ServiceDeskTicketNumber.Contains("888"));
                context.DeleteObject(incident);
                context.SaveChanges();
            }
        }

        public void Relation()
        {
            using (SIREntities context = new SIREntities())
            {

                var ia = context.IncidentAuditItem.Include("AuditITem").First(a => a.CreatedBy == "pxlee");
                if (ia != null)
                    Console.WriteLine(ia.AuditItem.Description);
            }

        }
	}
}
