[Home](../../readme.md) >

## E. Message Streaming

Bagian ini membahas tentang konfigurasi message stream, outbox pattern, serta cara mengirim dan
menerima message. Bagian ini juga membahas tentang cara melakukan idempotency check dan
compensating transaction.

### <a name="setting"></a>1. Setting message broker secara manual

Konfigurasi message stream akan dilakukan secara otomatis dari Layanan Vault (Vault Service).
Jika Layanan Vault tidak tersedia, sistem akan membaca isi dari file _appsettings.json_.
Properti yang harus diisi adalah:

- BootstrapServers: alamat server 
- EnableConsumerPrefetch: true, jika ingin consume message secara paralel. false, jika ingin consume message secara sequential.
- ConsumerThreadCount: jumlah pemrosesan paralel
- GroupId: selalu isi dengan empty string

Contohnya adalah sebagai berikut:

```json
"Messaging": {
    "BootstrapServers": "localhost:9092",
    "GroupId": "",
    "EnableConsumerPrefetch": true,
    "ConsumerThreadCount": 5
  },
```

### <a name="outbox-pattern"></a>2. Konsep outbox pattern

Outbox pattern adalah sebuah konsep arsitektur untuk mengatasi masalah konsistensi penyimpanan data dalam aplikasi yang menggunakan message streaming atau message queuing system. Konsep ini bertujuan untuk memastikan bahwa pesan yang dikirim melalui message streaming system dapat dijamin konsistensinya dengan data yang disimpan di dalam database.

Pada dasarnya, ketika aplikasi mengirimkan pesan ke message streaming system, tidak ada jaminan bahwa pesan tersebut akan sampai ke tujuan dengan cepat atau bahkan berhasil. Hal ini dapat disebabkan oleh berbagai faktor seperti kesalahan jaringan, kegagalan server, dan sebagainya. Jika pesan yang dikirim gagal terkirim, maka data yang disimpan di dalam database juga tidak akan terkonsistensi.

Untuk mengatasi masalah ini, outbox pattern menyarankan penggunaan outbox table yang bertindak sebagai buffer antara aplikasi dan message streaming system. Setiap kali aplikasi melakukan perubahan pada data di dalam database, ia juga akan menambahkan sebuah pesan ke dalam outbox table. Pesan ini akan berisi informasi tentang perubahan data yang dilakukan.

Setelah pesan berhasil ditambahkan ke dalam outbox table, message streaming system akan memproses pesan tersebut dan mengirimkannya ke tujuan yang ditentukan. Jika pesan berhasil terkirim, maka sistem akan menghapus pesan tersebut dari outbox table. Namun, jika terjadi kesalahan dalam pengiriman pesan, pesan tersebut akan tetap tersimpan di dalam outbox table hingga berhasil terkirim.

Dengan menggunakan outbox pattern, aplikasi dapat memastikan bahwa pesan yang dikirim melalui message streaming system selalu konsisten dengan data yang disimpan di dalam database. Jika terjadi kegagalan dalam pengiriman pesan, pesan tersebut tidak akan hilang dan masih dapat dikirimkan pada saat yang tepat. Dengan demikian, aplikasi dapat memastikan bahwa data yang disimpan di dalam database selalu terkonsistensi dan akurat.

### <a name="publish"></a>3. Mengirim (publish) message ke message streaming

Untuk mengirim message ke kafka, lakukan langkah-langkah berikut:
1. Tambahkan IKLGMessagingProvider messageProvider ke constructor di controller yang ingin digunakan, contoh:

    ```csharp
    public EmployeeController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
        : base(dbProvider, configuration, messageProvider, logger)
    ```

2. Disarankan untuk membuat class khusus untuk messaging DTO, sehingga message yang dikirim dapat
diproses secara _strong-typed_ baik oleh publisher maupun oleh subscriber. Dalam contoh di bawah,
DTO class yang dimaksud adalah EmployeeCreated yang terdapat pada project **KLG.Backend.Organization.Models**
di file **Message/EmployeeCreated.cs**. Messaging DTO dapat dibuat sebagai package agar
memudahkan subscriber service untuk mendapatkannya.

3. Panggil perintah `MessageProvider.PublishAsync` untuk mengirimkan message.
Message akan dikirim ke topic dengan nama class DTO yang anda gunakan.
Pada contoh di bawah ini, message akan dikirim ke topic 'EmployeeCreated'.

4. Jika sebelum mengirim message anda menyimmpan data ke database, maka Framework secara otomatis
akan melakukan _transaction commit_ untuk data dan pesan secara bersamaan.
Dengan demikian, data dan pesan selalu konsisten. Jika terjadi kegagalan pengiriman message,
maka Framework akan melakukan resend secara otomatis.

Contoh penggunaannya dapat dilihat pada file: Business/Employee/EmployeeManager.cs berikut:

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
### <a name="consume-serial"></a>4. Menerima (consume) message secara serial

Untuk menerima pesan secara terurut, pastikan agar settings message stream sebagai berikut:

```json
"Messaging": {
    "BootstrapServers": "localhost:9092",
    "GroupId": "",
    "EnableConsumerPrefetch": false,
    "ConsumerThreadCount": 1
  },
```

Untuk mendapatkan message, cukup buat method dengan annotasi `[NonAction]` dan annotasi
`[KLGMessageSubscribe(nameof(<NamaClassDTO>)]`.
Message bisa didapat dengan melakukan helper method `GetPayloadAsync`.

Contoh:

```csharp
[NonAction]
[KLGMessageSubscribe(nameof(EmployeeCreated))]
public async Task EmployeeCreatedSubscriber(KLGMessage p)
{
    var emp = MessageProvider.GetPayloadAsync<EmployeeCreated>(p)
        ?? throw new InvalidOperationException("Payload is null");

    await new HeadcountManager(DbProvider).RecruitNewEmployee();
}
```

### <a name="consume-paralel"></a>5. Menerima (consume) message secara paralel
Untuk menerima pesan secara paralel, pastikan agar settings message stream sebagai berikut:

```json
"Messaging": {
    "BootstrapServers": "localhost:9092",
    "GroupId": "",
    "EnableConsumerPrefetch": true,
    "ConsumerThreadCount": 5     // ganti dengan jumlah paralel proses yang diinginkan
  },
```

Cara membuat method untuk mendapatkan message sama dengan cara consume message secara serial.
Yang harus diperhatikan adalah logika pemrosesan, karena message akan di-consume secara acak.

### <a name="idempotency"></a>6. Idempotency check

Idempotensi adalah konsep penting dalam pengiriman dan pengolahan pesan dalam konteks streaming. Dalam konteks message streaming, idempotency check mengacu pada proses verifikasi apakah suatu pesan telah diterima sebelumnya dan diproses dengan sukses. Jika pesan telah diproses dengan sukses sebelumnya, maka pesan akan dianggap sudah diterima dan diproses, sehingga tidak akan diproses lagi.

Idempotency check penting untuk memastikan bahwa data yang diproses dalam lingkungan streaming tetap konsisten dan akurat. Jika suatu pesan diproses lebih dari satu kali, maka dapat menyebabkan duplikasi data dan dapat merusak integritas data. Oleh karena itu, idempotency check digunakan untuk memastikan bahwa setiap pesan yang dikirim hanya diproses satu kali.

Ada beberapa teknik yang dapat digunakan untuk melakukan idempotency check dalam message streaming. Salah satu teknik yang umum digunakan adalah dengan memberikan identifikasi unik untuk setiap pesan yang dikirim. Identifikasi unik ini kemudian digunakan untuk memverifikasi apakah suatu pesan telah diterima sebelumnya. Jika identifikasi unik tersebut telah ada sebelumnya, maka pesan dianggap sudah diterima dan diproses.

Teknik lain yang dapat digunakan adalah dengan menyimpan status pengolahan pesan dalam sebuah database. Setiap kali pesan diterima, statusnya akan diperiksa dalam database untuk memastikan bahwa pesan tersebut belum diproses sebelumnya. Jika pesan telah diproses sebelumnya, maka pesan tidak akan diproses lagi.

Dalam semua teknik idempotency check, penting untuk memastikan bahwa identifikasi unik atau status pengolahan pesan disimpan dalam database atau sistem yang dapat diakses dengan cepat dan efisien untuk meminimalkan keterlambatan dalam pengolahan pesan.

#### Implementasi dalam Framework
Framework secara otomatis melakukan idempotency check. Identifikasi dilakukan berdasarkan `OperationId` yang terdapat pada property `KLGMessage`. Jika aplikasi menerima lebih dari satu message dengan `OperationId` yang sama, maka framework akan secara otomatis menolaknya.

### <a name="compensating"></a>7. Compensating transaction

Jika anda membutuhkan pemrosesan lanjutan di service berbeda dan juga membutuhkan hasil eksekusinya,
maka fitur compensating transaction dapat memudahkan anda melakukannya. Pada contoh berikut kita akan
menghapus data dengan terlebih dahulu melakukan proses approval di subscriber method.

1. Tambahkan nama topic balasan (contoh: `nameof(EmployeeDeleteApproval)`) pada saat mengirimkan message,
pada saat memanggil metod PublishAsync.

    Contohnya dapat dilihat pada file **Controllers/MessageHandler/EmployeeMessageController.cs**:

    ```csharp
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _employeeManager.RequestToDelete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    ```

    dan file **Business/Employees/EmployeeManager.cs**:

    ```csharp
    public async Task RequestToDelete(string id)
    {
        var employee = await GetByIdAsync(id);

        if (employee == null)
            throw new InvalidOperationException($"Employee id:{id} is not found");

        // Send message to the message stream.
        var message = new EmployeeDeleteRequest() { Id = id, Name = employee.Name };
        await MessageProvider.PublishAsync(message, nameof(EmployeeDeleteApproval));
    }
    ```

2. Pada method subscriber (`EmployeeDeleteRequestSubscriber`), return hasilnya, contoh:

    Contohnya dapat dilihat pada file **Controllers/MessageHandler/EmployeeMessageController.cs**:

    ```csharp
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleteRequest))]
    public EmployeeDeleteApproval EmployeeDeleteRequestSubscriber(KLGMessage p)
    {
        var message = MessageProvider.GetPayloadAsync<EmployeeDeleteRequest>(p)
            ?? throw new InvalidOperationException("Invalid message");

        return new EmployeeDeleteApproval()
        {
            Id = message.Id,
            Approved = _employeeManager.DeleteApproval(message.Name)
        };
    }
    ```

    dan file **Business/Employees/EmployeeManager.cs**:

    ```csharp
    public bool DeleteApproval(string name)
    {
        return (!name.StartsWith("~"));
    }
    ```

3. Consume hasilnya di method `EmployeeDeleteApprovalSubscriber`.

    Contohnya dapat dilihat pada file **Controllers/MessageHandler/EmployeeMessageController.cs**:

    ```csharp
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleteApproval))]
    public async Task EmployeeDeleteApprovalSubscriber(KLGMessage p)
    {
        var message = MessageProvider.GetPayloadAsync<EmployeeDeleteApproval>(p)
            ?? throw new InvalidOperationException("Invalid message");

        if (message.Approved)
        {
            await _employeeManager.Delete(message.Id);
        }
        else
        {
            Logger.Information($"Deletion of employee {message.Id} was rejected");
        }
    }

    ```