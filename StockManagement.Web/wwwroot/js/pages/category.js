
$(function () {

    loadCategoryGrid();

});

function loadCategoryGrid() {

    const store = {
        load: function () {
            return fetch('/Category/Get')
                .then(r => r.json())
                .then(res => res.data);
        }
    };

    $("#gridContainer").dxDataGrid({
        dataSource: store,
        showBorders: true,
        rowAlternationEnabled: true,
        filterRow: { visible: true },
        searchPanel: { visible: true },
        paging: { pageSize: 10 },

        columns: [
            { dataField: "Id", caption: "Id", width: 80 },
            { dataField: "Name", caption: "Kategori Adı" },

            {
                caption: "Aksiyon",
                width: 120,
                alignment: "center",
                cellTemplate: function (container, options) {

                    $("<button>")
                        .addClass("btn btn-primary btn-sm")
                        .text("Düzenle")
                        .on("click", function () {
                            window.location = "/Category/Edit/" + options.data.Id;
                        })
                        .appendTo(container);
                }
            }
        ]
    });

}