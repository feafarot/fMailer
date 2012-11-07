// ------------------------------------------------------------------------
// <copyright file="MD5HashProvider.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

namespace fMailer.Web.Core.HashProviders
{
    public class MD5HashProvider : IHashProvider
    {
        public string CalculateHash(string value)
        {
            var cryptoServiceProvider = new MD5CryptoServiceProvider();
            var buffer = Encoding.UTF8.GetBytes(value);
            buffer = cryptoServiceProvider.ComputeHash(buffer);
            var hashBuilder= new StringBuilder();
            foreach (byte b in buffer)
            {
                hashBuilder.Append(b.ToString("x2").ToLower());
            }

            return hashBuilder.ToString();
        }
    }
}