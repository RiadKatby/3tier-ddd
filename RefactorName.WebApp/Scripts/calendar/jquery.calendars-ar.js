/* http://keith-wood.name/calendars.html
   Arabic localisation for UmmAlQura calendar for jQuery v2.0.0.
   Written by Amro Osama March 2013. */
(function ($) {
	$.calendars.calendars.gregorian.prototype.regionalOptions['ar'] = {
		name: 'Gregorian',
		epochs: ['BCE', 'CE'],
		monthNames: ['يناير.1', 'فبراير.2', 'مارس.3', 'إبريل.4', 'مايو.5', 'يونية.6',
		'يوليو.7', 'أغسطس.8', 'سبتمبر.9', 'أكتوبر.10', 'نوفمبر.11', 'ديسمبر.12'],
		monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
		dayNames:  ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
		dayNamesShort: ['أحد', 'اثنين', 'ثلاثاء', 'أربعاء', 'خميس', 'جمعة', 'سبت'],
		dayNamesMin: ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
		dateFormat: 'dd/mm/yyyy',
		firstDay: 6,
		isRTL: true
	};
})(jQuery);

