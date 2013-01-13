// ------------------------------------------------------------------------
// <copyright file="ReplyAttachmentMapping .cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.ModelMappings
{
    using System;
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class ReplyAttachmentMapping : ClassMap<ReplyAttachment>
    {
        public ReplyAttachmentMapping()
        {
            Id(x => x.Id);

            Map(x => x.ContentType).Not.Nullable();
            Map(x => x.Content).Not.Nullable().CustomSqlType("nvarchar(MAX)").Length(Int32.MaxValue);
            Map(x => x.Size).Not.Nullable();
            Map(x => x.Name).Not.Nullable();

            References(x => x.ParentReply).Cascade.All();
        }
    }
}
