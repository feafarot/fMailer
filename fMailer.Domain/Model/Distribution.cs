// ------------------------------------------------------------------------
// <copyright file="Distribution.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class Distribution : IUnique
    {
        public Distribution()
        {
            IsClosed = false;
            Replies = new List<Reply>();
            Contacts = new List<Contact>();
            Groups = new List<ContactsGroup>();
            FailedDeliveredContancts = new List<Contact>();
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual MailTemplate Template { get; set; }

        public virtual IList<Reply> Replies { get; set; }

        public virtual IList<Contact> Contacts { get; set; }

        public virtual IList<ContactsGroup> Groups { get; set; }

        public virtual IList<Contact> FailedDeliveredContancts { get; set; }

        public virtual bool IsClosed { get; set; }

        [ScriptIgnore]
        public virtual User User { get; set; }

        public virtual void AddReply(Reply reply)
        {
            reply.Distribution = this;
            Replies.Add(reply);
        }
    }
}
