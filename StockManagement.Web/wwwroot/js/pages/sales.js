
$(function () {
    loadSalesGrid();
});

function loadSalesGrid() {

    const store = {
        load: function () {
            return fetch('/Sales/Get')
                .then(r => r.json())
                .then(res => res.data);
        }
    };

    $("#salesGrid").dxDataGrid({

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
                dataField: "CustomerName",
                caption: "Müşteri"
            },

            {
                dataField: "ProductName",
                caption: "Ürün"
            },

            {
                dataField: "Quantity",
                caption: "Miktar",
                format: "#,##0.##"
            },

            {
                dataField: "ListPrice",
                caption: "Liste Fiyatı",
                format: "#,##0.00"
            },

            {
                dataField: "SalesPrice",
                caption: "Satış Fiyatı",
                format: "#,##0.00"
            },

            {
                dataField: "DiscountRate",
                caption: "İskonto %",
                format: "#,##0.00"
            },

            {
                dataField: "Amount",
                caption: "Tutar",
                format: "#,##0.00"
            },

            {
                dataField: "Date",
                caption: "Tarih",
                dataType: "date",
                format: "dd.MM.yyyy"
            }
        ]
    });

}