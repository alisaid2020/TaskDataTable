
    $(function () {

            // Proxy created on the fly
            var cus = $.connection.myHub;
    // Declare a function on the job hub so the server can invoke it
    var clientServerHub = $.connection.incidentServerHub;
    cus.client.getupdated = function () {
        getData();
            };

    // Start the connection
    $.connection.hub.start();
    getData();
        });

    function getData() {
            var $tbl = $('#tblInfo');
    $.ajax({
    //    url: '@Url.Content("~/Home/Get")',
        url: "/Home/Get",
    type: 'GET',
    datatype: 'json',
    success: function (data) {
                    //$tbl.empty();
                    //$.each(data.listCus, function (i, model) {
                    //    $tbl.append
                    //        (
                    //            '<tr>' +
                    //            '<td>' + model.Id + '</td>' +
                    //            '<td>' + model.CusId + '</td>' +
                    //            '<td>' + model.CusName + '</td>' +
                    //            '<tr>'
                    //        );
                    //});
                    
                    var notifcount = data.listCus.length;
                    if (data.listCus.length > 0) {
                $('#notif').css("background-color", "red");

                    } else
            $('#notif').css("background-color", "Black");

            $('#notif').text(notifcount);

                }
            });
        }
