// ------------------------------------------------------------------------
// <copyright file="FailedDeliveryMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class FailedDeliveryMapping : ClassMap<FailedDelivery>
    {
        public FailedDeliveryMapping()
        {
            Id(x => x.Id);

            Map(x => x.EmailText).Not.Nullable().CustomSqlType("nvarchar(MAX)");
            Map(x => x.IsNew).Not.Nullable().Default("1");
            Map(x => x.Subject).Not.Nullable();

            References(x => x.To).Not.Nullable().Cascade.SaveUpdate();
            References(x => x.Distribution).Not.Nullable().Cascade.SaveUpdate();
        }
    }
}
