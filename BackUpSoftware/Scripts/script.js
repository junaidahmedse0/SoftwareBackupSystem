
$(document).ready(function () {
   
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

function confirm(id) {
  

    bootbox.confirm("Are You Sure You Want To Delete??", function (result) {
        if (result) {
            $.ajax({
                url: "/Admin/Delete?id=" + id,
                method: "Post",
                success: function (status) {
                    console.log("Success:: " + status)
                    $( "#"+id).remove();
           
                },
                error: function (status) {
                    console.log("Error:: "+status)
                }
            });
        }
    });

};


function folder(path) {

    $.ajax({
        url: "/User/Index?path=" + path,
        method: "GET",
        success: function (status) {
            console.log("Success:: " + status)
            alert("Success:: " + status);

        },
        error: function (status) {
            console.log("Error:: " + status)
            alert("Error:: " + status);

        }
    });

};

function UserDetails(id)
{

    $.ajax({
        type: "GET",
        url: "/Admin/Details?id=" + id,

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#detail-firstname').text(response.FirstName);
            $('#detail-lastname').text(response.LastName);
            $('#detail-email').text(response.Email);
            $('#detail-password').text(response.Password);
            $('#detail-contact').text(response.Contact);
            $('#detail-country').text(response.Country);
            $('#DetailModal').modal('show');
           

        },
        error: function (response) {

            alert("Error::" + response);

        }
    });
}

function EditUser(id) {

    $.ajax({
        type: "GET",
        url: "/Admin/Edit?id=" + id,

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#Id').val(id);
            $('#FirstName').val(response.FirstName);
           $('#FirstName').val(response.FirstName);
            $('#LastName').val(response.LastName);
            $('#Email').val(response.Email);
            $('#Password').val(response.Password);
            $('#Contact').val(response.Contact);
            $('#Country').val(response.Country);
            $('#EditModal').modal('show');


        },
        error: function (response) {

            alert("Error::" + response);

        }
    });
}