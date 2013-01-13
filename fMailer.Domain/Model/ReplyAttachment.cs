// ----------------------------------------------------------------------
// <copyright file="ReplyAttachment.cs" company="feafarot">
//  Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Web.Script.Serialization;

    public class ReplyAttachment : IUnique
    {
        public virtual int Id { get; set; }

        [ScriptIgnore]
        public virtual byte[] Content { get; set; }

        public virtual string ContentType { get; set; }

        public virtual int Size { get; set; }

        public virtual string Name { get; set; }

        [ScriptIgnore]
        public virtual Reply ParentReply { get; set; }

        public virtual ReplyAttachment LiteClone()
        {
            var clone = this.MemberwiseClone() as ReplyAttachment;
            clone.Content = null;
            return clone;
        }
    }
}
