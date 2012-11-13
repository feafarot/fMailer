// -----------------------------------------------------------------------
// <copyright file="SessionMapping.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class SessionMapping : ClassMap<Session>
    {
        public SessionMapping()
        {
            Id(x => x.Id);

            Map(x => x.Outdated).Not.Nullable().Default("0");
            Map(x => x.SessionGuid).Not.Nullable();
            Map(x => x.StartedAt).Not.Nullable().Default("getdate()");

            References(x => x.User).Not.Nullable().Cascade.SaveUpdate();
        }
    }
}
