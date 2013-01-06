// ------------------------------------------------------------------------
// <copyright file="MailTemplate.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class MailTemplate : IUnique
    {
        public MailTemplate()
        {
            Attachments = new List<Attachment>();
            Distributions = new List<Distribution>();
        }

        public virtual int Id { get; set; }

        public virtual string Text { get; set; }

        public virtual string Name { get; set; }

        public virtual string Subject { get; set; }

        public virtual IList<Attachment> Attachments { get; set; }

        [ScriptIgnore]
        public virtual IList<Distribution> Distributions { get; set; }

        [ScriptIgnore]
        public virtual User User { get; set; }

        public virtual void AddAttachment(Attachment attachment)
        {
            attachment.ParentTemplate = this;
            Attachments.Add(attachment);
        }

        public virtual void UpdateAttachments()
        {
            foreach (var attachment in Attachments)
            {
                attachment.ParentTemplate = this;
            }
        }

        public virtual MailTemplate Clone()
        {
            return (MailTemplate)this.MemberwiseClone();
        }
    }
}
