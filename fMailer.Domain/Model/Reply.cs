// ------------------------------------------------------------------------
// <copyright file="Reply.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;

    public class Reply : IUnique
    {
        public Reply()
        {
            IsNew = true;
            Attachments = new List<ReplyAttachment>();
        }

        public virtual int Id { get; set; }

        public virtual Contact From { get; set; }

        public virtual DateTime RecievedOn { get; set; }

        public virtual string Subject { get; set; }

        public virtual string EmailText { get; set; }

        public virtual IList<ReplyAttachment> Attachments { get; set; }

        [ScriptIgnore]
        public virtual Distribution Distribution { get; set; }

        public virtual bool IsNew { get; set; }

        public virtual void AddAttachment(ReplyAttachment attachment)
        {
            attachment.ParentReply = this;
            Attachments.Add(attachment);
        }

        public virtual Reply Clone()
        {
            var clone = this.MemberwiseClone() as Reply;
            clone.From = From;
            clone.Attachments = new List<ReplyAttachment>();
            foreach (var attach in Attachments)
            {
                clone.Attachments.Add(attach.LiteClone());
            }

            return clone;
        }
    }
}
