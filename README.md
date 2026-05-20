
# 📦 Stock Management System

![.NET](https://img.shields.io/badge/.NET-ASP.NET%20Core-blue)
![MSSQL](https://img.shields.io/badge/Database-MS%20SQL%20Server-red)
![UI](https://img.shields.io/badge/UI-Bootstrap%205-purple)
![Charts](https://img.shields.io/badge/Charts-Chart.js-green)
![Grid](https://img.shields.io/badge/Grid-DevExtreme-orange)
![Status](https://img.shields.io/badge/Project-Completed-brightgreen)

---

## 🧠 Proje Açıklaması

ASP.NET Core MVC tabanlı bu proje; stok, satış, alış ve raporlama süreçlerini yöneten, dashboard ve grafiklerle desteklenmiş modern bir iş yönetim sistemidir.

Gerçek iş mantığı içeren bir yapı üzerine kurulmuştur (stok kontrolü, iskonto hesaplama, gelir-gider analizi).

---

## 🚀 Özellikler

- 📦 Ürün, müşteri ve kategori yönetimi
- 💰 Satış & alış işlemleri
- 📉 Otomatik iskonto hesaplama
- 📊 Stok takibi ve stok hareketleri
- 📈 Dashboard (Chart.js ile grafikler)
- 📋 DevExtreme DataGrid ile modern tablolar
- ⚡ AJAX tabanlı dinamik veri çekme
- 🔄 Gerçek zamanlı stok kontrol sistemi

---

## 🧱 Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework (Database First)
- MS SQL Server
- DevExtreme DataGrid
- Chart.js
- jQuery
- Bootstrap 5

---

## 🗄️ Veritabanı Kurulumu

Database script:

/backup/StockManagement.sql

### Kurulum:

1. SQL Server Management Studio (SSMS) açılır  
2. `StockManagement.sql` çalıştırılır  
3. Database otomatik oluşur  
4. Connection string güncellenir  

---

## 📌 İş Kuralları

### 🧾 Satış

- Ürün seçildiğinde liste fiyatı otomatik gelir  
- Satış fiyatı kullanıcı tarafından değiştirilebilir  
- İskonto otomatik hesaplanır:

DiscountRate = ((ListPrice - SalesPrice) / ListPrice) * 100

- Satış sırasında stok kontrolü yapılır  
- Stok negatif olamaz  
- Satış Stock tablosuna işlenir  

---

### 📦 Alış

- Alış işlemleri Stock tablosuna pozitif hareket olarak eklenir  
- Ürün ve tedarikçi bilgisi ile kayıt yapılır  

---

### 📊 Stok Yönetimi

- Tüm stok hareketleri tek tabloda tutulur  
- Anlık stok hesaplaması hareketlerden yapılır  

---

## 📈 Raporlar

### 1. Kategori Bazlı Satış Raporu
- Kategoriye göre satış analizi  
- Tarih filtreleme  
- En çok satılan ürün sıralaması  

### 2. Stok Raporu
- Ürün bazlı toplam stok  
- Hareket bazlı hesaplama  

---

## 📊 Dashboard

- 📈 Aylık Satış Grafiği  
- 📉 Aylık Alış Grafiği  
- 💰 Kâr / Zarar Analizi  

Chart.js ve AJAX ile dinamik veri çekilmektedir.

---

## 📊 Dashboard Görünümü

<p align="center" style="display:flex; gap:10px; justify-content:center; align-items:stretch;">

  <img src="https://github.com/user-attachments/assets/3c1e3575-26cf-468b-9fc5-b11c53530045"
       style="width: 32%; height: 260px; object-fit: cover; border-radius: 8px;" />

  <img src="https://github.com/user-attachments/assets/78f370e4-cc88-42ba-9721-a453b3ed32c7"
       style="width: 32%; height: 260px; object-fit: cover; border-radius: 8px;" />

  <img src="https://github.com/user-attachments/assets/4ba3ae00-02fc-4403-aef7-52a33bb79aab"
       style="width: 32%; height: 260px; object-fit: cover; border-radius: 8px;" />

</p>


## 🎯 Öne Çıkanlar

- ✔ Modular JavaScript architecture
- ✔ Gerçek zamanlı stok kontrolü
- ✔ Finansal hesaplama mantığı
- ✔ Dashboard + raporlama sistemi
- ✔ Clean MVC structure

---

## ⚙️ Kurulum

git clone <repo-url>

1. Database kur (`backup/StockManagement.sql`)
2. Connection string ayarla
3. Run project

---

- stok yönetim sistemi  
- satış/alış finansal hesaplama  
- veri görselleştirme  
- gerçek dünya business logic  

içeren tam bir yönetim panelidir.
