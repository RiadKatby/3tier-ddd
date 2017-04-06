var callbacksQueue = [];
var googleMapsApiIsLoading = false;
function loadGoogleMapsApi(callBackFnName, additionalParams) {
    if ((typeof google == 'undefined' || typeof google.maps == 'undefined')) {
        callbacksQueue.push(callBackFnName);
        if (googleMapsApiIsLoading) return;
        googleMapsApiIsLoading = true;
        additionalParams = additionalParams ? '&' + additionalParams : '';
        jQuery.getScript('https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&language=ar&region=sa&callback=googleMapsMainCallback' + additionalParams)
        // no success callback necessary, google can load our stuff-todo-function
    } else {
        // if gmaps already loaded, we can just continue whatever else we want to do
        executeFunction(callBackFnName);
    }
}

function googleMapsMainCallback() {
    //loaded
    googleMapsApiIsLoading = false;

    //call every callback function in queue
    for (i = 0; i < callbacksQueue.length; i++) {
        executeFunction(callbacksQueue[i]);
    }

    //clearthe array
    callbacksQueue = [];
}

function changeMarkerPosition(marker, lat, lng, moveToNewLocation, map, zoomTo){
    var newPosition = new google.maps.LatLng(lat, lng);
    marker.setPosition(newPosition);
    if(moveToNewLocation)
        map.panTo(newPosition);
    if(zoomTo)
        map.setZoom(zoomTo);
}