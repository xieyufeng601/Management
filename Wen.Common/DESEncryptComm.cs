using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wen.Common
{
    public class DESEncryptComm
    {
        public DESEncryptComm()
        {
        }

        #region ========加密========  

        /// <summary>  
        /// 加密  
        /// </summary>  
        /// <param name="Text"></param>  
        /// <returns></returns>  
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "shenjiany");
        }
        /// <summary>   
        /// 加密数据   
        /// </summary>   
        /// <param name="Text"></param>   
        /// <param name="sKey"></param>   
        /// <returns></returns>   
        public static string Encrypt(string Text, string sKey)
        {
            using (DES des = DES.Create()) // Fix for SYSLIB0021 and IDE0090  
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(Text);
                des.Key = Encoding.ASCII.GetBytes(Md5Hash(sKey));
                des.IV = Encoding.ASCII.GetBytes(Md5Hash(sKey));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }
                    StringBuilder ret = new StringBuilder();
                    foreach (byte b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }
                    return ret.ToString();
                }
            }
        }

        #endregion

        #region ========解密========  

        /// <summary>  
        /// 解密  
        /// </summary>  
        /// <param name="Text"></param>  
        /// <returns></returns>  
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "shenjiany");
        }
        /// <summary>   
        /// 解密数据   
        /// </summary>   
        /// <param name="Text"></param>   
        /// <param name="sKey"></param>   
        /// <returns></returns>   
        public static string Decrypt(string Text, string sKey)
        {
            try
            {
                using (DES des = DES.Create()) // Fix for SYSLIB0021 and IDE0090  
                {
                    int len = Text.Length / 2;
                    byte[] inputByteArray = new byte[len];
                    for (int x = 0; x < len; x++)
                    {
                        inputByteArray[x] = (byte)Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    }
                    des.Key = Encoding.ASCII.GetBytes(Md5Hash(sKey));
                    des.IV = Encoding.ASCII.GetBytes(Md5Hash(sKey));
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                        }
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                return Text;
            }
        }

        private static string Md5Hash(string input)
        {
            using (MD5 md5Hasher = MD5.Create()) // Fix for IDE0090  
            {
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        #endregion
    }
}
