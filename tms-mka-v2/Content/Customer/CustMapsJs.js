var newLong;
var newLat;

function getMapByLatLong(long, lat, rad, kec, elMap, MapModal) {
    var marker;
    MapModal.on('shown.bs.modal', function () {
        var gmarkers = [];
        if (long != "" && lat != "") {
            var addLatLng = { lat: parseFloat(lat.replace(',', '.')), lng: parseFloat(long.replace(',', '.')) };
            var map = new google.maps.Map(elMap, {
                zoom: 14,
                center: addLatLng
            });
        }
        else if (kec != '') {
            var map = new google.maps.Map(elMap, {
                zoom: 14
            });
            var geocoder = new google.maps.Geocoder;
            geocoder.geocode({ 'address': kec }, function (results, status) {
                if (status === 'OK') {
                    map.setCenter(results[0].geometry.location);
                }
            });
        } else {
            //longlat default jakarta
            long = 106.8451300;
            lat = -6.2146200;
            var addLatLng = { lat: parseFloat(lat), lng: parseFloat(long) };
            var map = new google.maps.Map(elMap, {
                zoom: 14,
                center: addLatLng
            });
        }
        marker = new google.maps.Marker({
            position: addLatLng,
            map: map,
            draggable: true,
            travelMode: google.maps.TravelMode.DRIVING
        });
        var circle = new google.maps.Circle({
            map: map,
            radius: parseFloat(rad),    // 10 miles in metres
            fillColor: '#AA0000',
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
        });

        circle.bindTo('center', marker, 'position');
        gmarkers.push(marker);

        google.maps.event.addListener(marker, 'dragend', function (e) {
            console.log(e)
            newLong = e.latLng.lng();
            newLat = e.latLng.lat();
        });

        map.addListener('click', function (e) {
            var lat = e.latLng.lat();
            var long = e.latLng.lng();
            removeMarkers(gmarkers);
            marker = new google.maps.Marker(
             {
                 map: map,
                 draggable: true,
                 animation: google.maps.Animation.DROP,
                 position: new google.maps.LatLng(lat, long),
             });
            gmarkers.push(marker);
            newLong = long;
            newLat = lat;
            google.maps.event.addListener(marker, 'dragend', function () {
                newLong = this.position.lng();
                newLat = this.position.lat();
            });
        });
    })
}

function removeMarkers(gmarkers) {
    for (i = 0; i < gmarkers.length; i++) {
        gmarkers[i].setMap(null);
    }
}