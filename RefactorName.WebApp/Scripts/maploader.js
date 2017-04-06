
var geocoder;
var map;
var marker;
var infowindow = new google.maps.InfoWindow();
function initializeMap(longitude, Laditude, mapDiv) {
    if (!mapDiv)
        mapDiv = "map-canvas";
    var qs = getQueryStrings();
    var myloc = qs["loc"];
    geocoder = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(24.693190719999983, 46.68778116);
    var mapOptions = {
        zoom: 15,
        center: latlng,
        mapTypeId: 'roadmap'
    }
    map = new google.maps.Map(document.getElementById(mapDiv), mapOptions);
    codeLatLng(longitude, Laditude);
}
function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}

function codeLatLng(longitude, Laditude) {
    var lat = parseFloat(Laditude);
    var lng = parseFloat(longitude);
    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                map.setZoom(15);
                map.setCenter(latlng);
                marker = new google.maps.Marker({
                    position: latlng,
                    map: map
                });
                infowindow.setContent(results[1].formatted_address);
                infowindow.open(map, marker);
            } else {
                alert('لا يوجد هذا الموقع في الخريطة');
            }
        } else {
            alert('Geocoder failed due to: ' + status + " خطأ في موقع المحدد على الخريطة ");
        }
    });
}