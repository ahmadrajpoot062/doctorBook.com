// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#search').click(function () {       
        var Speciality = $('#speciality').val();
        $.get('/Admin/Search', { category: Speciality }, function (result) {
            $('#list').html(result).fadeIn('slow'); // Clear previous content and add new result
        });
    });
});

