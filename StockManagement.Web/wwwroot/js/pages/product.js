
$(function () {
    loadProductGrid();
});

function loadProductGrid() {

    const store = {
        load: function () {
            return fetch('/Product/Get')
                .then(r => r.json())
                .then(res => res.data);
        }
    };

    $("#gridContainer").dxDataGrid({
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
                dataField: "CategoryName",
                caption: "Kategori"
            },

            {
                dataField: "Name",
                caption: "Ürün Adı"
            },

            {
                dataField: "ImageSrc",
                caption: "Resim Yolu"
            },

            {
                dataField: "Salesprice",
                caption: "Satış Fiyatı",
                format: "#,##0.00"
            },

            {
                caption: "Aksiyon",
                width: 120,
                alignment: "center",

                cellTemplate: function (container, options) {

                    $("<button>")
                        .addClass("btn btn-primary btn-sm")
                        .text("Düzenle")
                        .on("click", function () {
                            window.location = "/Product/Edit/" + options.data.Id;
                        })
                        .appendTo(container);
                }
            }
        ]
    });

}