using System;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.UI;

namespace ironclad.Features.UI.Reservations
{
    /// <summary> Class ReservationsModule. </summary>
    /// <seealso cref="mojoPortal.Web.SiteModuleControl" />
    public partial class ReservationsModule : SiteModuleControl
    {
        /// <summary> The maximum date </summary>
        public string MaxDate = new DateTime(DateTime.Today.Year, 10, 1).ToString("MM'/'dd'/'yyyy");

        /// <summary> The minimum date </summary>
        public string MinDate = new DateTime(DateTime.Today.Year, 5, 1).ToString("MM'/'dd'/'yyyy");

        /// <summary> The today </summary>
        public string Today = DateTime.Today.ToString("MM'/'dd'/'yyyy");

        /// <summary> The user </summary>
        public SiteUser User;

        /// <summary> The user identifier </summary>
        public int UserId;

        /// <summary> The user unique identifier </summary>
        public string UserGuid;

        /// <summary> The event title </summary>
        public string EventTitle;

        /// <summary> The reservation URL </summary>
        public string ReservationUrl;

        /// <summary> Handles the Load event of the Page control. </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> instance containing the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var basePage = Page as mojoBasePage;
            if (basePage != null)
                basePage.ScriptConfig.IncludeKnockoutJs = true;

            User = SiteUtils.GetCurrentSiteUser() ?? new SiteUser();
            UserId = User.UserId;
            UserGuid = User.UserGuid.ToString();
            EventTitle = User?.FirstName ?? User?.LoginName ?? "";
            ReservationUrl = Page.ResolveUrl("~/Reservations/ReservationsHandler.ashx");

            var script = $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{Page.ResolveUrl("~/Data/style/fullcalendar/fullcalendar.css")}\" />";
            script += $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{Page.ResolveUrl("~/Data/style/fullcalendar/jquery.contextMenu.css")}\" />";
            script += $"<script type=\"text/javascript\" src=\"{Page.ResolveUrl("~/ClientScript/fullcalendar/moment.min.js")}\"></script>";
            script += $"<script type=\"text/javascript\" src=\"{Page.ResolveUrl("~/ClientScript/fullcalendar/fullcalendar.js")}\"></script>";
            script += $"<script type=\"text/javascript\" src=\"{Page.ResolveUrl("~/ClientScript/fullcalendar/jQuery-contextMenu.js")}\"></script>";
            script += $"<script type=\"text/javascript\" src=\"{Page.ResolveUrl("~/ClientScript/fullcalendar/reservations.js")}\"></script>";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "fullcalendar-reservations", script);
        }
    }
}