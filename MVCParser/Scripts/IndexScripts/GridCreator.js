
let GridRows = new Array();
let GridColumns = new Set();
let GridWidth = 800;
let GridHeight = 400;
let defaultSortCoumnName = 'Id';
let defaultSortDirection = 'ASC';

$(document).ready(function () {
    $.ajax(
        {
            type: 'POST',
            url: "Home/GetAllColumns",
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                let data = response;
                ShowSourceColumns(data);
            }
        })
});

function ShowSourceColumns(data)
{

    let columns = document.getElementById('columns');
    for (var i = 0; i < data.length; i++) {
        let item = data[i];
        let label = document.createElement('label');
        label.classList.add('container');
        label.innerHTML = item.Name;
        let input = document.createElement('input');
        input.type = 'checkbox';
        let span = document.createElement('span');
        span.classList.add('checkmark');
        label.appendChild(input);
        label.appendChild(span);
        columns.appendChild(label);

    }
    let button = document.createElement('input');
    button.type = 'button';
    button.classList.add('button');
    button.value = 'Load';
    columns.appendChild(button);
    button.addEventListener('click', CreateGrid);
}
function CreateGrid()
{

    ClearGrid();
    let ColumnsToShowInTheGrid = new Array();
    var items = document.querySelectorAll('input[type="checkbox"]');
    for (let item of items) {
        if (item.checked) {
            ColumnsToShowInTheGrid.push(item.parentElement.innerText);
        }
    }
    if (items.length > 0) {
        $.ajax(
            {
                type: 'POST',
                url: "Home/GetData",
                dataType: "JSON",
                contentType: "application/json; charset=utf-8",
                traditional: true,
                data: JSON.stringify({ myKey: ColumnsToShowInTheGrid, sortColumn: defaultSortCoumnName, sortDirection: defaultSortDirection }),
                success: function (response) {
                    let data = response;
                    FillGrid(data, ColumnsToShowInTheGrid);
                }
            })
    }


}

function ClearGrid()

{
    var table = document.getElementById('table');
    table.remove();
    var newTable = document.createElement('table');
    newTable.id = 'table';
    var div = document.getElementById('grid');
    div.append(newTable);
}


function FillGrid(data, collumnsToShow) {
      
    GridRows = [];
    GridColumns.clear();
    for (var i = 0; i < data.length; i++) {
        let item = data[i];
        let row = new Map(Object.entries(item));

        for (let j = 0; j < collumnsToShow.length; j++) {
            GridColumns.add(collumnsToShow[j]);
        }  
        GridRows.push(row);
    }
    let table = document.getElementById('table');
    for (item of GridColumns)
    {
        let th = document.createElement('th');
        th.innerHTML = item;
        table.appendChild(th);
    }
    for (let row of GridRows)
    {
        let tr = document.createElement('tr');
        for (columnName of GridColumns) {
            let value = row.get(columnName);
            let td = document.createElement('td');
            if (value != undefined)
            {
                if (value.length > columnName.length * 3)
                {
                    let subValue = value.slice(0, columnName.length * 3) + '...';
                    value = subValue;
                }

                if (value.toString().includes("Date"))
                {                   
                    value = formatDate(value.toString());                  
                }
            }
            td.innerHTML = value;          
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    let div = document.getElementById('grid');
    div.style.height = GridHeight+'px';

    if (table.offsetWidth < GridWidth)
    {
        div.style.width = table.offsetWidth + 'px';
    }
}

function formatDate(value)
{
    let startIndex = value.indexOf('(') + 1;
    let subValue = value.slice(startIndex);
    subValue = subValue.slice(0, subValue.length - 2);
    let date = new Date(parseInt(subValue));
    let day = date.getDate();
    day = (day < 10) ? '0' + day : day;
    let month = date.getMonth() + 1;
    month = (month < 10) ? '0' + month : month;
    let formatedDate = day + '/' + month + '/' + date.getFullYear();
    return formatedDate;
}