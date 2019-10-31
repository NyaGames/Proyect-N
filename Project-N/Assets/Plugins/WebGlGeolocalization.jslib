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
    }

})