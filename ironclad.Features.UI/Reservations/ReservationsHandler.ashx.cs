using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Linq;
using ironclad.Business;
using ironclad.Features.UI.Reservations.Components;
using ironclad.Features.UI.Tools;
using mojoPortal.Business;
using Newtonsoft.Json;

namespace ironclad.Features.UI.Reservations
{
    /// <summary> Summary description for ReservationsHandler </summary>
    /// <seealso cref="System.Web.IHttpHandler" />
    public class ReservationsHandler : IHttpHandler
    {
        /// <summary> Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance. </summary>
        /// <value> <c>true</c> if this instance is reusable; otherwise, <c>false</c>. </value>
        public bool IsReusable => true;

        /// <summary> Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface. </summary>
        /// <param name="context"> An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var method = context.Request.HttpMethod.ToLowerInvariant();

            if (method == "get")
                context.Response.Write(GetEvents(context));
            else if (method == "post")
                context.Response.Write(ProcessAction(context.Request.Form));
            else
                context.Response.Write("{ 'text': 'Hello World'}");
        }

        /// <summary> Processes the action. </summary>
        /// <param name="requestForm"> The request form. </param>
        /// <returns> System.String. </returns>
        private string ProcessAction(NameValueCollection requestForm)
        {
            var action = requestForm["action"]?.ToLowerInvariant();

            return action == "add"
                ? AddEvent(requestForm)
                : action == "delete"
                    ? DeleteEvent(requestForm)
                    : action == "change"
                        ? ChangeEvent(requestForm)
                        : "Unknown action";
        }

        /// <summary> Changes the event. </summary>
        /// <param name="requestForm"> The request form. </param>
        /// <returns> System.String. </returns>
        /// <exception cref="NotImplementedException">  </exception>
        private string ChangeEvent(NameValueCollection requestForm)
        {
            throw new NotImplementedException();
        }

        /// <summary> Deletes the event. </summary>
        /// <param name="requestForm"> The request form. </param>
        /// <returns> System.String. </returns>
        private string DeleteEvent(NameValueCollection requestForm)
        {
            var itemId = requestForm["iid"].SafeParse<int>();
            return itemId.HasValue
                ? (ReservationEvent.DeleteByItemID(itemId.Value)
                    ? JsonConvert.SerializeObject(new { success = true, message = "Your reservation was accuessfully added." })
                    : $"Unable to find reservation id {itemId.Value}.")
                : "Unable to find reservation.";
        }

        /// <summary> Adds the event. </summary>
        /// <param name="requestForm"> The request form. </param>
        /// <returns> System.String. </returns>
        private string AddEvent(NameValueCollection requestForm)
        {
            var start = requestForm["start"].SafeParse<DateTime>();
            var end = requestForm["end"].SafeParse<DateTime>();
            var mid = requestForm["mid"].SafeParse<int>();
            var mguid = requestForm["mguid"].SafeParse<Guid>();
            var uid = requestForm["uid"].SafeParse<int>();
            var uguid = requestForm["uguid"].SafeParse<Guid>();

            if (start.HasValue && end.HasValue && mid.HasValue && uid.HasValue && mguid.HasValue && uguid.HasValue)
            {
                var moduleSettings = ModuleSettings.GetModuleSettings(mid.Value);
                var reservation = new ReservationEvent
                {
                    ModuleId = mid.Value,
                    ModuleGuid = mguid.Value,
                    Title = requestForm["title"],
                    Description = requestForm["description"] ?? $"Reservation for {requestForm["title"]}",
                    ImageName = "",
                    StartDate = start.Value,
                    EndDate = end.Value.AddDays(1),
                    Location = moduleSettings["Location"]?.ToString() ?? "",
                    Latitude = moduleSettings["Latitude"]?.ToString() ?? "",
                    Longitude = moduleSettings["Longitude"]?.ToString() ?? "",
                    UserId = uid.Value,
                    UserGuid = uguid.Value,
                    LastModUserGuid = uguid.Value,
                    LastModUserId = uid.Value,
                    LastModUtc = DateTime.UtcNow
                };
                reservation.Save();
                return JsonConvert.SerializeObject(new { success = true, message = "Your reservation was successfully added." });
            }

            return "Unable to Add Event: Missing data";
        }

        /// <summary> Gets the events. </summary>
        /// <param name="context"> The context. </param>
        /// <returns> System.String. </returns>
        private string GetEvents(HttpContext context)
        {
            DateTime start;
            if (DateTime.TryParse(context.Request.QueryString["start"], out start))
            {
                DateTime end;
                if (DateTime.TryParse(context.Request.QueryString["end"], out end))
                {
                    int moduleId;
                    if (int.TryParse(context.Request.QueryString["mid"], out moduleId))
                    {
                        var set = ReservationEvent.GetEvents(moduleId, start, end);
                        return JsonConvert.SerializeObject(ConvertToReservationRows(set), Formatting.Indented);
                    }
                }
            }

            return "";
        }

        /// <summary> Converts to reservation rows. </summary>
        /// <param name="set"> The set. </param>
        /// <returns> List&lt;FullCalendarEvent&gt;. </returns>
        private List<FullCalendarEvent> ConvertToReservationRows(DataSet set)
        {
            return (from DataTable table in set.Tables from DataRow row in table.Rows select ConvertToReservationRow(row)).ToList();
        }

        /// <summary> Converts to reservation row. </summary>
        /// <param name="row"> The row. </param>
        /// <returns> FullCalendarEvent. </returns>
        private static FullCalendarEvent ConvertToReservationRow(DataRow row)
        {
            var evt = new FullCalendarEvent
            {
                id = row.Field<int>("ItemID"),
                itemGuid = row.Field<Guid>("ItemGuid").ToString(),
                moduleId = row.Field<int>("ModuleID"),
                moduleGuid = row.Field<Guid>("ModuleGuid").ToString(),
                title = row.Field<string>("Title"),
                description = row.Field<string>("Description"),
                imageName = row.Field<string>("ImageName"),
                start = row.Field<DateTime>("StartDate").ToString("yyyy-MM-dd HH:mm:ss.fff"),
                end = row.Field<DateTime>("EndDate").ToString("yyyy-MM-dd HH:mm:ss.fff"),
                createdDate = row.Field<DateTime>("CreatedDate").ToString("yyyy-MM-dd"),
                userId = row.Field<int>("UserID"),
                userGuid = row.Field<Guid>("UserGuid").ToString(),
                location = row.Field<string>("Location"),
                latitude = row.Field<string>("Latitude"),
                longitude = row.Field<string>("Longitude"),
                lastModUserId = row.IsNull("LastModUserID") ? null : row.Field<int?>("LastModUserID"),
                lastModUserGuid = row.IsNull("LastModUserGuid") ? null : row.Field<Guid>("LastModUserGuid").ToString(),

                allDay = false
                //backgroundColor = "#a00",
                //textColor = "#fff"
            };
            return evt;
        }
    }
}
