# Rijndael

- Inheritance

  [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object?view=netframework-4.8)[SymmetricAlgorithm](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.symmetricalgorithm?view=netframework-4.8)Rijndael

- Derived

  [System.Security.Cryptography.RijndaelManaged](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged?view=netframework-4.8)

- Attributes

  [ComVisibleAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.comvisibleattribute?view=netframework-4.8)



```c#
[HttpPost]
        [Route("encrypt")]
        public ActionResult<SimpleText> Encrypt([FromBody]SimpleText model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.text))
                {
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = new UTF8Encoding().GetBytes(model.text);

                    Rijndael rijndael = new RijndaelManaged();

                    rijndael.KeySize = 256;

                    MemoryStream mStream = new MemoryStream();
                    CryptoStream encryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateEncryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    encryptor.Write(bText, 0, bText.Length);
                    encryptor.FlushFinalBlock();

                    return Ok(new SimpleText { text = Convert.ToBase64String(mStream.ToArray()) });
                }
                else
                {
                    return BadRequest(new SimpleText { text = "Texto de entrada não pode ser nulo" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new SimpleText { text = "Erro ao criptografar" + ex.Message });
            }
        }


        [HttpPost]
        [Route("decrypt")]
        public ActionResult<SimpleText> Decrypt([FromBody]SimpleText model)
        {
            try
            {

                if (!string.IsNullOrEmpty(model.text))
                {
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = Convert.FromBase64String(model.text);

                    Rijndael rijndael = new RijndaelManaged();

                    rijndael.KeySize = 256;

                    MemoryStream mStream = new MemoryStream();

                    CryptoStream decryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateDecryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    decryptor.Write(bText, 0, bText.Length);
                    decryptor.FlushFinalBlock();
                    UTF8Encoding utf8 = new UTF8Encoding();


                    return Ok(new SimpleText { text = utf8.GetString(mStream.ToArray()) });
                }
                else
                {
                    return BadRequest(new SimpleText { text = "Texto de entrada não pode ser nulo" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new SimpleText { text = "Erro ao descriptografar" + ex.Message });
            }
        }

    }
    public class SimpleText
    {
        public string text { get; set; }
    }
```

