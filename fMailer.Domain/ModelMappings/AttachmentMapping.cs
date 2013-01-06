// ------------------------------------------------------------------------
// <copyright file="AttachmentMapping .cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using System;
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class AttachmentMapping : ClassMap<Attachment>
    {
        public AttachmentMapping()
        {
            Id(x => x.Id);

            Map(x => x.ContentType).Not.Nullable();
            Map(x => x.Content).Not.Nullable().Length(Int32.MaxValue);
            Map(x => x.Size).Not.Nullable();
            Map(x => x.Name).Not.Nullable();

            References(x => x.ParentTemplate).Not.Nullable().Cascade.SaveUpdate();
        }
    }
}
