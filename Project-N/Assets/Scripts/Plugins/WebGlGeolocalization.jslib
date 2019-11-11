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

        function error(error) {
            console.error('ERROR(' + error.code + '): ' + error.message);
        }

        function sendLocationInfo(position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            var latLon = latitude + ', ' + longitude;
            SendMessage('WebGlLocationProvider', 'LocationReceived', latLon);
        }
    },

    StartTrackingOrientation: function () {
        if (!isPlayingOnMobile) return;
        window.addEventListener("deviceorientation", sendOrientationInfo)

        function sendOrientationInfo(orientation) {
            var userHeading = orientation.alpha;
            userHeading = userHeading.toString();


            SendMessage('WebGlLocationProvider', 'OrientationReceived', userHeading);
        }

        function isPlayingOnMobile() {
            if (navigator.userAgent.match(/Android/i)
                || navigator.userAgent.match(/webOS/i)
                || navigator.userAgent.match(/iPhone/i)
                || navigator.userAgent.match(/iPad/i)
                || navigator.userAgent.match(/iPod/i)
                || navigator.userAgent.match(/BlackBerry/i)
                || navigator.userAgent.match(/Windows Phone/i)
            ) {
                return true;
            }
            else {
                return false;
            }
        }
    }   
})