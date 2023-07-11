[Home](../../readme.md) >

## D. Database

Bagian ini membahas tentang konfigurasi database, pembuatan Entity model dan DbContext, dan
penggunaan API controller class. Bagian ini juga membahas tentang cara mengakses record database dan
menggunakan Transaction. Selain itu, bagian ini juga membahas tentang cara reset table dan database.

### <a name="setting"></a>1. Setting Database, user, dan password secara manual

Konfigurasi basis data (database) akan dilakukan secara otomatis dari Layanan Vault (Vault Service).
Jika Layanan Vault tidak tersedia, sistem akan membaca isi dari file _appsettings.json_.
Properti yang harus diisi adalah:

- Database: jenis basis data yang digunakan. 0 = PostgreSQL, SQL Server = 1, SQL Lite = 2
- Server: alamat server basis data
- Port: nomor port yang digunakan
- User: nama pengguna (username)
- Password: kata sandi (password)
- ResetFileFolder: Lokasi file excel untuk mereset isi database

Contohnya adalah sebagai berikut:

```json
"Database": {
    "Database": 0,
    "ConnectionString": "",
    "Server": "localhost",
    "Port": 5432,
    "User": "postgres",
    "Password": "mysecretpassword",
    "ResetFileFolder": "Models/"
  },
```

Catatan: property ConnectionString digunakan oleh sistem untuk membentuk Connection String
ke Database secara otomatis. Property ini tidak dapat disetting oleh developer.

### <a name="entitymodel"></a>2. Membuat Entity model dan DbContext

Untuk membuat tabel atau database object, lakukan langkah-langkah berikut:

#### 2.A. Membuat Entity Model
1. Buatlah sebuah file Class baru.

2. Turunkan kelas `KLGModelBase`. Nama tabel akan diambil dari nama class.
Beberapa sistem manajemen basis data relasional seperti Postgresql membutuhkan
agar nama tabel terdiri dari huruf kecil semua. Framework akan meng-konversi hal tersebut secara otomatis.
Sebagai contoh:

    ```csharp
    public class Employee : KLGModelBase
    ```    

3. Dengan melakukan penurunan kelas `KLGModelBase`, class `Employee` akan secara otomatis memiliki properti berikut:

    - Id : Identitas unik untuk setiap record
    - CreatedDate : Tanggal record dibuat
    - CreatedBy : Nama user yang membuat record untuk pertama kalinya
    - LastUpdatedDate : Tanggal record terakhir kali diperbaharui
    - LastUpdatedBy: Nama user yang memperbaharui record untuk terakhir kalinya
    - ActiveFlag : Status aktif/non-aktif

    Property CreatedDate, CreatedBy, LastUpdatedDate, dan LastUpdateBy akan ditentukan secara otomatis oleh sistem.

4. Tambahkan properti lain dan tentukan kolom-kolomnya. Nama kolom akan diambil secara otomatis dari nama property.
   Gunakan notasi untuk menentukan nullable atau tidak, panjang maksimal, dan atribut lainnya. Sebagai contoh:

    ```csharp
    [MaxLength(100)]
    public string Name { get; set; }
    ```

    Anotasi [MaxLength(100)] mengindikasikan bahwa panjang karakter maksimal dari Name adalah 100.

5. Jika perlu menambahkan alternate key agar satu atau beberapa kolom selalu unik, menambahkan index, atau menambahkan foreign key, Anda dapat merujuk pada dokumentasi Entity Framework yang tersedia.

    Contoh model entitas dapat ditemukan di file `Entities/Employee.cs.`

    ```csharp
    using System.ComponentModel.DataAnnotations;
    using KLG.Library.Microservice.DataAccess;

    namespace KLG.Backend.Organization.Services.Entities;

    // Create your entity model by inheriting KLGModelBase and using Data Annotation.
    public class Employee : KLGModelBase
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public int Age { get; set; }

        public ICollection<EmployeeCompetency> competency { get; set; }
    }
    ```

#### 2.B. Membuat DbContext

1. Buka file `Entities/DefaultDbContext.cs`
2. Tambahkan entity model yang anda buat ke bawah baris "add your [ENTITY_MODELS] here..":

    ```csharp
    public class DefaultDbContext : KLGDbContext
    {
        public DefaultDbContext() { }

        public DefaultDbContext(DbContextOptions opt) : base(opt) { }
    
        // add your [ENTITY_MODELS] here..
        // public DbSet<[ENTITY_MODELS]> [ENTITY_MODELS] { get; set; }
        ...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ...
        }    
    }
    ```

3. Secara umum, disarankan untuk menggunakan _annotation_ dalam mendefinisikan informasi table name,
column name, dll. Namun, dalam kasus yang lebih kompleks, anda mungkin perlu menggunakan _Fluent API_.
Hal tersebut dapat dilakukan dengan menambahkannya di dalam method `OnModelCreating`. Contoh:

    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // The fluent API is considered a more advanced feature
        // and it's recommended to use Data Annotations unless your requirements
        // require you to use the fluent API.

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employee"); // Set table name
            entity.HasKey(e => e.Id); // Set primary key
            entity.Property(e => e.Id).HasMaxLength(40); // Set id field length
            entity.Property(e => e.Name).HasMaxLength(100); // Set name field length
        });
    }
    ```

### <a name="diservice"></a>3. Mendapatkan DbProvider dan DbContext dari DI Service

Untuk mengakses database, anda bisa menggunakan class `KLGDbProvider` dan class `DefaultDbContext`.
Keduanya dibuat sebagai instance baru untuk setiap request (menggunakn dependency injection sebagai
scoped transaction).
Untuk mendapatkannya, tambahkan parameter IDbProvider pada constuctor di controller class yang anda buat.
Contohnya dapat dilihat pada file `Controllers/EmployeeController.cs` sebagai berikut:

```csharp
private EmployeeManager _employeeManager;

public EmployeeController(
    IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
    IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
    : base(dbProvider, configuration, messageProvider, logger)
{
    _employeeManager = new EmployeeManager(DbProvider, MessageProvider, ConfigurationProvider);
}
```

Dengan menambahkan parameter `IKLGDbProvider<DefaultDbContext> dbProvider` pada constuctor dan
melewatkannya ke base constructor sebagai berikut: `base(dbProvider, ...`, maka anda dapat mengakses
database dengan menggunakan member variable 'DbProvider' yang ada di parent class `KLGApiController<T>`.

Object DI dapat diteruskan ke kode Business Logic (`_employeeManager`) sehingga kode tersebut dapat mengakses database,
messaging, configuration, dan log. Jika anda ingin membuat unit test, maka object DI dapat diganti dengan object Mock.

Contoh membaca record Database adalah sebagai berikut:

File **Controllers/RestApi/EmployeeController.cs**:
```csharp
[HttpGet]
public async Task<IActionResult> Read()
{
    return Ok(await _employeeManager.GetAll());
}
```

File **Business/Employees/EmployeeManager.cs**:
```csharp
public async Task<IEnumerable<Employee>> GetAll()
{
    return await DbProvider.DbContext.Employees.ToListAsync();
}
```




### <a name="crud"></a>4. Mengakses record database dan menggunakan Transaction

Microservice Framework secara otomatis memulai database transaction baru setiap kali anda
memanggil perintah SaveChanges atau SaveChangesAsync di class DbContext. Jika anda mengirim message ke kafka,
maka proses penyimpanan data dan pengiriman pesan akan dilakukan secara bersamaan sebagai satu transaction.
Baca tentang konsep outbox pattern [di sini](). 

Microservice Framework juga akan melakukan commit dan rollback secara otomatis.
Jika method berhasil sampai akhir, maka commit akan otomatis dilakukan.
Sedangkan jika method throw exception, maka transaction akan otomatis di rollback.
Anda juga bisa melakukan commit atau rollback secara manual dengan memanggil perintah:

    await DbProvider.DbContext.Transaction.CommitAsync()

atau

    await DbProvider.DbContext.Transaction.RollbackAsync()

Contoh lengkap dapat dilihat pada file `Controllers/RestApi/EmployeeController.cs`:

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO employee)
{
    try
    {
        return Ok(await _employeeManager.Create(KLGMapper.Map<Employee>(employee)));
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
```

dimana business logicnya dapat dilihat di file `Business/Employees/EmployeeManager.cs`:

```csharp
public async Task<Employee> Create(Employee employee)
{
    ValidateEmployeeEntry(employee);

    // Add entry to the DB context.
    DbProvider.DbContext.Employees.Add(employee);

    // Save all changes. SaveChangesAsync will always trigger a DbTransaction
    // to ensure database and messaging processes are always consistent.
    // See the outbox pattern concept.
    await DbProvider.DbContext.SaveChangesAsync();

    // Send message to the message stream.
    var message = KLGMapper.Map<EmployeeCreated>(employee);
    await MessageProvider.PublishAsync(message);

    // Return the newly created employee.
    // The ID property will be filled with the new employee ID.
    return employee;
}
```

### <a name="concurrency"></a>5. Update concurrency Check

Pada saat melakukan update, ada resiko terjadi perubahan yang dilakukan oleh beberapa pihak sekaligus.
Contoh skenarionya:
- Jam 1:00, user A mengambil data employee dengan Id 01 dengan nama = John
- Jam 1:01, user B mengambil data employee juga dengan Id 01 dengan nama = John
- Jam 1:05, user A mengubah nama employee menjadi Bob dan merekamnya dan berhasil.
- Jam 1:06, user B mengubah data employee menjadi James dan berusaha merekamnya juga.

Perubahan yang dilakukan oleh B harus ditolak, karena pada saat mengubah,
user B mengira ia mengubah John menjadi James. Kenyataannya ia mengubah Bob menjadi James.

Framework dapat mengatasi kasus ini secara otomatis. Untuk itu, setiap kali user ingin melakukan perubahan,
user harus mengirimkan data LastUpdateDate yang sama dengan nilai aslinya. Jika nilai LastUpdatedDate ini
berbeda dengan nilai LastUpdatedDate yang ada di server, maka framework akan mengetahui bahwa data sudah
diubah dan mengembalikan exception dengan tipe  `DbUpdateConcurrencyException`. Contoh:

```csharp
[HttpPut]
public async Task<IActionResult> Update([FromBody] UpdateEmployeeDTO employee)
{
    try
    {
        return Ok(await _employeeManager.Update(KLGMapper.Map<Employee>(employee)));
    }
    catch (DbUpdateConcurrencyException)
    {
        return BadRequest("Database concurrency error. The record already changed since the last time you retrieved it.");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
```

### <a name="reset"></a>6. Reset table dan database

Reset table dapat digunakan untuk menghapus dan mengembalikan seluruh database ke kondisi awal.
Hati-hati menggunakan fitur ini karena seluruh data akan terhapus. Contoh penggunaannya dapat dilihat
pada file `Controllers/RestApi/UtilityController.cs`:

```csharp
[HttpPost]
[Route("resetall")]
public async Task<IActionResult> ResetAll([FromServices] IBootstrapper bootstrapper)
{
    await bootstrapper.DisposeAsync();

    await DbProvider.ResetTablesAsync();

    await bootstrapper.BootstrapAsync();

    return Ok();
}
```

Anda juga dapat menghapus dan memuat ulang data awal salah satu table saja. Berikut contohnya:
```csharp
HttpPost]
[Route("reset")]
public async Task<IActionResult> Reset([FromServices] IBootstrapper bootstrapper)
{
    await bootstrapper.DisposeAsync();

    await DbProvider.ResetTablesAsync(typeof(Employee));

    await bootstrapper.BootstrapAsync();

    return Ok();
}
```