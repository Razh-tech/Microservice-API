[Home](../../readme.md) >

## Instalasi

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
    git clone https://devops.klgsys.com/KLG-IT-CORP/KawanLamaPlatform/_git/Sample
    ```
4. Tunggu proses cloning selesai. Setelah selesai, Anda akan memiliki salinan repository Git pada direktori yang Anda tentukan di langkah 2.

Jika Anda memerlukan akses ke repository tersebut, pastikan bahwa Anda memiliki izin akses dan autentikasi yang sesuai sebelum mencoba melakukan cloning.

### <a name="InstalasiDocker"></a>2. Instalasi Docker container
Docker container berfungsi sebagai platform virtualisasi yang memungkinkan pengguna untuk
membuat dan menjalankan aplikasi dalam lingkungan terisolasi.
Dengan menggunakan Docker, pengguna dapat mempercepat proses pengembangan, pengujian, dan penyebaran aplikasi.
Berikut adalah langkah-langkah untuk menginstal Docker:

1. Pastikan sistem operasi yang digunakan mendukung Docker. Docker dapat diinstal pada sistem operasi Windows, macOS, dan Linux.

2. Unduh Docker dari situs web resmi Docker (https://docs.docker.com/get-docker/). Untuk sistem operasi Windows dan macOS, unduh Docker Desktop. Untuk sistem operasi Linux, unduh Docker Engine.

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

Untuk mencoba akses ke database, bisa menggunakan IDE yang disukai.
Salah satu pilihannya adalah menggunakan PgAdmin.
Untuk menginstal PgAdmin, jalankan perintah berikut pada command line:

```
docker pull dpage/pgadmin4
docker run -p 80:80 \
    -e 'PGADMIN_DEFAULT_EMAIL=user@domain.com' \
    -e 'PGADMIN_DEFAULT_PASSWORD=SuperSecret' \
    -d dpage/pgadmin4
```

**Catatan:** Detil cara instalasi PgAdmin bisa dibaca di [dokumentasi PgAdmin](https://hub.docker.com/r/dpage/pgadmin4/)


Untuk mengakses database postgresql lakukan langkah-langkah berikut:
1. Buka browser di alamat localhost:80
2. Login menggunakan email: "user@domain.com" dan password: "SuperSecret"
3. Pilih menu Object > Register > Server
4. Masukkan Name = "KLG"
5. Pilih tab "Connection"
6. Masukan Host name / address = alamat IP docker postgres

    Cara mengetahui alamat IP docker postgres adalah sbb:
    ```
    docker inspect postgres
    ```

    Sistem akan menampilkan konfigurasi dalam format Json,
    lalu lihat di bagian NetworkSettings > Networks > IPAddress

7. Masukkan Username: "postgres"
8. Masukkan Password: "mysecretpassword"
9. Klik button "Save"

### <a name="InstalasiKafka"></a>4. Instalasi Kafka
Untuk menginstal Kafka, jalankan perintah berikut pada command line:

```
docker run --name kafka -p 9092:9092 -d bashj79/kafka-kraft
```
Perintah ini akan membuat container Docker bernama "kafka" yang berjalan dengan port 9092
untuk koneksi dan menggunakan image docker "bashj79/kafka-kraft".

### <a name="InstallasiSeq"></a>5. Instalasi Seq

1. Buka terminal atau command prompt.

2. Tentukan direktori lokal di mana Anda ingin menyimpan data Seq:

```
mkdir -p data
```

3. Jalankan perintah berikut untuk mengunduh image Seq dari Docker Hub, membuat kontainer, dan menjalankannya:

```bash
docker run \
  --name seq \
  -d \
  --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -e SEQ_FIRSTRUN_ADMINPASSWORDHASH="" \
  -v %cd%/data:/data \
  -p 80:80 \
  -p 5341:5341 \
  datalust/seq
```

Ubah `%cd%` menjadi `$(pwd)` jika anda menjalankan perintah di atas dengan macOS atau Linux.

4. Setelah menjalankan perintah di atas, tunggu beberapa saat agar Seq mulai berjalan. Kemudian, buka browser dan akses Seq melalui alamat `http://localhost`.

### <a name="InstallasiOpenTelemetry"></a>6. Instalasi Open Telemetry Collector

1. **Buat berkas konfigurasi**

   Buat berkas konfigurasi bernama `otel-collector-config.yaml`, berikut merupakan contoh konten:

   ```yaml
   receivers:
     otlp:
       protocols:
         grpc:
         http:
   
   processors:
     batch:
   
   exporters:
     logging:
       loglevel: debug
   
   service:
     pipelines:
       traces:
         receivers: [otlp]
         processors: [batch]
         exporters: [logging]
   ```

   Berkas konfigurasi ini akan digunakan untuk mengatur OpenTelemetry Collector.

2. **Jalankan OpenTelemetry Collector dengan Docker**

   Buka terminal atau command prompt, arahkan ke direktori di mana Anda menyimpan berkas `otel-collector-config.yaml`, lalu jalankan perintah berikut:

   ```bash
   docker run --name otel-collector -p 4317:4317 -p 55681:55681 -v %cd%/otel-collector-config.yaml:/otel-collector-config.yaml otel/opentelemetry-collector:latest --config /otel-collector-config.yaml
   ```

   Ubah `%cd%` menjadi `$(pwd)` jika anda menjalankan perintah di atas dengan macOS atau Linux.

   Perintah ini akan melakukan hal berikut:

   - Menamai kontainer dengan `otel-collector`.
   - Memetakan port 4317 dan 55681 dari kontainer ke mesin lokal Anda. Port 4317 adalah port gRPC default, sedangkan 55681 adalah port HTTP default untuk OTLP.
   - Menggabungkan berkas konfigurasi Anda ke dalam kontainer.
   - Menjalankan citra OpenTelemetry Collector terbaru menggunakan berkas konfigurasi Anda.

### <a name="KoneksiArtifactFeed"></a>7. Koneksi ke Artifact Feed
Berikut merupakan langkah yang harus dilakukan untuk dapat menggunakan artifact feed:
1. Buka aplikasi Visual Studio
2. Pilih Project `KLG.Backend.Organization.Services`, lalu klik kanan dan pilih _"Manage Nuget Packages"_
3. Buka Settings, setelah itu tambah packages dengan menekan tombol plus berwarna hijau
4. Tambahkan setting berikut

Name
```
KLGLibraries
```

Source
```
https://devops.klgsys.com/KLG-IT-CORP/_packaging/KLGLibraries/nuget/v3/index.json
```

5. Pastikan untuk menginput kredensial dengan benar
6. Feed siap untuk digunakan

**Catatan:** Jika terjadi kesalahan, perhatikan pesan kesalahan yang dapat menjadi petunjuk.
Jika anda mendapatkan error 401 unauthorized, silahkan konsultasikan dengan tim infrastruktur anda.


### <a name="MenjalankanAplikasi"></a>8. Menjalankan aplikasi

Boilerplate sudah dikonfigurasi menggunakan nilai default.
Jika anda mengikuti semua langkah sebelumnya dengan tepat, maka aplikasi bisa langsung dijalankan.
Caranya:
1. Buka aplikasi Visual Studio
2. Pilih Project `KLG.Backend.Organization.Services`, lalu klik kanan dan pilih _"Set As Startup Project"_
3. Run

**Catatan:** Jika terjadi kesalahan, perhatikan pesan kesalahan yang dapat menjadi petunjuk.
Jika anda menggunakan instalasi yang berbeda (tidak sesuai langkah di atas),
maka variabel konfigurasi kemungkinan akan berbeda,
dan ini anda perlu menyesuaikan file appsettings.json sebelum bisa menjalankan aplikasi.

