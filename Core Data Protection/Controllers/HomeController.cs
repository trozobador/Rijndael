using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core_Data_Protection.Controllers
{
    [ApiController]
    [Route("cipher")]
    public class HomeController : ControllerBase
    {
        private static byte[] bIV =
    { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18,
        0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };


        //Change this key to use in your project
        private const string cryptoKey =
           "FCvGJyHpG7WujJZMNbC2eRqFsp6j4yeCuy7YBurfQye=";

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


}