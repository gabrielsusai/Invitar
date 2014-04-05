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
});


