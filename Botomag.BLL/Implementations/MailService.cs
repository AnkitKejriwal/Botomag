using System;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using AutoMapper;

using Botomag.BLL.Contracts;
using Botomag.DAL;

namespace Botomag.BLL.Implementations
{
    /// <summary>
    /// General service for working with email messaging in app
    /// </summary>
    public class MailService : BaseService, IMailService
    {
        #region Constructors

        public MailService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        #endregion Constructors
        /// <summary>
        /// Send message on behalf webmaster
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public async Task SendMessageAsync(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("to is not defined.");
            }


            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            if (string.IsNullOrEmpty(smtpServer))
            {
                throw new ArgumentNullException("smtpServer is not defined.");
            }

            string smtpLogin = ConfigurationManager.AppSettings["smtpLogin"];
            if (string.IsNullOrEmpty(smtpLogin))
            {
                throw new ArgumentNullException("smtpLogin is not defined.");
            }

            string smtpPass = ConfigurationManager.AppSettings["smtpPass"];
            if (string.IsNullOrEmpty(smtpPass))
            {
                throw new ArgumentNullException("smtpPass is not defined.");
            }

            string webmasterMail = ConfigurationManager.AppSettings["webmasterMail"];
            if (string.IsNullOrEmpty(webmasterMail))
            {
                throw new ArgumentNullException("webmasterMail is not defined.");
            }

            using(SmtpClient client = new SmtpClient())
            {
                client.Host = smtpServer;
                client.Credentials = new NetworkCredential(smtpLogin, smtpPass);
                if (subject == null)
                {
                    subject = "";
                }
                if (body == null)
                {
                    body = "";
                }
                MailMessage message = new MailMessage(webmasterMail, to, subject, body);
                await client.SendMailAsync(message);
            }
        }
    }
}
