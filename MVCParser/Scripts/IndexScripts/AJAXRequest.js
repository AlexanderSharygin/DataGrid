

$(document).ready(function () {
    $.ajax(
        {
            type: 'POST',
            url: "Home/WorkerSearch",
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                let data = response;
                OnSuccess(data);
            }
        })
});
let Rows = new Array();
let Columns = new Set();
function OnSuccess(data) {
   
    for (var i = 0; i < data.length; i++) {
        let item = data[i];
        let row = new Map(Object.entries(item));

        for (let j = 0; j < Object.keys(item).length; j++) {
            Columns.add(Object.keys(item)[j]);
        }  
        Rows.push(row);
    }
}