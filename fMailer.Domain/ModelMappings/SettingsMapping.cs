// ------------------------------------------------------------------------
// <copyright file="SettingsMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class SettingsMapping : ClassMap<Settings>
    {
        public SettingsMapping()
        {
            Id(x => x.Id);

            Map(x => x.Signature);
            Map(x => x.EmailAddressFrom);
            Map(x => x.Username);
            Map(x => x.Password);

            Map(x => x.Pop3Address);
            Map(x => x.Pop3Prot);
            Map(x => x.Pop3UseSsl);

            Map(x => x.SmtpAddress);
            Map(x => x.SmtpSslPort);
            Map(x => x.SmtpTlsPort);
            Map(x => x.SmtpUseAuth);
            Map(x => x.SmtpUseSsl);
        }
    }
}
