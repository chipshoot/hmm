﻿using Hmm.Api.Models;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiNoteCatalogForUpdate : ApiEntity
    {
        public string Name { get; set; }

        public string Schema { get; set; }

        public bool IsDefault { get; set; }

        public string Description { get; set; }
    }
}