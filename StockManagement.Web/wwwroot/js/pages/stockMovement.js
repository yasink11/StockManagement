
$(function () {
    loadStockMovementGrid();
});

function loadStockMovementGrid() {

    const store = {
        load: function () {
            return fetch('/Stock/Get')
                .then(r => r.json())
                .then(res => res.data);
        }
    };

    $("#stockMovementGrid").dxDataGrid({

        dataSource: store,

        showBorders: true,
        rowAlternationEnabled: true,

        filterRow: {
            visible: true
        },

        searchPanel: {
            visible: true,
            placeholder: "Ara..."
        },

        paging: {
            pageSize: 15
        },

        columns: [

            {
                dataField: "Id",
                caption: "ID",
                width: 70
            },

            {
                dataField: "ProductName",
                caption: "Ürün"
            },

            {
                dataField: "CategoryName",
                caption: "Kategori"
            },

            {
                dataField: "Quantity",
                caption: "Miktar",
                format: "#,##0.##"
            },

            {
                dataField: "Date",
                caption: "Tarih",
                dataType: "date",
                format: "dd.MM.yyyy HH:mm"
            }
        ]
    });

}