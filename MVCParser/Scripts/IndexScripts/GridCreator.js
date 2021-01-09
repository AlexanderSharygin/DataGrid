
let GridRows = new Array();
let GridColumns = new Set();
let GridWidth = 800;
let GridHeight = 400;
let SortColumnName = 'Id';
let SortDirection = 'ASC';
let ColumnWithDisabledSorting = new Set(['Notes']);
let PageSize = 100;

$(document).ready(function () {
    $.ajax(
        {
            type: 'POST',
            url: "Home/GetAllColumns",
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                let data = response;
                ShowAllExistedColumns(data);
            }
        })
});

function ShowAllExistedColumns(data)
{
    let columns = document.getElementById('columns');
    for (let i = 0; i < data.length; i++) {
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
    let ColumnsToShow = GetSelectedColumns();
    if (ColumnsToShow.length > 0) {
        GetGridDataByAjax(ColumnsToShow);
    }
    let table = document.getElementById('table');
    table.onclick = function (event)
    {
        let target = event.target;
        if (target.tagName == 'TH')
        {
            if (ColumnWithDisabledSorting.has(target.innerHTML))
            {
                return;
            }
            if (target.innerHTML != SortColumnName) {
                SortDirection = 'ASC';
                SortColumnName = target.innerHTML;          
            }
            else
            {
                if (SortDirection == 'ASC')
                {
                    SortDirection = 'DESC';
                }
                else if (SortDirection == 'DESC')
                {
                    SortDirection = 'ASC';
                }

            }
        }
       
        GetGridDataByAjax(ColumnsToShow);
    };   
}

function GetSelectedColumns()
{
    let ColumnsToShow = new Array();
    let items = document.querySelectorAll('input[type="checkbox"]');
    for (let item of items) {
        if (item.checked) {
            ColumnsToShow.push(item.parentElement.innerText);
        }
    }
    return ColumnsToShow;
}



function GetGridDataByAjax(selectedColumns)
{
    $.ajax(
        {
            type: 'POST',
            url: "Home/GetData",
            dataType: "JSON",
          
             beforeSend: function () { // Before we send the request, remove the .hidden class from the spinner and default to inline-block.
              $('#overlay').fadeIn(300);　
           },
            contentType: "application/json; charset=utf-8",
            traditional: true,
            data: JSON.stringify({ myKey: selectedColumns, sortColumn: SortColumnName, sortDirection: SortDirection, pageSize: PageSize }),
            success: function (response) {
                let data = response;
                ClearGrid();
                FillGrid(data[0], selectedColumns);
            },
           complete: function () { // Set our complete callback, adding the .hidden class and hiding the spinner.
               $("#overlay").fadeOut(300);
          },
          
        });

}




function ClearGrid()

{
    let table = document.getElementById('table');
    let tableItems = table.querySelectorAll('td');
    [].forEach.call(tableItems, function (elem) {
        elem.remove();
    });
    tableItems = table.querySelectorAll('tr');
    [].forEach.call(tableItems, function (elem) {
        elem.remove();
    });
  tableItems = table.querySelectorAll('th');
    [].forEach.call(tableItems, function (elem) {
      elem.remove();
   }); 

}


function FillGrid(data, collumnsToShow) {
      
    GridRows = [];
    GridColumns.clear();
    for (let i = 0; i < data.length; i++) {
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
        if (th.innerHTML == SortColumnName)
        {
            if (SortDirection == 'DESC')
            {
                 th.classList.add('DESCsorted');
               
            }
            else if (SortDirection =='ASC') {
                 th.classList.add('ASCsorted');                                    
            }           
        }
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


