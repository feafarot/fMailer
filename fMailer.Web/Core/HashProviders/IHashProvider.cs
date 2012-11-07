// ------------------------------------------------------------------------
// <copyright file="IHashProvider.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Web.Core.HashProviders
{
    public interface IHashProvider
    {
        string CalculateHash(string value);
    }
}