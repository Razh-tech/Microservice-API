[Home](../../readme.md) >

## Menggunakan Swagger UI untuk menguji API
Swagger UI adalah sebuah alat yang digunakan untuk menguji dan memvisualisasikan API.
Dengan Swagger UI, pengguna dapat dengan mudah menguji endpoint-endpoint API dan
melihat respons yang diberikan oleh server.
Swagger UI juga menyediakan dokumentasi API yang lengkap dan mudah dipahami.

###  <a name="accesstoken"></a>1. Mendapatkan access token
JWT access token adalah sebuah token yang digunakan untuk mengakses API yang memerlukan autentikasi.
JWT access token menggunakan algoritma RSA untuk mengenkripsi data yang dikirimkan.
RSA adalah sebuah algoritma kriptografi yang digunakan untuk mengenkripsi dan mendekripsi data.
Dalam konteks ini, RSA digunakan untuk mengenkripsi data yang dikirimkan dalam JWT access token
agar tidak dapat dibaca oleh pihak yang tidak berwenang.

Untuk mendapatkan access token:
- Pilih tab _Key_
- Pilih API `[POST] /Key/token`.
- Isi _userId_, _userName_, dan _roles_ sesuai dengan yang dibutuhkan.
- Eksekusi API

<img src="create-token.png" alt="Deskripsi Gambar" height="300">

Contoh access token yang dihasilkan adalah sebagai berikut:
```
eyJhbGciOiJQUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IlVTRVItMDAxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkpvaG4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiTWFuYWdlciIsIkZpbmFuY2UiXSwibmJmIjoxNjgyODc3NTAyLCJleHAiOjE2ODM0ODIzMDIsImlzcyI6Imlzc3VlciIsImF1ZCI6ImF1ZGllbmNlIn0.qY1oD5vHPW0YiRd2MGLQZa26Hn8V2KXUSUFS1W35gFFB2NC4ZESsT7j1P96n10GfWop15ObVG2mLOT7WL2KUUvTyawSgISxeI6ju7K5Sky47mISsgEGMlEl3PUevfX0eiO4YF1WSdizN7Ru1mGRVo7snEHm-8llJuL5_YlQVQmXYrZ-r6Pzxswz_r-umyovDIneJ0-VU7kHg2fqm1NZnaWJe6zNrTw8Tddf2N1jqcplzZtAxWTEycK0A8Whi1yHAtCEhohUiohrP9uke6mja1iFnkRxR_-X5WHygXZJWKdf_8188ToFgvR78CuqUrP3N7gnYKNaV4072nZ4jKJ7Fcw
```

###  <a name="resetdb"></a>2. Reset databasse
Reset database adalah sebuah fitur yang digunakan untuk menghapus semua data yang tersimpan pada database
dan mengembalikan database ke kondisi awal. Fitur ini berguna untuk menguji API pada kondisi awal dan
memastikan bahwa API berfungsi dengan baik setelah direset.

_Catatan: Reset database harus dipanggil sebelum memanggil API lain (kecuali Key),
karena API lain membutuhkan database yang telah terisi._

Untuk melakukan reset database, lakukan langkah-langkah berikut:
- Pilih menu _Authorize_ dan masukkan access token
- Pilih tab _Utility_
- Pilih API `[POST] /Utility/resetall`.
- Eksekusi API

###  <a name="crud"></a>3. Melihat data dan memasukkan data baru
Pada bagian ini, pengguna dapat melihat data dengan memanggil API yang tersedia.
Pengguna juga dapat memasukkan data baru ke dalam API.
Hal ini dapat dilakukan dengan menggunakan Swagger UI yang menyediakan fitur untuk menguji endpoint-endpoint API.

Untuk melihat data yang sudah terekam:
- Pilih menu _Authorize_ dan masukkan access token
- Pilih tab _Employee_
- Pilih API `[GET] Employee`.
- Eksekusi API

Untuk merekam data baru:
- Pilih menu _Authorize_ dan masukkan access token
- Pilih tab _Employee_
- Pilih API `[POST] Employee`.
- Isi property _name_, dan _age_.
- Eksekusi API


