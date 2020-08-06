using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Dahab.Models;

namespace Dahab
{
    public class Methods
    {
        // DataBase Context 
        static DB db = new DB();

        public static string CipherKey = ConfigurationManager.AppSettings["Cipher"].ToString();
        public static string Domain = ConfigurationManager.AppSettings["Domain"].ToString();

        /// <summary>
        /// Encrypt A string And Retuen Encryption String
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string encrypt(string encryptString)
        {
            string EncryptionKey = CipherKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        /// <summary>
        /// Send Html E-Mail To Muli-Users
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="file"></param>
        /// <param name="To"></param>
        public static void Send_Mail(string Subject, HttpPostedFileBase file, List<string> To)
        {
            string From = ConfigurationManager.AppSettings["Email"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            foreach (var item in To)
            {
                if (item.Contains("@"))
                {
                    mail.To.Add(item);
                }
            }
            mail.Subject = Subject;
            StreamReader read = new StreamReader(file.InputStream);
            mail.Body = read.ReadToEnd();
            mail.IsBodyHtml = true;
            ///-------------------------------------------------------------------------//
            SmtpClient smtpMail = new SmtpClient();
            smtpMail.EnableSsl = false;
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpMail.Host = Host;
            smtpMail.Port = Port;

            smtpMail.UseDefaultCredentials = false;
            smtpMail.Credentials = new NetworkCredential(From, Pass);
            ///-------------------------------------------------------------------------//
            smtpMail.Send(mail);
        }
        /// <summary>
        /// Send Password To E-Mail
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="file"></param>
        /// <param name="To"></param>
        public static void Send_Password(string UserPass, string To)
        {
            string From = ConfigurationManager.AppSettings["Email"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            mail.To.Add(To);
            mail.Subject = "FreeHands Confirmation Password";

            mail.Body = "Hi , Your Password Is : " + UserPass;
            ///-------------------------------------------------------------------------//
            SmtpClient smtpMail = new SmtpClient();
            smtpMail.EnableSsl = false;
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpMail.Host = Host;
            smtpMail.Port = Port;

            smtpMail.UseDefaultCredentials = false;
            smtpMail.Credentials = new NetworkCredential(From, Pass);
            ///-------------------------------------------------------------------------//
            smtpMail.Send(mail);
        }

        /// <summary>
        /// This Function For Return Random Number From 0 to 999999
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {
            Random random = new Random();
            return random.Next(999999);
        }

        /// <summary>
        /// Find User That Register Using FaceBook
        /// </summary>
        /// <param name="FaceBook_ID"></param>
        /// <returns></returns>
        public static object FindFaceBookUser(string FaceBook_ID)
        {
            var facebookuser = db.Users.Where(x => x.Facebook_ID == FaceBook_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Token,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.Email,
                x.Password,
                x.Facebook_ID,
                x.Address,
                x.AllowNews,
                x.BillAddress,
            }).FirstOrDefault();

            return facebookuser;
        }

        /// <summary>
        /// This Function For Return The Price After Discount
        /// </summary>
        /// <param name="Price"></param>
        /// <param name="Discount"></param>
        /// <returns></returns>
        public static double GetPriceAfterDiscount(double Price,double? Discount)
        {
            if(Discount.HasValue)
            {
                double DiscountPrice = (Discount.Value * Price) / 100;
                Price = Price - DiscountPrice;
            }
            return Price;
        }
        
    }
}