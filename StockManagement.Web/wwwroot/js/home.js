
document.addEventListener("DOMContentLoaded", function () {

    console.log("Home dashboard loaded");

    loadSalesChart();
    loadPurchaseChart();
    loadProfitChart();

});


// =====================
// 🌍 GLOBAL CONSTANTS
// =====================

const MONTHS_TR = [
    "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
    "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
];


// =====================
// 📈 SALES
// =====================

function loadSalesChart() {

    fetch('/Home/GetSalesChart')
        .then(r => r.json())
        .then(data => {

            data.sort((a, b) => a.Month - b.Month);

            const labels = data.map(x => MONTHS_TR[x.Month - 1]);
            const values = data.map(x => x.Total);

            new Chart(document.getElementById('salesChart'), {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Satış',
                        data: values,
                        borderColor: '#2a5298',
                        backgroundColor: 'rgba(42,82,152,0.15)',
                        tension: 0.3
                    }]
                },
                options: {
                    responsive: false,
                    maintainAspectRatio: false
                }
            });

        });

}


// =====================
// 📊 PURCHASE
// =====================

function loadPurchaseChart() {

    fetch('/Home/GetPurchaseChart')
        .then(r => r.json())
        .then(data => {

            data.sort((a, b) => a.Month - b.Month);

            const labels = data.map(x => MONTHS_TR[x.Month - 1]);
            const values = data.map(x => x.Total);

            new Chart(document.getElementById('purchaseChart'), {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Aylık Alış Tutarı',
                        data: values,
                        backgroundColor: 'rgba(30, 60, 114, 0.85)',
                        borderRadius: 8,
                        barThickness: 35
                    }]
                },
                options: {
                    responsive: false,
                    maintainAspectRatio: false,
                    plugins: {
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    return context.raw.toLocaleString('tr-TR') + ' ₺';
                                }
                            }
                        }
                    },
                    scales: {
                        y: {
                            ticks: {
                                callback: function (value) {
                                    return value.toLocaleString('tr-TR') + ' ₺';
                                }
                            }
                        }
                    }
                }
            });

        });

}


// =====================
// 💰 PROFIT
// =====================

function loadProfitChart() {

    fetch('/Home/GetProfitChart')
        .then(r => r.json())
        .then(data => {

            const sales = data.Sales;
            const purchases = data.Purchases;
            const profit = data.Profit;

            new Chart(document.getElementById('profitChart'), {
                type: 'doughnut',
                data: {
                    labels: ['Satış', 'Alış', 'Kâr'],
                    datasets: [{
                        data: [sales, purchases, profit],
                        backgroundColor: [
                            'rgba(42, 82, 152, 0.9)',
                            'rgba(30, 60, 114, 0.9)',
                            'rgba(40, 167, 69, 0.9)'
                        ],
                        borderWidth: 2,
                        borderColor: '#fff',
                        hoverOffset: 10
                    }]
                },
                options: {
                    responsive: false,
                    maintainAspectRatio: false,
                    plugins: {
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    return context.label + ': ' +
                                        context.raw.toLocaleString('tr-TR') + ' ₺';
                                }
                            }
                        }
                    }
                }
            });

        });

}
