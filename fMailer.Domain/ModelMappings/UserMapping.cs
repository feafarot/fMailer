// ------------------------------------------------------------------------
// <copyright file="UserMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("Users");
            Id(x => x.Id).GeneratedBy.Foreign("Settings");
            Map(x => x.Login).Not.Nullable();
            Map(x => x.Password).Not.Nullable();
            Map(x => x.Email);
            Map(x => x.CreatedOn).Not.Nullable().Default("getdate()").Generated.Always();
            HasOne(x => x.Settings).Constrained().ForeignKey();
            HasMany(x => x.Contacts).Cascade.All();
            HasMany(x => x.ContactsGroups).Cascade.All();
            HasMany(x => x.Templates).Cascade.All();
            HasMany(x => x.Distributions).Cascade.All();
        }
    }
}
