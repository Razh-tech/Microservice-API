[Home](../../readme.md) >

## B. Configuraition

Bagian ini membahas tentang konfigurasi pada aplikasi, baik pada level local development maupun remote resources.
Konfigurasi dapat dilakukan dengan menggunakan appsettings pada lingkungan lokal maupun Vault Service untuk
mengakses sumber daya yang ada di remote server. Selain itu, bagian ini juga membahas tentang pembuatan
custom configuration.

### <a name="local-config"></a>1. Menggunakan appsettings untuk konfigurasi local development

Konfigurasi appsettings untuk local development akan membaca isi dari file _appsettings.json_.
Konfigurasi akan memiliki struktur seperti berikut:

```json
"<Path>":{
    // properti yang harus diisi
}
```

Untuk nilai `Path` dan properti yang harus diisi adalah sebagai berikut:

- [Database](database.md#setting)
- [Messaging](messaging.md#setting)
- [Security](security.md#setting)
- [Logging](logging-and-telemetry.md#setting)
- [Telemetry](logging-and-telemetry.md#setting)

### <a name="vault-service"></a>2. Menggunakan Vault Service untuk mengakses remote resources

Untuk mengambil properti konfigurasi dari vault service, properti yang harus ditambahkan 
adalah sebagai berikut:

- VaultToken: merupakan token yang digunakan untuk otorisasi ke vault service
- VaultEndpoint: merupakan endpoint dari vault service
- VaultMountpoint: merupakan mount point dari secrets engine yang akan dipakai

Contohnya adalah sebagai berikut:

```json
"VaultConfiguration": {
  "VaultToken": "token",
  "VaultEndpoint": "https://vaulturl",
  "VaultMountPoint": "klg-kv/"
},
```

### <a name="custom-configuration"></a>3. Membuat custom configuration

Untuk membuat custom configuration, anda dapat menggunakan interface `IKLGConfigurationSection` ke dalam
custom class anda. Berikut adalah contoh implementasinya:

```csharp
public class MyOwnConfiguration : IKLGConfigurationSection
{
    public string Path => "MyOwn";
    public string MyProperty { get; private set; } = default!;
    public string MySecondProperty { get; private set; } = default!;
}
```

Lalu tambahkan properti berikut ke dalam _appsettings.json_ atau vault service:

```json
"MyOwn": {
    "MyProperty": "MyValue",
    "MySecondProperty": "MySecondValue"
}
```

Lihat bahwa `Path` akan mengambil property dari `"MyOwn"` untuk kemudian dipetakan ke dalam `MyProperty`
dan `MySecondProperty`.

Untuk dapat mengakses value tersebut, anda dapat melakukan registrasi custom class tersebut 
ke dalam builder seperti berikut:

```csharp
builder.RegisterConfigurationSection<MyOwnConfiguration>();
```

Lalu panggil class tersebut seperti berikut:

```csharp
[HttpGet]
[Route("MyOwn")]
public IActionResult GetMyOwn()
{
    var data = ConfigurationProvider.GetConfig<MyOwnConfiguration>();
    return new OkObjectResult(data);
}
```

Contoh di atas akan mengembalikan nilai dari property `"MyOwn"` ketika dilakukan HTTP GET request.