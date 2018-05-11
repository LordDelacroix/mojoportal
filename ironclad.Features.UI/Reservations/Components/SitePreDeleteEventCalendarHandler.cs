using log4net;
using ironclad.Business;
using mojoPortal.Business.WebHelpers;

namespace ironclad.Features.UI.Reservations.Components
{
    public class SitePreDeleteEventReservationHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SitePreDeleteEventReservationHandler));

        public override void DeleteSiteContent(int siteId)
        {
            if(Log.IsDebugEnabled)Log.Debug($"Deleting content by site id {siteId}");
            ReservationEvent.DeleteBySite(siteId);
        }
    }
}
