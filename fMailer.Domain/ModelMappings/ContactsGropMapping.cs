// ------------------------------------------------------------------------
// <copyright file="ContactsGropMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class ContactsGropMapping : ClassMap<ContactsGroup>
    {
        public ContactsGropMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name).Not.Nullable().Unique();
            Map(x => x.Description);

            References(x => x.User).Not.Nullable().Cascade.SaveUpdate();

            HasManyToMany(x => x.Contacts);
            HasManyToMany(x => x.Distributions);
        }
    }
}
