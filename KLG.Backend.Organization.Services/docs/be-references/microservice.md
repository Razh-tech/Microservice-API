[Home](../../readme.md) >

## A. Microservice

Bagian ini membahas tentang life-cycle, konfigurasi, dan keamanan microservice.
Life-cycle microservice meliputi hal-hal yang dilakukan package library secara garis besar.
Konfigurasi microservice dapat dilakukan secara lokal maupun dengan menggunakan Vault Service
untuk mengakses sumber daya yang ada di server jarak jauh.
Selain itu, bagian ini juga membahas tentang konsep asimetrik key, access token, dan
refresh token sebagai langkah-langkah keamanan dalam pengembangan microservice.


### <a name="service-lifecycle"></a>1. Service Life-cycle

`KLGMicroserviceBuilder<T>` adalah sebuah class yang digunakan untuk membuat dan menjalankan
sebuah aplikasi microservice. Class ini merupakan generic class dengan parameter T yang merupakan
tipe dari KLG database context `(IKLGDbContext)`. Cara kerja microservice wrapper ini adalah sebagai berikut:

1. **Registrasi Service agar dapat diakses sebagai Dependency Injection**

    `KLGMicroserviceBuilder<T>` akan melanjutkan dengan memanggil method `RegisterServices(config)`
    untuk mendaftarkan service-service yang diperlukan oleh aplikasi microservice. Service ini akan di-injeksi
    sehingga dapat digunakan secara otomatis di class-class yang menurunkan KLGApiController.    

    Service yang didefinisikan antara lain:

    - `ILogger`: service untuk melakukan logging menggunakan Serilog.
    - `IKLGConfiguration`: service yang menyediakan instance dari KLGConfiguration.
    - `IKLGDbProvider<T>`: service yang menyediakan instance dari database context (IKLGDbContext).
    - `IKLGMessagingProvider`: service yang menyediakan instance dari messaging provider.

    Class `KLGConfiguration` akan mendapatkan data konfigurasi dari Vault Service. Jika Vault Service tidak tersedia,
    maka class ini akan menggunakan setting yang terdapat pada file _appsettings.json_.

2. **Build Application**

    Setelah service-service berhasil didaftarkan, instance dari `KLGMicroserviceBuilder<T>` akan membangun
    web application menggunakan WebApplicationBuilder. Pembuatan web application dilakukan dengan memanggil method
    Build() pada instance dari WebApplicationBuilder.

    Setelah web application berhasil dibangun, aplikasi microservice akan melakukan konfigurasi middleware, yaitu:
    - Swagger dan SwaggerUI untuk dokumentasi API,
    - Autentikasi dan autorisasi menggunakan JWT.
 
3. **Menambahkan Transaction Middleware**
    Class ini juga akan membuat middleware untuk menangani DbTransaction secara otomatis.
    Jika endpoint berhasil dipanggil tanpa exception, maka transaksi yang sedang berlangsung
    pada database context otomatis akan di-commit. Sedangkan, jika endpoint menghasilkan exception,
    maka transaksi yang sedang berlangsung pada database context akan otomatis di-rollback.

4. **Melakukan konfigurasi khusus**
    Mekanisme Datetime localization .NET 6 berbeda dengan versi sebelumnya.
    Oleh karena itu kita menambahkan konfigurasi agar mekanisme Datetime legacy masih tetap dapat diterima.


### <a name="konfigurasi"></a>2. Menentukan konfigurasi microservice

Untuk mengkonfigurasi microservice lakukan langkah-langkah berikut:
1. Ubah nama Class dengan nama microservice yang diinginkan. Pada contoh di bawah ini,
ubah nama class Organization menjadi nama microservice yang diinginkan.
Nama microservice akan secara otomatis menentukan nama instance dari platform pendukungnya.
Misalnya, jika nama microservice diset menjadi "Payroll", maka sistem akan membuat database dengan nama "payroll"

```csharp
using KLG.Backend.Organization.Services.Configuration;
using KLG.Backend.Organization.Services.Entities;
using KLG.Library.Microservice;

namespace KLG.Backend.Organization.Services;

public static class Organization
{
    public async static Task Main(string[] args)
    {
        // initialize auto-mapper
        KLGMapper.Initialize();

        // cerate new microservice builder
        var builder = new KLGMicroserviceBuilder<DefaultDbContext>();

        // add your own services
        //builder.Services.AddScoped<...> ();

        // add your own configuration classes
        // you can add more than one custom configurations
        builder.RegisterConfigurationSection<OrganizationConfiguration>();

        // run the microservice
        await builder.Build().RunAsync();

    }

}

```

2. Tambahkan custom service yang di butuhkan di area:

    `builder.Services.AddScoped<...> ();`.

    Ubah atau tambahkan custom configuration yang di butuhkan di bawah baris:

    `builder.RegisterConfigurationSection<OrganizationConfiguration>();`.

    Anda dapat menambahkan lebih dari satu custom service / custom configuration class.
    Detail mengenai hal ini dapat dilihat pada section terkait.



