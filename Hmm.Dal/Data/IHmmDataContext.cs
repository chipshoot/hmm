﻿using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Microsoft.EntityFrameworkCore;

namespace Hmm.Dal.Data
{
    public interface IHmmDataContext
    {
        DbSet<HmmNote> Notes { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<NoteRender> Renders { get; set; }

        DbSet<NoteCatalog> Catalogs { get; set; }

        void Save();
    }
}