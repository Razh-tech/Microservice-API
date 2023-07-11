# KLG Application Development Framework

## Getting Started

### A. Instalasi 

1. [Clone boilerplate code dari git repository](docs/getting-started/getting-started.md#CloneBoilerplate)

2. [Instalasi Docker](docs/getting-started/getting-started.md#InstalasiDocker)

3. [Instalasi PostgreSQL](docs/getting-started/getting-started.md#InstalasiPostgreSQL)

4. [Instalasi Kafka](docs/getting-started/getting-started.md#InstalasiKafka)

5. [Menjalankan aplikasi](docs/getting-started/getting-started.md#MenjalankanAplikasi)

### B. Menggunakan Swagger UI untuk menguji API

1. [Mendapatkan access token](docs/getting-started/using-swaggerui.md#accesstoken)

2. [Reset databasse](docs/getting-started/using-swaggerui.md#resetdb)

3. [Melihat data dan memasukkan data baru](docs/getting-started/using-swaggerui.md#crud)

### C. Boilerplate
1. Clone aplikasi Front-end
2. Menjalankan aplikasi
3. Contoh login
4. Contoh menampilkan menu
5. Contoh menampilkan daftar entry
6. Contoh menambahkan, edit, dan delete entry data
7. Deployment


## Tutorial

### A. How to Create a Front-End application
1.
2.
3.
4.
5.

### B. How to Create a Back-end application

1. Menentukan konfigurasi awal microservice 
2. Membuat dan mendapatkan user access token serta permission
3. Membuat Entity model dan menyesuaikan DbContext
4. Membuat API controller
5. Membaca dan menyimpan ke database
6. Reset database menggunakan excel file
7. Mengirim dan menerima message
8. Menulis log & metric


## Back-End Reference

### A. Microservice
1. Service Life-cycle
2. Menentukan konfigurasi microservice

### B. Configuration
1. Menggunakan appsettings untuk konfigurasi local development
2. Menggunakan Vault Service untuk mengakses remote resources
3. Membuat custom configuration

### C. Security
1. Konsep asymetric key dan generate private/public key
2. Konsep access token dan refresh token
3. Generate access token
4. Setting public key dari identity service
5. Mendapatkan user info dan claims dari access token
6. Mengelola user permission
7. Mendapatkan permission (daftar aplikasi dan menu)

### D. Database
1. Setting Database, user, dan password secara manual
2. Membuat Entity model dan DbContext
3. Mendapatkan DbProvider dan DbContext dari DI Service
4. Mengakses record database dan menggunakan Transaction
5. Reset table dan database

### E. Message Stream
1. Setting message broker secara manual
2. Konsep outbox pattern
3. Mengirim (publish) message ke message broker
4. Menerima (consume) message secara serial
5. Menerima (consume) message secara paralel
6. Idempotency check
7. Compensating transaction

### F. Log dan Telemetry
1. Setting log dan telemetry secara manual
2. Menulis ke log
3. Menulis ke telemetry
4. Menampilkan log data
5. Menampilkan telemetry trace

### G. File dan Blob Storage
1.
2.

### H. Notifikasi
1.
2.

### I. Generate E-Mail
1.
2.

### J. Generate Report
1.
2.

### K. Utilities
1. Layanan document numbering
2.

## Front-End Components

### ...
