
$(function () {
    loadStockReportGrid();
});

function loadStockReportGrid() {

    const store = {
        load: function () {
            return fetch('/Report/GetStockReport')
                .then(r => r.json())
                .then(res => res.data);
        }
    };

    $("#stockGrid").dxDataGrid({

        dataSource: store,

        showBorders: true,

        filterRow: {
            visible: true
        },

        searchPanel: {
            visible: true,
            placeholder: "Ara..."
        },

        paging: {
            pageSize: 20
        },

        columns: [

            {
                dataField: "ProductId",
                caption: "Ürün ID",
                width: 90
            },

            {
                dataField: "ProductName",
                caption: "Ürün Adı"
            },

            {
                dataField: "CategoryName",
                caption: "Kategori"
            },

            {
                dataField: "TotalQuantity",
                caption: "Toplam Stok",
                format: "#,##0.##"
            }
        ]
    });

}