<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddReservation.ascx.cs" Inherits="ironclad.Features.UI.Reservations.AddReservation" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<mp:CornerRounderTop ID="ctop1" runat="server" />
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendar eventcalendarbasic">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

				<div class="start-date">
					<label for="start-date-value">Start Date</label>
					<input id="start-date-value" type="text" class="pickdate" placeholder="mm/dd/yyyy" />
				</div>
				<div class="end-date">
					<label for="end-date-value">End Date</label>
					<input id="end-date-value" type="text" placeholder="mm/dd/yyyy" />
				</div>

				<script type="text/javascript">
					$( function() {
						$( "input.pickdate" ).datepicker();
					} );
				</script>

			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
	</portal:InnerWrapperPanel>
	<mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
