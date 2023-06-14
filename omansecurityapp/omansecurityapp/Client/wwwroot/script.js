function initialize() {
    var latlng = new google.maps.LatLng(21.4735, 55.9754);
    var options = {
        zoom: 8, center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map"), options);
}