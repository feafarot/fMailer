// ------------------------------------------------------------------------
// <copyright file="MailTemplateMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using fMailer.Domain.Model;
    using FluentNHibernate.Mapping;

    public class MailTemplateMapping : ClassMap<MailTemplate>
    {
        public MailTemplateMapping()
        {
            Id(x => x.Id);

            Map(x => x.Text).Not.Nullable().CustomSqlType("nvarchar(MAX)");
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);

            References(x => x.User).Column(ForeignKeys.UserFK).Not.Nullable();
        }
    }
}
