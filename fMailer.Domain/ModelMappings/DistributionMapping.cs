// ------------------------------------------------------------------------
// <copyright file="DistributionMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class DistributionMapping : ClassMap<Distribution>
    {
        public DistributionMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name).Not.Nullable();
            Map(x => x.IsClosed).Not.Nullable().Default("0");

            References(x => x.User).Not.Nullable();
            References(x => x.Template).ForeignKey().Not.Nullable();

            HasMany(x => x.Replies).Cascade.All();
            HasMany(x => x.FailedDeliveries).Cascade.All();

            HasManyToMany(x => x.Contacts).Table("ContantToDistribution");
            HasManyToMany(x => x.Groups).Table("ContantsGroupToDistribution");
        }
    }
}
