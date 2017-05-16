//client side datetime validation
$.validator.methods.date = function (value, element) {
    var isValid = true;

    //var cul = $(element).attr("data-culture") == "en-US" ? "en-US" : "ar-SA";
    //isValid = this.optional(element) || $.parseDate(value, "dd/MM/yyyy", cul) !== null;

    //check using jQuery.calendar
    var date = value.split('/');
    var year = parseInt(date[2], 10);
    var month = parseInt(date[1], 10);
    var day = parseInt(date[0], 10);

    var calType = $(element).attr("data-culture") == "en-US" ? "gregorian" : "ummalqura";
    isValid = this.optional(element) || $.calendars.instance(calType).isValid(year, month, day);

    return isValid;
}

//isdateafter date compare
$.validator.addMethod("isdateafter", function (value, element, params) {
    if (!value) return true;
    var date = value.split('/');
    var year = parseInt(date[2], 10);
    var month = parseInt(date[1], 10);
    var day = parseInt(date[0], 10);
    var calType = $(element).attr("data-culture") == "en-US" ? "gregorian" : "ummalqura";
    value = $.calendars.instance(calType).toJD(year, month, day);

    var startdatevalue = $('input[name$="' + params.propertytested + '"]').val();

    date = startdatevalue.split('/');
    year = parseInt(date[2], 10);
    month = parseInt(date[1], 10);
    day = parseInt(date[0], 10);
    calType = $('input[name="' + params.propertytested + '"]').attr("data-culture") == "en-US" ? "gregorian" : "ummalqura";
    startdatevalue = $.calendars.instance(calType).toJD(year, month, day);

    if (!value || !startdatevalue) return true;
    return (params.allowequaldates.toString().toLowerCase() == 'true') ? startdatevalue <= value : startdatevalue < value;
});

$.validator.unobtrusive.adapters.add('isdateafter', ['propertytested', 'allowequaldates'], function (options) {
    options.rules['isdateafter'] = options.params;
    options.messages['isdateafter'] = options.message;
});

// iban 
$.validator.addMethod("iban", function (value, element) {
    var conversion = {
        "A": 10, "B": 11, "C": 12, "D": 13, "E": 14, "F": 15, "G": 16, "H": 17, "I": 18, "J": 19, "K": 20, "L": 21, "M": 22, "N": 23,
        'O': 24, "P": 25, "Q": 26, "R": 27, "S": 28, "T": 29, 'U': 30, "V": 31, "W": 32, "X": 33, "Y": 34, "Z": 35
    };
    var numbers = "0123456789";


    var ibanregex = /^([A-Za-z]{2}\d{2})\s?([A-Z0-9]{4}\s?)*([A-Z0-9]{1,4})$/;

    if (ibanregex.test(value)) {
        var iban = value.replace(/\s+/g, "").toUpperCase();
        iban = iban.substr(4) + iban.substr(0, 4);
        var convertediban = "";
        for (var i = 0; i < iban.length; ++i) {
            if (numbers.indexOf(iban.charAt(i), 0) == -1) { // check its number or char 
                convertediban += conversion[iban.charAt(i)];
            }
            else {
                convertediban += iban.charAt(i);
            }
        }
        var reminder = 0;
        while (convertediban.length > 0) {
            var p = convertediban.length > 7 ? 7 : convertediban.length;
            reminder = parseInt(reminder.toString() + convertediban.substr(0, p), 10) % 97;
            convertediban = convertediban.substr(p);
        }

        if (reminder == 1) {
            return true;
        }
    }

    return false; //this.optional(element) ||
}, "");

$.validator.unobtrusive.adapters.addBool("iban");

//future date check

$.validator.addMethod("futuredate", function (value, element, params) {
    var allowTody = params.allowtoday.toString().toLowerCase() == 'true';

    var date = value.split('/');
    var year = parseInt(date[2], 10);
    var month = parseInt(date[1], 10);
    var day = parseInt(date[0], 10);
    var calType = $(element).attr("data-culture") == "en-US" ? "gregorian" : "ummalqura";
    value = $.calendars.instance(calType).toJD(year, month, day);

    var dateNow = $.calendars.instance().today().toJD();
    if (!value) return true;
    return (allowTody && dateNow <= value || !allowTody && dateNow < value);
});

$.validator.unobtrusive.adapters.add('futuredate', ['allowtoday'], function (options) {
    options.rules['futuredate'] = options.params;
    options.messages['futuredate'] = options.message;
});

/////// IsDateInPast
$.validator.addMethod("isdateinpast", function (value, element, params) {
    var allowTody = params.allowtoday.toString().toLowerCase() == 'true';

    var date = value.split('/');
    var year = parseInt(date[2], 10);
    var month = parseInt(date[1], 10);
    var day = parseInt(date[0], 10);
    var calType = $(element).attr("data-culture") == "en-US" ? "gregorian" : "ummalqura";
    value = $.calendars.instance(calType).toJD(year, month, day);

    var dateNow = $.calendars.instance().today().toJD();
    if (!value) return true;
    return (allowTody && dateNow >= value || !allowTody && dateNow > value);
});

$.validator.unobtrusive.adapters.add('isdateinpast', ['allowtoday'], function (options) {
    options.rules['isdateinpast'] = options.params;
    options.messages['isdateinpast'] = options.message;
});

//ID Number 

$.validator.addMethod("natidnumber", function (value, element, params) {
    var isSaudi = params.issaudi.toString().toLowerCase() == 'true';

    if (!value) return true;

    //length
    if (value.length !== 10)
        return false;

    //saudi
    if ((value.charAt(0) != '1' && value.charAt(0) != '2') ||
        (isSaudi && value.charAt(0) != '1'))
        return false;

    var sum = 0, currentDigit;

    for (var i = 0; i <= 8; i++) {
        currentDigit = parseInt(value.charAt(i), 10);
        if (i % 2 == 0) {
            currentDigit *= 2;
            if (currentDigit > 9)
                currentDigit = 1 + (currentDigit % 10);
        }
        sum += currentDigit;
    }
    var lastDigit = sum % 10;
    if (lastDigit != 0)
        lastDigit = 10 - lastDigit;

    //compare last digit with actual value
    var theLast = parseInt(value.charAt(9), 10);
    return lastDigit == theLast;
});

$.validator.unobtrusive.adapters.add('natidnumber', ['issaudi'], function (options) {
    options.rules['natidnumber'] = options.params;
    options.messages['natidnumber'] = options.message;
});