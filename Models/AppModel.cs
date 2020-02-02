using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTest001.Models
{
    public class UserInformation
    {
        [Key]
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string TransactionNo { get; set; }

    }

    public class UserDocument
    {
        [Key]
        public Guid DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public int DocumentSize { get; set; }
        public string DocumentPath { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; } //Email Of the User Who Created the Document


        public string TransactionNo { get; set; }

        //public virtual UserInformation UserInformation { get; set; }
    }
}