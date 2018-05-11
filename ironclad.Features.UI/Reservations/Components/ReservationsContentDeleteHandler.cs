using System;
using ironclad.Business;
using mojoPortal.Business.WebHelpers;

namespace ironclad.Features.UI.Reservations.Components
{
    public class ReservationsContentDeleteHandler : ContentDeleteHandlerProvider
    {
        public override void DeleteContent(int moduleId, Guid moduleGuid)
        {
            ReservationEvent.DeleteByModule(moduleId);
        }
    }
}
