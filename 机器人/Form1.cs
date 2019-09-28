using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace 机器人
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string paraUrlCoded = "{\"msgtype\":\"text\",\"text\":{\"content\":\"";
            paraUrlCoded += textBox2.Text;
            paraUrlCoded += "\"}}";
            Post(paraUrlCoded);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string paraUrlCoded = "{\"msgtype\": \"link\", \"link\": {\"text\": \"我的博客：欢迎光临\", \"title\": \"推广博客啦，机器人开发者\", \"picUrl\": \"\", \"messageUrl\": \"http://www.cnblogs.com/left69/\"}}";
            Post(paraUrlCoded);
        }

        private void Post(string paraUrlCoded)
        {
            string url = textBox1.Text;//webhook地址
            string strURL = url;
            HttpWebRequest request;//创建请求
            request = (HttpWebRequest)WebRequest.Create(strURL);//请求之创建带地址请求
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";//头文件补完

            byte[] payload;//请求的负载
            payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();//写入负载流
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            HttpWebResponse response;//接受应答
            response = (HttpWebResponse)request.GetResponse();
            Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            label3.Text = strValue;
        }

		private void button3_Click(object sender, EventArgs e)
		{
			string paraUrlCoded = "{\"msgtype\":\"image\",\"image\":{\"base64\":\"";
			paraUrlCoded += GetImageBase64andMd5(@"S:\code\机器人\机器人\bin\Debug\test.jpg")[0];
			paraUrlCoded += "\",\"md5\":\"";
			//paraUrlCoded +=GetMD5HashFromFile(@"S:\code\机器人\机器人\bin\Debug\test.jpg");
			paraUrlCoded += GetImageBase64andMd5(@"S:\code\机器人\机器人\bin\Debug\test.jpg")[1];
			paraUrlCoded += "\"}}";
			Post(paraUrlCoded);
			textBox3.Text = paraUrlCoded;
		}
		/// <summary>
		/// 图片转Base64
		/// </summary>
		/// <param name="ImageFileName">图片的完整路径</param>
		/// <returns></returns>
		public static string ImgToBase64(string imageFilePath)
		{
			try
			{
				Bitmap bmp = new Bitmap(imageFilePath);

				MemoryStream ms = new MemoryStream();
				bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
				byte[] arr = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(arr, 0, (int)ms.Length);
				ms.Close();
				return Convert.ToBase64String(arr);
			}
			catch (Exception)
			{
				return null;
			}
		}
		private static string GetMD5HashFromFile(string imageFilePath)
		{
			try
			{
				FileStream file = new FileStream(imageFilePath, FileMode.Open);
				System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				file.Close();

				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				return sb.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
			}
		}
		private static string[] GetImageBase64andMd5(string imageFilePath)
		{
			string[] strr = new string[2];
			try//get base64
			{
				
			Bitmap bmp = new Bitmap(imageFilePath);
			MemoryStream ms = new MemoryStream();
			
			bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
			byte[] arr = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(arr, 0, (int)ms.Length);
			ms.Close();
			ms.Dispose();
			strr[0] = Convert.ToBase64String(arr);
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
			}
			//base64get



			try//get md5
			{
				FileStream file = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				file.Close();

				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				strr[1] = sb.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
			}
			return strr;

		}
	}
}
