
const monthsTR = [
    "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
    "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
];

$(function () {

    initDates();
    loadSalesReportGrid();
    bindFilter();

});


// =====================
// 📅 DEFAULT DATE (1 AY)
// =====================

function initDates() {

    const today = new Date();
    const lastMonth = new Date();
    lastMonth.setMonth(lastMonth.getMonth() - 1);

    document.getElementById("startDate").value = formatDate(lastMonth);
    document.getElementById("endDate").value = formatDate(today);
}

function formatDate(date) {
    const d = new Date(date);
    const month = ("0" + (d.getMonth() + 1)).slice(-2);
    const day = ("0" + d.getDate()).slice(-2);
    return `${d.getFullYear()}-${month}-${day}`;
}


// =====================
// 📊 GRID
// =====================

function loadSalesReportGrid() {

    const store = {
        load: function () {

            const startDate = document.getElementById("startDate").value;
            const endDate = document.getElementById("endDate").value;

            const url =
                `/Report/GetSalesDetail?startDate=${startDate ?? ""}&endDate=${endDate ?? ""}`;

            return fetch(url)
                .then(r => r.json())
                .then(res => res.data);
        }
    };

    $("#salesReportGrid").dxDataGrid({

        dataSource: store,

        showBorders: true,
        rowAlternationEnabled: true,

        filterRow: { visible: true },

        searchPanel: {
            visible: true,
            placeholder: "Ara..."
        },

        paging: { pageSize: 20 },

        columns: [

            { dataField: "CategoryName", caption: "Kategori" },
            { dataField: "ProductName", caption: "Ürün" },
            { dataField: "CustomerName", caption: "Müşteri" },
            { dataField: "Quantity", caption: "Miktar", format: "#,##0.##" },
            { dataField: "SalesPrice", caption: "Satış Fiyatı", format: "#,##0.00" },
            { dataField: "DiscountRate", caption: "İskonto %", format: "#,##0.00" },
            { dataField: "Date", caption: "Tarih", dataType: "date", format: "dd.MM.yyyy" },
            { dataField: "TotalQuantity", caption: "Toplam Satış Miktarı", format: "#,##0.##" }
        ]
    });

}


// =====================
// 🔘 FILTER BUTTON
// =====================

function bindFilter() {

    $("#filterBtn").on("click", function () {

        const grid = $("#salesReportGrid").dxDataGrid("instance");
        grid.refresh();
    });
}