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
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);
            HasManyToMany(x => x.Contacts);
        }
    }
}
