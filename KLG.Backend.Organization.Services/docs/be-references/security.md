[Home](../../readme.md) >

## C. Security

Bagian ini membahas tentang konsep keamanan pada aplikasi, seperti penggunaan public key,
access token, dan refresh token. Bagian ini juga membahas tentang bagaimana cara generate access token
serta cara mengakses public key dari identity service. Selain itu, bagian ini juga membahas tentang
cara mengelola user permission dan mendapatkan daftar aplikasi serta menu yang ada pada aplikasi.

Contohnya implementasi security dapat dilihat pada file: **Controllers/RestApi/KeyControllers**.

### <a name="asymetrickey"></a>1. Konsep asymetric key dan generate private/public key

Asymmetric key adalah sebuah metode kriptografi yang menggunakan sepasang kunci (key) untuk melakukan enkripsi dan dekripsi data. Dalam sistem keamanan aplikasi, kita menggunakan public key untuk enkripsi dan private key untuk dekripsi. Dengan menggunakan metode ini, pengguna dapat mengirimkan data yang telah dienkripsi dengan public key ke penerima yang hanya memiliki private key untuk dekripsi data tersebut.

Generate private/public key adalah proses untuk membuat sepasang kunci (key) yang terdiri dari private key dan public key. Private key merupakan kunci yang hanya dimiliki oleh pemiliknya dan digunakan untuk dekripsi data yang telah dienkripsi menggunakan public key. Sedangkan public key adalah kunci yang bisa dibagikan ke orang lain dan digunakan untuk mengenkripsi data yang akan dikirimkan ke pemilik private key.

Public key akan diinformasikan oleh admin sistem yang mengelola identity service (Cerberus).
Contoh untuk membuat simulasi private/public key sendiri adalah sebagai berikut:

```csharp
[HttpGet]
[Route("key")]
public IActionResult GenerateKeyPair()
{
    var jwtManager = new KLGTokenManager();

    var (privateKey, publicKey) = jwtManager.GetKey();
    return new OkObjectResult(new { PrivateKey = privateKey, PublicKey = publicKey });
}
```

### <a name="token"></a>2. Konsep access token dan refresh token

Access token adalah sebuah token yang digunakan untuk mengakses sumber daya (resource) pada suatu aplikasi yang memerlukan otorisasi. Access token ini berisi informasi mengenai izin akses yang diberikan oleh pengguna, seperti role atau permission. Dalam sistem keamanan aplikasi, access token digunakan sebagai tanda bukti bahwa pengguna telah melewati proses autentikasi dan otorisasi.

Refresh token adalah sebuah token yang digunakan untuk memperbaharui access token. Refresh token ini berisi informasi mengenai izin akses yang sama dengan access token, namun memiliki masa aktif yang lebih lama. Jadi, ketika masa aktif access token telah habis, pengguna dapat menggunakan refresh token untuk memperbaharui access token tanpa perlu melakukan proses autentikasi dan otorisasi ulang.

Dalam penggunaannya, access token dan refresh token dapat diberikan kepada pengguna setelah proses autentikasi dan otorisasi berhasil dilakukan. Access token dapat digunakan oleh pengguna untuk mengakses sumber daya yang memerlukan otorisasi, sedangkan refresh token dapat digunakan oleh pengguna untuk memperbaharui access token yang telah habis masa aktifnya. Namun, penting untuk memperhatikan keamanan dalam penggunaan access token dan refresh token, seperti melakukan pengamanan terhadap token tersebut dan memperbaharui masa aktifnya secara teratur.


### <a name="setting"></a>3. Setting public key dari identity service

Untuk keperluan development, public key perlu diletakkan di file appsettings.json sebagai berikut:

```json
"Security": {
    "Audience": "audience",
    "Issuer": "issuer",

    // private key is required for identity service, but not for the others
    "PrivateKey": "eyJEIjoiSTNLbUJQM.....",

    // public key is required for services other than identity service
    "PublicKey": "eyJEIjpudWxsLCJEUC....."
  },
```

_Private key_ perlu dimasukkan jika anda ingin menggenerate token sendiri. Namun jika anda menggunakan token dari identity service yang sudah ada, maka setting ini tidak perlu diisi.

_Public key_ perlu dimasukkan sesuai dengan kebutuhan. Jika anda ingin menggunakan token sendiri, isi dengan public key yang digenerate pada langkah 1. Jika anda ingin menggunakan token dari identity service yang tersedia, maka isi dengan public key yang diberikan oleh admin sistem.

**Catatan:** Settings ini tidak akan berpengaruh pada saat aplikasi berjalan di server, karena konfigurasi server akan mengacu pada Vault yang telah disetting oleh admin sistem.

### <a name="generate"></a>4. Generate access token

Contoh untuk membuat access token, adalah sebagai berikut:
```csharp
[HttpPost]
[Route("token")]
public IActionResult CreateToken([FromBody] CreateTokenDTO tokenInfo)
{
    var jwtManager = new KLGTokenManager();

    var claims = new List<Claim>() {
        new Claim(ClaimTypes.NameIdentifier, tokenInfo.UserId),
        new Claim(ClaimTypes.Name, tokenInfo.UserName),
    };

    foreach (var role in tokenInfo.Roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var token = jwtManager.GenerateToken(
        ConfigurationProvider.Security.PrivateKey,
        ConfigurationProvider.Security.Audience,
        ConfigurationProvider.Security.Issuer,
        claims);

    return new OkObjectResult(token);
}

public class CreateTokenDTO
{
    [JsonProperty("userId")]
    public string UserId { get; set; }

    [JsonProperty("userName")]
    public string UserName { get; set; }

    [JsonProperty("roles")]
    public IEnumerable<string> Roles { get; set; }
}
```

### <a name="userinfo"></a>5. Mendapatkan user info dan claims dari access token

Access token mengandung informasi user id, user name, dan claim. Untuk mendapatkannya anda dapat
menggunakan method helper (`ParseClaims`) yang ada di class `KLGTokenManager`. Contohnya adalah sebagai berikut:

```csharp
[Authorize]
[HttpGet]
[Route("info")]
public IActionResult GetTokenInfo()
{
    return Ok(KLGTokenManager.ParseClaims(User.Claims));
}
```

Method `ParseClaims` akan mengembalikan hasil dengan tipe UserInfo, contohnya:

```json
{
  "id": "EMP-001",
  "name": "John Doe",
  "roles": [
    "Manager",
    "Supervisor"
  ]
}
```

### <a name="managepermission"></a>6. Mengelola user permission
...

### <a name="getpermission"></a>7. Mendapatkan permission (daftar aplikasi dan menu)
...
