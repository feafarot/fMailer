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
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.MiddleName);
            Map(x => x.Email).Not.Nullable();
        }
    }
}
