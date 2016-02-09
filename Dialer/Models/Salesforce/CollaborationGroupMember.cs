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
    public class CollaborationGroupMember
    {
        [Key]
        [Display(Name = "Group Member Id")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "CollaborationGroup ID")]
        [Updateable(false)]
        public string CollaborationGroupId { get; set; }

        [Display(Name = "Member ID")]
        [Updateable(false)]
        public string MemberId { get; set; }

        [Display(Name = "Group Member Role")]
        public string CollaborationRole { get; set; }

        [Display(Name = "Notification Frequency")]
        public string NotificationFrequency { get; set; }

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

        [Display(Name = "Last Feed Access Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? LastFeedAccessDate { get; set; }

    }
}
