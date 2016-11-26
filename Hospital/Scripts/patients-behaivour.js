$(document).ready(function () {
    $("tr").click(function() {
        var id = $(this).children().html();
        document.location.href = "/Patients/GetPatientInfo?id=" + id;
    });
});