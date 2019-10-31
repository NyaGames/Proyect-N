mergeInto(LibraryManager.library, {
    Hello: function () {
        window.alert("Hello, world!");
    },

    WebGlGeolocation: function () {

        function success(position) {
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;

            console.log(latitude + ' | ' + longitude);
            return [latitude, longitude];
        }

        function error() {
            console.error('Unable to retrieve your location');
        }

        if (!navigator.geolocation) {
            console.error('Geolocation is not supported by your browser');
        } else {
            navigator.geolocation.getCurrentPosition(success, error);
        }
    },

    AskForLocationPermission: function () {
        alert('Permission asked');
        navigator.permissions.query({ name: 'geolocation' }).then(function (result) {
            if (result.state == 'granted') {
                report(result.state);
                return true;

            } else if (result.state == 'prompt') {
                report(result.state);
                //navigator.geolocation.getCurrentPosition(revealPosition, positionDenied, geoSettings);
            } else if (result.state == 'denied') {
                report(result.state);
                return false;

            }

            result.onchange = function () {
                report(result.state);
            }
        });
    },

    report: function (state) {
        alert('Permission ' + state);
    },

    StartTrackingLocalization: function () {
        var geo_options = {
            enableHighAccuracy = true,
            maximumAge: 30000,
            timeout = 27000
        }

        var wpid = navigator.geolocation.watchPosition(SendLocationInfo, error, geo_options);
    },

    SendLocationInfo: function (position) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;

        var latLon = [latitude, longitude];

        unityInstace.SendMessage('WebGlLocationProvider', LocationReceived, latLon);
        
    }, 

    error: function(error){
        console.error('ERROR(' + error.code + '): ' + error.message);
    }





})