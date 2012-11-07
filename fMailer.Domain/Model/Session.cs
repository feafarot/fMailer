// ------------------------------------------------------------------------
// <copyright file="Session.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using fMailer.Domain.Model;

    public class Session : IUnique
    {
        public virtual int Id { get; set; }

        public virtual Guid SessionGuid { get; set; }
        
        public virtual bool Outdated { get; set; }

        public virtual User User { get; set; }

        public virtual DateTime StartedAt { get; set; }
    }
}