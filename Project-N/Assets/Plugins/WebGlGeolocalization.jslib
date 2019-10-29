mergeInto(LibraryManager.library, {
    Hello: function(){
        window.alert("Hello, world!");
    },

    WebGlGeolocation: function(){

        function success(position){
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;

            alert(latitude + ' | ' + longitude);
        }  

        function error(){
            console.error('Unable to retrieve your location');
        }

        if (!navigator.geolocation) {
            console.error('Geolocation is not supported by your browser');
          } else {            
            navigator.geolocation.getCurrentPosition(success, error);
          }
    }
})