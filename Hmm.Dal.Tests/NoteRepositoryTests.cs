using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Xml;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteRepositoryTests : IDisposable
    {
        private IConfigurationRoot _configuration;
        private HmmDataContext _dbcontext;

        public NoteRepositoryTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var conn = _configuration.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<HmmDataContext>().UseSqlServer(conn).Options;
            _dbcontext = new HmmDataContext(options);

            _dbcontext.Database.EnsureCreated();
            if (_dbcontext.Notes.Any())
            {
                return;
            }

            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");

            var author = _dbcontext.Users.Any(u => u.FirstName == "Jack")
                ? _dbcontext.Users.FirstOrDefault(u => u.FirstName == "Jack")
                : new User
                {
                    FirstName = "Jack",
                    LastName = "Fang",
                    AccountName = "jfang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                };

            var cat = _dbcontext.Catalogs.Any(c => c.Name == "Gas Log")
                ? _dbcontext.Catalogs.FirstOrDefault(c => c.Name == "Gas Log")
                : new NoteCatalog
                {
                    Name = "Gas Log",
                    Description = "Testing catalog"
                };

            var render = _dbcontext.Renders.Any(r => r.Name == "GasLog")
                ? _dbcontext.Renders.FirstOrDefault(r => r.Name == "GasLog")
                : new NoteRender
                {
                    Name = "GasLog",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                };

            var note = new HmmNote
            {
                Author = author,
                Catalog = cat,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Render = render,
                Content = xmldoc.InnerXml
            };
            _dbcontext.Notes.Add(note);
            _dbcontext.Entry(note.Author).State = note.Author.Id > 0 ? EntityState.Unchanged : EntityState.Added;
            _dbcontext.Entry(note.Catalog).State = note.Catalog.Id > 0 ? EntityState.Unchanged : EntityState.Added;
            _dbcontext.Entry(note.Render).State = note.Render.Id > 0 ? EntityState.Unchanged : EntityState.Added;
            _dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            if (!_dbcontext.Notes.Any())
            {
                return;
            }

            var notes = _dbcontext.Notes;
            foreach (var note in notes)
            {
                _dbcontext.Notes.Remove(note);
            }

            _dbcontext.SaveChanges();
        }

        [Fact]
        public void CanAddNoteToRepository()
        {
            // Arrange
            var repo = new DefaultRepository<HmmNote>();
            var note = new HmmNote
            {
                Id = 0,
                Subject = "Testing",
                Content = ""
            };

            // Act
            //var savedRec = repo.Add(note);

            // Assert
            //Assert.NotNull(savedRec);
            //Assert.Equal(1, savedRec.Id);
        }
    }
}