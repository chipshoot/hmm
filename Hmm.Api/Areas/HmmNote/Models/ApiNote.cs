﻿using System;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiNote : ApiEntity
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public ApiUser Author { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}