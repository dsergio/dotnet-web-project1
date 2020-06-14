using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Data
{
	public class ModelObject : EntityBase
    {
        public string EmailAddress { get => _EmailAddress; set => _EmailAddress = value ?? throw new ArgumentNullException(nameof(EmailAddress)); }
        private string _EmailAddress = string.Empty;
        public string? LinkToFile { get; set; }
        public DateTime? Expiration { get; set; } // only return valid links

        public ModelObject()
            : this("")
        { }

        public ModelObject(string emailAddress) 
            : this(emailAddress, null, null)
        { }

        public ModelObject(string emailAddress, string? linkToFile, DateTime? expiration)
        {
            EmailAddress = emailAddress;
            LinkToFile = linkToFile;
            Expiration = expiration;
        }

    }
}
