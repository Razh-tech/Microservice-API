# KLG Application Development Framework

KLG Application Development Framework adalah sebuah template atau kerangka kerja yang dirancang untuk
membantu developer dalam membangun aplikasi berkualitas tinggi dengan lebih mudah dan lebih cepat.
KLG Application Development Framework terdiri atas:
- **Boilerplates**, yaitu contoh aplikasi yang dapat digunakan sebagai kerangka awal untuk membangun aplikasi.
- **Library & Components**, yaitu alat bantu untuk membuat aplikasi.

## Getting Started

Berikut ini adalah langkah-langkah yang harus dilakukan oleh developer untuk memulai menggunakan
KLG Application Development Framework. Bagian ini sangat penting untuk dipahami oleh developer
yang ingin menggunakan KLG Boilerplate & Library karena merupakan langkah awal yang harus dilakukan
sebelum dapat mulai mengembangkan aplikasi dengan menggunakan boilerplate dan library tersebut.
Dalam bagian ini, dijelaskan cara untuk melakukan clone boilerplate code dari git repository,
instalasi PostgreSQL dan Kafka, serta menjalankan aplikasi.

Selain itu, terdapat juga penjelasan tentang penggunaan Swagger UI untuk menguji API pada aplikasi yang
dikembangkan menggunakan KLG Boilerplate & Library. Swagger UI adalah sebuah alat pengujian API yang memungkinkan
developer untuk memeriksa dan menguji fungsi-fungsi API dengan mudah dan cepat.

Bagian terakhir dari Getting Started menjelaskan tentang aplikasi contoh (SampleApps) yang dapat
dijadikan referensi oleh developer dalam mengembangkan aplikasi menggunakan KLG Application Development Framework.

### A. Instalasi 

Di sini, dijelaskan cara untuk melakukan clone boilerplate code dari git repository,
instalasi PostgreSQL dan Kafka, serta menjalankan aplikasi.
Bagian ini sangat penting untuk dipahami oleh developer yang ingin menggunakan
KLG Boilerplate & Library karena merupakan langkah awal yang harus dilakukan
sebelum dapat mulai mengembangkan aplikasi dengan menggunakan boilerplate dan library tersebut.
Dengan mengikuti langkah-langkah yang dijelaskan di bagian ini,
developer dapat memulai pengembangan aplikasi dengan lebih mudah dan cepat.

1. [Clone boilerplate code dari git repository](docs/getting-started/instalasi.md#CloneBoilerplate)

2. [Instalasi Docker](docs/getting-started/instalasi.md#InstalasiDocker)

3. [Instalasi PostgreSQL](docs/getting-started/instalasi.md#InstalasiPostgreSQL)

4. [Instalasi Kafka](docs/getting-started/instalasi.md#InstalasiKafka)

5. [Instalasi Seq](docs/getting-started/instalasi.md#InstalasiSeq)

6. [Instalasi Open Telemetry](docs/getting-started/instalasi.md#InstalasiOpenTelemetry)

7. [Koneksi ke Artifact Feed](docs/getting-started/instalasi.md#KoneksiArtifactFeed)

8. [Menjalankan aplikasi](docs/getting-started/instalasi.md#MenjalankanAplikasi)

### B. Menggunakan Swagger UI untuk menguji API

Pada bagian ini, akan dijelaskan cara menggunakan Swagger UI untuk menguji API
pada aplikasi yang dikembangkan menggunakan KLG Boilerplate & Library.
Swagger UI adalah sebuah alat pengujian API yang memungkinkan developer untuk memeriksa dan
menguji fungsi-fungsi API dengan mudah dan cepat.
Dalam bagian ini, dijelaskan langkah-langkah untuk mendapatkan access token,
melihat dan memasukkan data baru ke dalam aplikasi menggunakan Swagger UI,
serta mereset database jika diperlukan. Dengan mengikuti langkah-langkah yang dijelaskan di bagian ini,
developer dapat lebih mudah dan cepat memeriksa dan menguji API pada aplikasi yang dikembangkan.

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

Bagian tutorial ini akan membahas langkah-langkah untuk membuat aplikasi front-end dan back-end menggunakan KLG Application Development Framework. Pada bagian ini, Anda akan diajarkan cara membuat aplikasi dari awal, termasuk cara mengatur konfigurasi, membuat dan mengintegrasikan entity model, membuat API controller, mengakses database, dan mengirim serta menerima pesan melalui message stream. Selain itu, Anda juga akan mempelajari bagaimana cara menulis log dan metric, mengelola file dan blob storage, mengirim notifikasi, membuat e-mail dan report, serta menggunakan layanan utilities seperti document numbering.

Dalam tutorial ini, kami akan membahas dua bagian utama: cara membuat aplikasi front-end dan cara membuat aplikasi back-end. Bagian pertama akan membahas cara membuat aplikasi front-end dari awal, termasuk cara mengunduh boilerplate code dan menjalankan aplikasi. Sedangkan bagian kedua akan membahas cara membuat aplikasi back-end, termasuk cara mengonfigurasi microservice, mengelola database, dan mengirim serta menerima pesan melalui message stream.

Anda tidak perlu memiliki pengetahuan yang mendalam tentang KLG Application Development Framework untuk mengikuti tutorial ini. Namun, pengetahuan dasar tentang pemrograman web dan penggunaan database akan sangat membantu. Selain itu, sebelum memulai tutorial ini, pastikan Anda sudah mengikuti langkah-langkah pada bagian "Getting Started" untuk mengunduh dan menginstal semua yang diperlukan untuk menggunakan KLG Application Development Framework.

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

Bagian ini menyediakan panduan dan referensi bagi pengembang untuk membuat aplikasi back-end
menggunakan KLG Application Development Framework. Dalam bagian ini, kami akan membahas berbagai topik
terkait mikroservices, konfigurasi, keamanan, database, message stream, log dan telemetri,
penyimpanan file dan blob, notifikasi, serta utilitas lainnya.

Untuk membangun aplikasi back-end yang andal dan efisien, kami menyarankan agar pengembang
mempelajari topik-topik yang tercakup di dalam bagian ini.
Setiap topik dijelaskan secara detail dengan bahasa teknis, dan dilengkapi dengan contoh penggunaan.
Bagian ini juga berisi referensi untuk berbagai komponen yang terdapat pada KLG Application Development Framework.

Setelah membaca bagian ini, diharapkan pengembang dapat memahami dan menerapkan berbagai konsep dan teknologi yang dibutuhkan dalam pengembangan aplikasi back-end menggunakan KLG Application Development Framework.

### A. Microservice
Bagian ini membahas tentang life-cycle, konfigurasi, dan keamanan microservice.
Life-cycle microservice meliputi hal-hal yang dilakukan package library secara garis besar.
Konfigurasi microservice dapat dilakukan secara lokal maupun dengan menggunakan Vault Service
untuk mengakses sumber daya yang ada di server jarak jauh.
Selain itu, bagian ini juga membahas tentang konsep asimetrik key, access token, dan refresh token
sebagai langkah-langkah keamanan dalam pengembangan microservice.

1. [Service Life-cycle](docs/be-references/microservice.md#service-lifecycle)

2. [Menentukan konfigurasi microservice](docs/be-references/microservice.md#konfigurasi)

### B. Configuration
Bagian ini membahas tentang konfigurasi pada aplikasi, baik pada level local development maupun remote resources.
Konfigurasi dapat dilakukan dengan menggunakan appsettings pada lingkungan lokal maupun Vault Service untuk
mengakses sumber daya yang ada di remote server. Selain itu, bagian ini juga membahas tentang pembuatan
custom configuration.

1. [Menggunakan appsettings untuk konfigurasi local development](docs/be-references/configuration.md#local-config)
2. [Menggunakan Vault Service untuk mengakses remote resources](docs/be-references/configuration.md#vault-service)
3. [Membuat custom configuration](docs/be-references/configuration.md#custom-configuration)

### C. Security
Bagian ini membahas tentang konsep keamanan pada aplikasi, seperti penggunaan public key,
access token, dan refresh token. Bagian ini juga membahas tentang bagaimana cara generate access token
serta cara mengakses public key dari identity service. Selain itu, bagian ini juga membahas tentang
cara mengelola user permission dan mendapatkan daftar aplikasi serta menu yang ada pada aplikasi.

1. [Konsep asymetric key dan generate private/public key](docs/be-references/security.md#asymetrickey)

2. [Konsep access token dan refresh token](docs/be-references/security.md#token)

3. [Setting public key dari identity service](docs/be-references/security.md#setting)

4. [Generate access token](docs/be-references/security.md#generate)

5. [Mendapatkan user info dan claims dari access token](docs/be-references/security.md#userinfo)

6. [Mengelola user permission](docs/be-references/security.md#managepermission)

7. [Mendapatkan permission (daftar aplikasi dan menu)](docs/be-references/security.md#getpermission)

### D. Database
Bagian ini membahas tentang konfigurasi database, pembuatan Entity model dan DbContext, dan
penggunaan API controller class. Bagian ini juga membahas tentang cara mengakses record database dan
menggunakan Transaction. Selain itu, bagian ini juga membahas tentang cara reset table dan database.

1. [Setting Database, user, dan password secara manual](docs/be-references/database.md#setting)

2. [Membuat Entity model dan DbContext](docs/be-references/database.md#entitymodel)

3. [Mendapatkan DbProvider dan DbContext dari DI Service](docs/be-references/database.md#diservice)

4. [Mengakses record database dan menggunakan Transaction](docs/be-references/database.md#crud)

5. [Update concurrency Check](docs/be-references/database.md#concurrency)

6. [Reset table dan database](docs/be-references/database.md#reset)

### E. Message Streaming
Bagian ini membahas tentang konfigurasi message stream, outbox pattern, serta cara mengirim dan
menerima message. Bagian ini juga membahas tentang cara melakukan idempotency check dan
compensating transaction.

1. [Setting message broker secara manual](docs/be-references/messaging.md#setting)

2. [Konsep outbox pattern](docs/be-references/messaging.md#outbox-pattern)

3. [Mengirim (publish) message ke message streaming](docs/be-references/messaging.md#publish)

4. [Menerima (consume) message secara serial](docs/be-references/messaging.md#consume-serial)

5. [Menerima (consume) message secara paralel](docs/be-references/messaging.md#consume-paralel)

6. [Idempotency check](docs/be-references/messaging.md#idempotency)

7. [Compensating transaction](docs/be-references/messaging.md#compensating)

### F. Log dan Telemetry
Bagian ini membahas tentang pengaturan log dan telemetry pada aplikasi, serta cara menulis dan
menampilkan log data dan telemetry trace. Penggunaan log dan telemetry sangat penting untuk
memantau kinerja aplikasi dan memudahkan proses debugging ketika terjadi masalah.

1. [Setting log dan telemetry secara manual](docs/be-references/logging-and-telemetry.md#setting)
2. [Menulis ke log](docs/be-references/logging-and-telemetry.md#write-to-log)
3. [Menulis ke telemetry](docs/be-references/logging-and-telemetry.md#write-to-telemetry)
4. [Menampilkan log data](docs/be-references/logging-and-telemetry.md#show-log)
5. [Menampilkan telemetry trace](docs/be-references/logging-and-telemetry.md#show-telemetry)

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
