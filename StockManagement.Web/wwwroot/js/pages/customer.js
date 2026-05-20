
$(function () {
    loadCustomerGrid();
});

function loadCustomerGrid() {

    const store = {
        load: function () {
            return fetch('/Customer/Get')
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
            { dataField: "Id", width: 80 },
            { dataField: "Customertitle", caption: "Ünvan" },
            { dataField: "Customernumber", caption: "Numara" },

            {
                caption: "Aksiyon",
                width: 120,
                alignment: "center",
                cellTemplate: function (container, options) {

                    $("<button>")
                        .addClass("btn btn-primary btn-sm")
                        .text("Düzenle")
                        .on("click", function () {
                            window.location = "/Customer/Edit/" + options.data.Id;
                        })
                        .appendTo(container);
                }
            }
        ]
    });

}