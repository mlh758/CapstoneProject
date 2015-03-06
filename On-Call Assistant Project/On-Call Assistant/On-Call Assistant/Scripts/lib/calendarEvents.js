$(document).ready(function () {

    // page is now ready, initialize the calendar...
    
    $('#calendar').fullCalendar({
        theme: true,
        editable: false,
        events: "/Home/RotationData"

    });
    


});