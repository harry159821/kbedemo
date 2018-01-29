using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

    public class Util
    {

        public static int Int(object o)
        {

            return Convert.ToInt32(o);

        }



        public static float Float(object o)
        {

            return (float)Math.Round(Convert.ToSingle(o), 2);

        }



        public static long Long(object o)
        {

            return Convert.ToInt64(o);

        }



        public static int Random(int min, int max)
        {

            return UnityEngine.Random.Range(min, max);

        }



        public static float Random(float min, float max)
        {

            return UnityEngine.Random.Range(min, max);

        }

        /// <summary>

        /// 计算字符串的MD5值

        /// </summary>

        public static string md5(string source)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);

            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);

            md5.Clear();



            string destString = "";

            for (int i = 0; i < md5Data.Length; i++)
            {

                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');

            }

            destString = destString.PadLeft(32, '0');

            return destString;

        }



        /// <summary>

        /// 计算文件的MD5值

        /// </summary>

        public static string md5file(string file)
        {

            try
            {

                FileStream fs = new FileStream(file, FileMode.Open);

                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

                byte[] retVal = md5.ComputeHash(fs);

                fs.Close();



                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < retVal.Length; i++)
                {

                    sb.Append(retVal[i].ToString("x2"));

                }

                return sb.ToString();

            }
            catch (Exception ex)
            {

                throw new Exception("md5file() fail, error:" + ex.Message);

            }

        }










   










    }

