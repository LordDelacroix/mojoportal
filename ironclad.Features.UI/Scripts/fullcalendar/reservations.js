


$(function () {

	var moduleId = $('#mid').val();
	var moduleGuid = $('#mguid').val();
	var userId = $('#uid').val();
	var userGuid = $('#uguid').val();
	var url = $('#reservationUrl').val();

	function getDate(when, isEnd) {
		var date = new Date(when);
		return (date.getMonth() + 1) + '/' + (date.getDate() + (isEnd ? -1 : 0)) + '/' + date.getFullYear();
	}

	function clearDlg() {
		$('.start-date').val('');
		$('.end-date').val('');
		$('.event-title').val('');
		$('.event-description').val('');
	}

	self.addEvent = function (event) {
		clearDlg();
		$('div.edit-reservation').dialog({
			title: 'Add Reservation',
			width: 280,
			modal: false,
			position: { my: 'center top', at: 'center top+80', of: 'body' },
			buttons: [{
				text: 'Ok',
				click: function () {
					var start = $('.start-date').val();
					var end = $('.end-date').val();
					if ($.hasValue(start) && $.hasValue(end)) {
						var data = {
							action: 'add',
							start: start,
							end: end,
							mid: moduleId,
							mguid: moduleGuid,
							uid: userId,
							uguid: userGuid,
							title: $('.event-title').val(),
							description: $('.event-description').val()
						};
						$.post('/Reservations/ReservationsHandler.ashx', data, function (result) {
							if (result.success)
								$('div.calendar').fullCalendar('refetchEvents');
							else {
								alert(result.message);
							}
						});
						$(this).dialog('close');
					}
				}
			}]
		});
	}

	self.current = {};

	self.editEvent = function (event) {
		self.current = event;
		$('.start-date').val(getDate(event.start._i, false));
		$('.end-date').val(getDate(event.end._i, true));
		$('div.edit-reservation').dialog({
			title: 'Edit Reservation',
			width: 280,
			modal: false,
			position: { my: 'center top', at: 'center top+80', of: 'body' },
			buttons: [{
				text: 'Ok',
				click: function () {
					var start = $('.start-date').val() + '00:00:00.000';
					var end = $('.end-date').val() + '23:59:59.999';
					var title = $('.event-title').val();
					if ($.hasValue(start) && $.hasValue(end)) {
						var data = {
							action: 'add',
							start: start,
							end: end,
							mid: moduleId,
							mguid: moduleGuid,
							uid: userId,
							uguid: userGuid,
							title: title,
							description: $('.event-description').val()
						};
						$.post('/Reservations/ReservationsHandler.ashx', data, function (result) {
							if (result.success) {
								$('div.calendar').fullCalendar('refetchEvents');
								$(this).dialog('close');
							} else
								alert(result.message);
						});
					}
				}
			},
				{
					text: 'Delete',
					click: function () {
						var dlg = $(this);
						if (confirm('Are you sure you want to delete this reservation?')) {
							self.deleteEvent(self.current, dlg);
						}
					}
				}]
		});
	}

	self.deleteEvent = function(event, dlg) {
		$.post('/Reservations/ReservationsHandler.ashx', { action: 'delete', iid: event.id }, function(result) {
			if (result.success) {
				$('div.calendar').fullCalendar('refetchEvents');
				if ($.hasValue(dlg))
					dlg.dialog('close');
			}
		});
	}

	var calendarRight = userId > -1 ? 'add' : '';

	self.calendar = $('div.calendar').fullCalendar({
		header: {
			left: 'prev,next today',
			center: 'title',
			right: calendarRight
		},
		eventSources: [{
			url: url,
			type: 'GET',
			data: { mid: moduleId },
			cache: true
		}],
		eventDurationEditable: true,
		editable: true,
		eventLimit: true,
		weekNumbersWithinDays: true,
		customButtons: {
			add: {
				text: 'Add Reservation',
				click: function () {
					if (userId > 0)
						self.addEvent({ title: 'Add Reservation' });
				}
			}
		},
		eventRender: function (event, element) {
			if ($.hasValue(event)) {
				var el = $(element);
				var ev = event;
				el.attr('title', event.description);
				el.attr('uguid', event.userGuid);
				el.attr('uid', event.userId);
				el.data('reservation', event);
				el.dblclick(function (e, f, g) {
					self.editEvent(ev);
				});
			}
		}
	});

	self.menuItems = {
		'edit': { name: 'Edit', icon: 'edit' },
		"delete": { name: 'Delete', icon: 'delete' },
		"sep1": '---------',
		"quit": {
			name: 'Never mind...',
			icon: function () {
				return 'context-menu-icon context-menu-icon-quit';
			}
		}
	};

	$.contextMenu({
		selector: 'a[uid="' + userId + '"]',
		build: function ($trigger, e) {
			self.current = $trigger.data('reservation');
			return {
				items: self.menuItems,
				callback: function (key, options) {
					if (key === 'edit')
						editEvent(self.current);
					else if (key === 'delete')
						deleteEvent(self.current, null);
				}
			}
		}
	});

	$('.reservation-date').datepicker({
		maxDate: '10/01/2017',
		minDate: '05/01/2017'
	});


});


$.extend({
	hasValue: function (item) {
		if (typeof item === 'undefined' || item === null) return false;
		if ($.isArray(item) || typeof item === 'string')
			return item.length > 0;
		if ($.isFunction(item)) {
			return $.hasValue(item());
		}
		return true;
	}
});
