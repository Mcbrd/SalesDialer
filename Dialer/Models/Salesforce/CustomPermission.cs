﻿using Newtonsoft.Json;
using Salesforce.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialer.Models.Salesforce
{
    public class CustomPermission
    {
        [Key]
        [Display(Name = "Custom Permission ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "Deleted")]
        [Createable(false), Updateable(false)]
        public bool IsDeleted { get; set; }

        [Display(Name = "Name")]
        [StringLength(80)]
        [Createable(false), Updateable(false)]
        public string DeveloperName { get; set; }

        [Display(Name = "Master Language")]
        [Createable(false), Updateable(false)]
        public string Language { get; set; }

        [Display(Name = "Label")]
        [StringLength(80)]
        [Createable(false), Updateable(false)]
        public string MasterLabel { get; set; }

        [Display(Name = "Namespace Prefix")]
        [StringLength(15)]
        [Createable(false), Updateable(false)]
        public string NamespacePrefix { get; set; }

        [Display(Name = "Created Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Created By ID")]
        [Createable(false), Updateable(false)]
        public string CreatedById { get; set; }

        [Display(Name = "Last Modified Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LastModifiedDate { get; set; }

        [Display(Name = "Last Modified By ID")]
        [Createable(false), Updateable(false)]
        public string LastModifiedById { get; set; }

        [Display(Name = "System Modstamp")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset SystemModstamp { get; set; }

        [Createable(false), Updateable(false)]
        public string Description { get; set; }

    }
}
