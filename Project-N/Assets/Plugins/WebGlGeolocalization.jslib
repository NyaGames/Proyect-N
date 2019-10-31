mergeInto(LibraryManager.library, {

    Hello: function () {
        window.alert("Hello, world!");
    },

    StartTrackingLocalization: function () {
        var geo_options = {
            enableHighAccuracy: true,
            maximumAge: 30000,
            timeout: 27000
        }

        console.log("Starting to track localization");

        var wpid = navigator.geolocation.watchPosition(sendLocationInfo, error, geo_options);

        function error(error){
            console.error('ERROR(' + error.code + '): ' + error.message);
        }

        function sendLocationInfo(position){
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            var latLon = latitude+', '+longitude;
            SendMessage('WebGlLocationProvider', 'LocationReceived', latLon);          
        }
    } 

   
})