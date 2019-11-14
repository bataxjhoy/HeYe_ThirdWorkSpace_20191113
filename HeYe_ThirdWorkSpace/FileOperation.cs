using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace HeYe_ThirdWorkSpace
{
    class  FileOperation
        {

        /// <summary>
        /// 按行读取文本文件:
        /// </summary>
        /// <param name="txtFileName"></param>
        /// <returns></returns>
        public string[] ReadTxtFile(string txtFileName)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。 
            string[] text = new string[] { };
            if (System.IO.File.Exists(path + txtFileName))
            {
                //System.Diagnostics.Process.Start(path + txtFileName);//打开此文件:
                text = File.ReadAllLines(path + txtFileName, Encoding.Default);
            }
            return text;
        }


        /// <summary>
        /// 写入文本文件:
        /// </summary>
        /// <param name="txtFileName"></param>
        /// <param name="text"></param>
        public void WriteTxtFile(string txtFileName, string text)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。 
            FileStream fs = new FileStream(path + txtFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(text);
            sw.Close();
            fs.Close();

            //保存成功后打开此文件:
            //if (System.IO.File.Exists(path + txtFileName))
            //{
            //  System.Diagnostics.Process.Start(path + txtFileName);
            //}
        }


        /// <summary>
        /// 读取byte文件
        /// </summary>
        /// <param name="txtFileName"></param>
        /// <returns></returns>
        public static byte[] ReadByteFile(string txtFileName, byte[] savebuf)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。 
            FileStream file = new FileStream(path + txtFileName, FileMode.Open);
            file.Seek(0, SeekOrigin.Begin);
            file.Read(savebuf, 0, savebuf.Length);
            file.Close();
            return savebuf;
        }
        /// <summary>
        /// 写入byte文件
        /// </summary>
        /// <param name="txtFileName"></param>
        /// <param name="text"></param>
        public static void WriteByteFile(string txtFileName, byte[] savebuf)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。 
            FileStream file = new FileStream(path + txtFileName, FileMode.Create);
            file.Write(savebuf, 0, savebuf.Length);
            file.Flush();
            file.Close();
        }


    }
}
