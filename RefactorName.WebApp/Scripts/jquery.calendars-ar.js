/* http://keith-wood.name/calendars.html
   Arabic localisation for Gregorian calendar for jQuery.
   Written by Amro Osama March 2013. */
(function ($) {
    $.calendars.calendars.gregorian.prototype.regionalOptions['ar'] = {
        name: 'Gregorian', // The calendar name
		epochs: ['BAM', 'AM'],
		monthNames: ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'اغسطس', 'سمتمبر', 'اكتوبر', 'نوفمبر', 'ديسمبر'],
		monthNamesShort: ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'اغسطس', 'سمتمبر', 'اكتوبر', 'نوفمبر', 'ديسمبر'],
		dayNames: ['الأحد', 'الإثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
		dayNamesMin: ['أح', 'إث', 'ث', 'أر', 'خ', 'ج', 'س'],
		dayNamesShort: ['أ', 'إ', 'ث', 'أ', 'خ', 'ج', 'س'],
		dateFormat: 'dd/mm/yyyy', // See format options on BaseCalendar.formatDate
		firstDay: 6, // The first day of the week, Sat = 0, Sun = 1, ...
		isRTL: true // True if right-to-left language, false if left-to-right
	};
})(jQuery);

