var index = 0;

$(function () {

	// Focus input when clicking label
	$('body').on('click tap', 'fieldset > label', function (e) {
		if ($(e.target).is('a'))
			return true;
		var el = $(this).nextAll("input,select").first();
		el.is("input[type='checkbox']") ? el.prop('checked', !el.prop('checked')) : el.focus();
	});

	// Notice for no assigned modules
	if ($('.tabs').length && !$('.tabs').html().trim().length) {
		$('.tabs').hide();
		$('.tab_container').html("<span style='font-size:14px;color:#900;padding-left:25px;'>No modules have been assigned to this page (you can't edit it yet).</span>");
	}

	// FAQ appends
	$('textarea').length && $('textarea[id$=description], textarea[id$=subText]').each(function () {
		$(this).parents('fieldset').find('label').append("<span><a class='ep' href='http://scripts.vertouk.com/FAQ/Help.html#search:text'>(?)</a></span>");
	});
	$('input[type=checkbox]').length && $('input[type=checkbox][id$=published]').each(function () {
		var el = $(this).parents('fieldset').find('label');
		!el.find('a').length && el.append("<span><a class='ep' href='http://scripts.vertouk.com/FAQ/Help.html#search:publish'>(?)</a></span>");
	});
	$('input[type=text]').length && $('input[type=text][id$=slug]').each(function () {
		var el = $(this).parents('fieldset').find('label');
		!el.find('a').length && el.append("<span><a class='ep' href='http://scripts.vertouk.com/FAQ/Help.html#search:slug'>(?)</a></span>");
	});

	// Fix enter key
	$('body').on('keypress', 'input', function (e) {
		if (e.keyCode == 13) {
			e.preventDefault();
			$(this).parents('.module').find('.save-button:visible').trigger('click');
		}
	});

	// Everything below is for drag + drop row ordering
	var fixHelper = function (e, ui) {
		ui.children().each(function () {
			$(this).width($(this).width());
		});
		return ui;
	};

	$(".gv.sortable tbody").each(function () {
		var tab = $(this).parents('.tab_content');
		$(this).sortable({
			helper: fixHelper,
			containment: '#' + tab.attr('id') + ' .gv',
			cursor: 'ns-resize',
			items: 'tr:not(.gvheader):not(.paging)',
			change: function (e, ui) {
				var p = $(this).parents('.module');
				p.find("input[src='/admin/images/move_up.png'],input[src='/admin/images/move_down.png']").fadeOut(250);
				p.find('.save-order-result').remove();
				!p.find('.save-order-btn').length && p.find('input[type=submit].add').after("<a href='#save-order' class='btn save-order-btn' style='float:right;margin-right:20px;'>Save ordering</a>");
			},
		}).disableSelection();
	});

	$('body').on('click', '.save-order-btn', function (e) {
		e.preventDefault();
		var p = $(this).parents('.module'), b = $(this), t = p.find('.gv');
		ep.create('save-order', { loading: true });
		ep.load('save-order');
		var r = "";
		t.find("tr:not(.gvheader)").each(function () {
			r += $(this).attr('data-id') + ":" + ($(this).index() + (Number(t.attr('data-current-page')) * Number(t.attr('data-page-size')))) + ";";
		});
		setTimeout(function () {
			$.ajax({
				type: 'POST',
				url: '/admin/UpdatePageOrder.aspx',
				data: 'table=' + t.attr('data-table') + '&sort-column=' + t.attr('data-sort-column') + '&id-column=' + t.attr('data-id-column') + '&records=' + encodeURIComponent(r),
				success: function (r) {
					ep.hide('save-order');
					if ($("<div>" + r + "</div>").text().trim() == "true")
						b.after("<span class='save-order-result' style='float:right;margin-right:20px;margin-top:10px;font-size:12px;font-weight:bold;color:#090;'>&#10004; Ordering saved successfully</span>").remove();
					else
						ep.alert({ type: 'error', size: 32, title: 'Ordering Error', content: r });
				},
				error: function (r) {
					ep.hide('save-order');
					ep.alert({ type: 'error', size: 32, title: 'Ordering Error', content: $("<div>" + r.responseText + "</div>").text() });
				},
			});
		}, 500);
	});

	$('body').on('click', '.move-up input, .move-down input', function (e) {
		if (!localStorage['drag-drop-tip'] && $(this).parents('.gv.sortable').length)
			localStorage['drag-drop-tip'] = 1;
	}).on('ep-hidden', '#ep-window-drag-drop-tip', function (e) {
		if (localStorage['drag-drop-tip'] != 2)
			delete localStorage['drag-drop-tip'];
	});
	if (localStorage['drag-drop-tip'] == 1)
		ep.alert({
			id: 'drag-drop-tip', size: 32, title: "Did you know?", content: "You can also drag the rows into the order you desire.<br/>Just click the save button in the top right afterwards.", ok: 'Got it!', 'finally': function () {
				localStorage['drag-drop-tip'] = 2;
			},
		});

});