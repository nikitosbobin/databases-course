$(document).ready(function () {
    $("tr").click(function () {
        var id = $(this).children().eq(1).html();
        console.log(id);
        document.location.href = "/Patients/GetPatientInfo?id=" + id;
    });
});