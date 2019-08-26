using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 加密解密操作类
    /// </summary>
    public class CryptionHelper
    {
        #region "静态常量"
        /// <summary>
        /// 授权固定Key
        /// </summary>
        private const String AuthorizationKey = "$ccvza$fsr2#@";
        //密钥 
        private const string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM3";
        //矢量，矢量可以为空 
        private const string sIV = "qcDY6X+aPLw=";
        //构造一个对称算法 
        private static SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
        #endregion

        #region public string EncryptString(string Value)


        /// <summary> 
        /// 加密字符串 
        /// </summary> 
        /// <param name="Value">输入的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncryptString(string Value)
        {
           return EncryptString(Value, sKey);
        }


        /// <summary> 
        /// 加密字符串 
        /// </summary> 
        /// <param name="Value">输入的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncryptString(String Value, String Key)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(splitStringLen(Key, 32, '0'));
            mCSP.IV = Convert.FromBase64String(sIV);
            //指定加密的运算模式 
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            //获取或设置加密算法的填充模式 
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region public string DecryptString(string Value)

          /// <summary> 
        /// 解密字符串 
        /// </summary> 
        /// <param name="Value">加过密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecryptString(String Value)
        {
            return DecryptString(Value, sKey);
        }


        /// <summary> 
        /// 解密字符串 
        /// </summary> 
        /// <param name="Value">加过密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecryptString(String Value, String Key)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(splitStringLen(Key, 32, '0'));
            mCSP.IV = Convert.FromBase64String(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion

        #region "3des加密字符串"

        /// <summary>
        /// 3des加密函数(ECB加密模式,PaddingMode.PKCS7,无IV)
        /// </summary>
        /// <param name="encryptValue">加密字符</param>
        /// <returns>加密后Base64字符</returns>
        public static string EncryptString1(string encryptValue)
        {
            return EncryptString1(encryptValue, AuthorizationKey);
        }

        /// <summary>
        /// 3des加密函数(ECB加密模式,PaddingMode.PKCS7,无IV)
        /// </summary>
        /// <param name="encryptValue">加密字符</param>
        /// <param name="key">加密key(24字符)</param>
        /// <returns>加密后Base64字符</returns>
        public static string EncryptString1(string encryptValue, string key)
        {
            string enstring = "加密出错!";
            ICryptoTransform ct; //需要此接口才能在任何服务提供程序上调用 CreateEncryptor 方法，服务提供程序将返回定义该接口的实际 encryptor 对象。
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            SymmetricAlgorithm des3 = SymmetricAlgorithm.Create("TripleDES");
            des3.Mode = CipherMode.ECB;
            des3.Key = Encoding.UTF8.GetBytes(splitStringLen(key, 24, '0'));
            //des3.KeySize = 192;
            des3.Padding = PaddingMode.PKCS7;

            ct = des3.CreateEncryptor();

            byt = Encoding.UTF8.GetBytes(encryptValue);//将原始字符串转换成字节数组。大多数 .NET 加密算法处理的是字节数组而不是字符串。

            //创建 CryptoStream 对象 cs 后，现在使用 CryptoStream 对象的 Write 方法将数据写入到内存数据流。这就是进行实际加密的方法，加密每个数据块时，数据将被写入 MemoryStream 对象。

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            try
            {
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                enstring = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                enstring = ex.ToString();
            }
            finally
            {
                cs.Close();
                cs.Dispose();
                ms.Close();
                ms.Dispose();
                des3.Clear();
                ct.Dispose();
            }
            enstring = Convert.ToBase64String(ms.ToArray());
            return enstring;
        }
        #endregion

        #region "3des解密字符串"

        /// <summary>
        /// 3des解密函数(ECB加密模式,PaddingMode.PKCS7,无IV)
        /// </summary>
        /// <param name="decryptString">解密字符</param>
        /// <returns></returns>
        public static string DecryptString1(string decryptString)
        {
            return DecryptString1(decryptString, AuthorizationKey);
        }


        /// <summary>
        /// 3des解密函数(ECB加密模式,PaddingMode.PKCS7,无IV)
        /// </summary>
        /// <param name="decryptString">解密字符</param>
        /// <param name="key">解密key(24字符)</param>
        /// <returns>解密后字符</returns>
        public static string DecryptString1(string decryptString, string key)
        {
            string destring = "解密字符失败!";
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            SymmetricAlgorithm des3 = SymmetricAlgorithm.Create("TripleDES");
            des3.Mode = CipherMode.ECB;
            des3.Key = Encoding.UTF8.GetBytes(splitStringLen(key, 24, '0'));
            //des3.KeySize = 192;
            des3.Padding = PaddingMode.PKCS7;

            ct = des3.CreateDecryptor();

            byt = Convert.FromBase64String(decryptString);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            try
            {
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                destring = Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                destring = ex.ToString();
            }
            finally
            {
                ms.Close();
                cs.Close();
                ms.Dispose();
                cs.Dispose();
                ct.Dispose();
                des3.Clear();
            }
            return destring;
        }
        #endregion

        #region "MD5加密"
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }

            return strEncrypt;
        }
        #endregion

        #region 字符串截取补字符函数
        /// <summary>
        /// 字符串截取补字符函数
        /// </summary>
        /// <param name="s">要处理的字符串</param>
        /// <param name="len">长度</param>
        /// <param name="b">补充的字符</param>
        /// <returns>处理后字符</returns>
        public static string splitStringLen(string s, int len, char b)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            if (s.Length >= len)
                return s.Substring(0, len);
            return s.PadRight(len, b);
        }
        #endregion

        #region "动态密钥操作"
    
        


        #endregion

        #region "获取随机数"
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string GetRandomPassword(int length)
        {
            byte[] random = new Byte[length / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);

            StringBuilder sb = new StringBuilder(length);
            int i;
            for (i = 0; i < random.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", random[i]));
            }
            return sb.ToString();
        }
        #endregion

        #region "按当前日期和时间生成随机数"
        /// <summary>
        /// 按当前日期和时间生成随机数
        /// </summary>
        /// <param name="Num">附加随机数长度</param>
        /// <returns></returns>
        public static string sRndNum(int Num)
        {
            string sTmp_Str = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00") + System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00");
            return sTmp_Str + RndNum(Num);
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="VcodeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum)
        {
            StringBuilder sb = new StringBuilder(VcodeNum);
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        #region "通过RNGCryptoServiceProvider 生成随机数 0-9"
        /// <summary>
        /// 通过RNGCryptoServiceProvider 生成随机数 0-9 
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string RndNumRNG(int length)
        {
            byte[] bytes = new byte[16];
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                r.GetBytes(bytes);
                sb.AppendFormat("{0}", (int)((decimal)bytes[0] / 256 * 10));
            }
            return sb.ToString();

        }
        #endregion



        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

    }
}
