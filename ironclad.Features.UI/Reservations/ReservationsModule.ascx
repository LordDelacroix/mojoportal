<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationsModule.ascx.cs" Inherits="ironclad.Features.UI.Reservations.ReservationsModule" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<mp:CornerRounderTop ID="ctop1" runat="server" />
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendar eventcalendarbasic">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

				<div class="reservation">

					<div style="height: 0; overflow: hidden">
						<input id="mid" type="hidden" value="<%= ModuleId %>" />
						<input id="mguid" type="hidden" value="<%= ModuleGuid.ToString() %>" />
						<input id="uid" type="hidden" value="<%= UserId %>" />
						<input id="uguid" type="hidden" value="<%= UserGuid %>" />
						<input id="reservationUrl" type="hidden" value="<%= ReservationUrl %>" />
						<input id="minDate" type="hidden" value="<%= MinDate %>" />
						<input id="maxDate" type="hidden" value="<%= MaxDate %>" />
						<input id="today" type="hidden" value="<%= Today %>" />
					</div>

					<div class="calendar"> </div>

					<div class="edit-reservation" style="display: none">
						<label>Start Date</label>
						<input type="text" placeholder="mm/dd/yyyy" class="start-date reservation-date" />
						<label>End Date</label>
						<input type="text" placeholder="mm/dd/yyyy" class="end-date reservation-date" />
						<% if( User.IsInRoles("Admins")) { %>
							<label>Title</label>
							<input type="text" placeholder="Event title" class="event-title" />
							<label>Description</label>
							<input type="text" placeholder="description" class="event-description"  />
						<% } %>
					</div>

				</div>

			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
	</portal:InnerWrapperPanel>
	<mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
