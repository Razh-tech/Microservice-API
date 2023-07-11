[Home](../../readme.md) >

## Getting Started (Memulai)

Langkah-langkah awal untuk memulai menggunakan KLG Boilerplate & Library.
Di sini, dijelaskan cara untuk melakukan clone boilerplate code dari git repository,
instalasi PostgreSQL dan Kafka, serta menjalankan aplikasi.
Bagian ini sangat penting untuk dipahami oleh developer yang ingin menggunakan
KLG Boilerplate & Library karena merupakan langkah awal yang harus dilakukan
sebelum dapat mulai mengembangkan aplikasi dengan menggunakan boilerplate dan library tersebut.
Dengan mengikuti langkah-langkah yang dijelaskan di bagian ini,
developer dapat memulai pengembangan aplikasi dengan lebih mudah dan cepat.

### <a name="CloneBoilerplate"></a>1. Clone boilerplate code dari git repository
Untuk mendapatkan kode boilerplate, lakukan hal-hal berikut ini:
1. Buka terminal atau command prompt pada komputer Anda.
2. Pindah ke direktori atau folder di mana Anda ingin menyimpan repositori Git yang di-clone.
3. Jalankan perintah "git clone" diikuti dengan URL repository Git yang ingin Anda clone. Seperti ini:

    ```
    git clone https://devops.klgsys.com/KLG-IT-CORP/KawanLamaPlatform/_git/KLG.Mono
    ```
4. Tunggu proses cloning selesai. Setelah selesai, Anda akan memiliki salinan repository Git pada direktori yang Anda tentukan di langkah 2.

Jika Anda memerlukan akses ke repository tersebut, pastikan bahwa Anda memiliki izin akses dan autentikasi yang sesuai sebelum mencoba melakukan cloning.

### <a name="InstalasiDocker"></a>2. Instalasi Docker container
Docker container berfungsi sebagai platform virtualisasi yang memungkinkan pengguna untuk
membuat dan menjalankan aplikasi dalam lingkungan terisolasi.
Dengan menggunakan Docker, pengguna dapat mempercepat proses pengembangan, pengujian, dan penyebaran aplikasi.
Berikut adalah langkah-langkah untuk menginstal Docker:

1. Pastikan sistem operasi yang digunakan mendukung Docker. Docker dapat diinstal pada sistem operasi Windows, macOS, dan Linux.

2. Unduh Docker dari situs web resmi Docker. Untuk sistem operasi Windows dan macOS, unduh Docker Desktop. Untuk sistem operasi Linux, unduh Docker Engine.

3. Setelah unduhan selesai, buka file instalasi dan ikuti instruksi yang diberikan.

4. Setelah instalasi selesai, buka terminal atau command prompt dan jalankan perintah "docker version" untuk memastikan Docker telah terinstal dengan benar.

5. Untuk memulai menggunakan Docker, jalankan perintah "docker run" di terminal atau command prompt.

6. Untuk mempelajari lebih lanjut tentang Docker, kunjungi situs web resmi Docker dan pelajari dokumentasi dan tutorial yang tersedia.

**Catatan:** Pastikan sistem operasi yang digunakan mendukung virtualisasi dan telah diaktifkan. Jika tidak, Docker tidak akan berfungsi dengan baik.

### <a name="InstalasiPostgreSQL"></a>3. Instalasi PostgreSQL
Untuk menginstal PostgreSQL, jalankan perintah berikut pada command line:

```
docker run --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgres
```
Perintah ini akan membuat container Docker bernama "postgres" yang berjalan dengan port 5432 untuk
koneksi dan variabel lingkungan POSTGRES_PASSWORD diatur sebagai "mysecretpassword".

### <a name="InstalasiKafka"></a>4. Instalasi Kafka
Untuk menginstal Kafka, jalankan perintah berikut pada command line:

```
docker run --name kafka -p 9092:9092 -d bashj79/kafka-kraft
```
Perintah ini akan membuat container Docker bernama "kafka" yang berjalan dengan port 9092
untuk koneksi dan menggunakan image docker "bashj79/kafka-kraft".


### <a name="MenjalankanAplikasi"></a>5. Menjalankan aplikasi

Boilerplate sudah dikonfigurasi menggunakan nilai default.
Jika anda mengikuti semua langkah sebelumnya dengan tepat, maka aplikasi bisa langsung dijalankan.
Caranya:
1. Buka aplikasi Visual Studio
2. Pilih Project `KLG.Sample.Boilerplate`, lalu klik kanan dan pilih _"Set As Startup Project"_
3. Run

**Catatan:** Jika terjadi kesalahan, perhatikan pesan kesalahan yang dapat menjadi petunjuk.
Jika anda menggunakan instalasi yang berbeda (tidak sesuai langkah di atas),
maka variabel konfigurasi kemungkinan akan berbeda,
dan ini anda perlu menyesuaikan file appsettings.json sebelum bisa menjalankan aplikasi.

