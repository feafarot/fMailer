// ----------------------------------------------------------------------
// <copyright file="Contact.cs" company="feafarot">
//  Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Contact : IUnique
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string MiddleName { get; set; }

        public virtual string Email { get; set; }
    }
}
