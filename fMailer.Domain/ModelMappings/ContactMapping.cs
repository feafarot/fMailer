// ------------------------------------------------------------------------
// <copyright file="ContactMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class ContactMapping : ClassMap<Contact>
    {
        public ContactMapping()
        {
            Id(x => x.Id);

            Map(x => x.Email).Not.Nullable().Unique();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.MiddleName);
            Map(x => x.Organization);

            References(x => x.User).Not.Nullable().Cascade.SaveUpdate();

            HasManyToMany(x => x.Distributions);
            HasManyToMany(x => x.Groups);
        }
    }
}
