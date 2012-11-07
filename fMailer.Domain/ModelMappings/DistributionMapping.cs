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
            References(x => x.Template).ForeignKey();
            HasManyToMany(x => x.Contacts).Table("ContantToDistribution");
            HasManyToMany(x => x.ContactsGroups).Table("ContantsGroupToDistribution");
            HasManyToMany(x => x.FailedDeliveredContancts).Table("FailedDeliveryContantToDistribution");
            HasMany(x => x.Replies).Cascade.All();
        }
    }
}
