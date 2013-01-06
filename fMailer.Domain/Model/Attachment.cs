// ----------------------------------------------------------------------
// <copyright file="Attachment.cs" company="feafarot">
//  Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Web.Script.Serialization;

    public class Attachment : IUnique
    {
        public virtual int Id { get; set; }

        [ScriptIgnore]
        public virtual byte[] Content { get; set; }

        public virtual string ContentType { get; set; }

        public virtual int Size { get; set; }

        public virtual string Name { get; set; }

        [ScriptIgnore]
        public virtual MailTemplate ParentTemplate { get; set; }
    }
}
