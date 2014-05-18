$(document).ready(function () {
    $(document).on("click", ".img-photo", function () {
        $(".img-photo").removeClass("selected-image");
        $(this).addClass("selected-image");
    });
    $('#btnSearch').click(function () {
        var query = $('#txtSearch').val();
        
        if (query == null) {
            aler('Enter search query');
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/Event/WebSearch",
            data: { 'query': query},
            datatype: "html",
            success: function (data) {
                $('#result').html(data);
            }
        });
    });

    
    $('#btnContinue').click(function () {
            var src = $('img.selected-image').attr("src");
        if(src == null)
        {
            alert('Select an image');
            return false;
        }
        $('#hdnimageSource').val(src);
        return true;
    });
});


function SetResponse(res)
{
    $('#hdnres').val(res);
    return true;
}
var autocomplete;

function initializeGooglePlace() {
    // Create the autocomplete object, restricting the search
    // to geographical location types.
    autocomplete = new google.maps.places.Autocomplete(
        (document.getElementById('Location')),
        { types: ['geocode'] });
    // When the user selects an address from the dropdown,
    // populate the address fields in the form.
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        fillInAddress();
    });
}

// [START region_geolocation]
// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = new google.maps.LatLng(
                position.coords.latitude, position.coords.longitude);
            autocomplete.setBounds(new google.maps.LatLngBounds(geolocation,
                geolocation));
        });
    }
}


