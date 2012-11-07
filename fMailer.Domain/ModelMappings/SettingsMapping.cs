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
        }
    }
}
