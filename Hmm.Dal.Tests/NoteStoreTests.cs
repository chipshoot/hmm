using Castle.Components.DictionaryAdapter;
using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Validation;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteStorageTests : IDisposable
    {
        private readonly List<HmmNote> _notes;
        private readonly List<User> _authors;
        private readonly List<NoteCatalog> _cats;
        private readonly List<NoteRender> _renders;
        private readonly NoteStorage _noteStorage;

        public NoteStorageTests()
        {
            _notes = new List<HmmNote>();
            _authors = new List<User>();
            _cats = new List<NoteCatalog>();
            _renders = new EditableList<NoteRender>();

            // add seed data
            var user = new User
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
            _authors.AddEntity(user);
            var cat = new NoteCatalog
            {
                Name = "Gas Log",
                Description = "Testing catalog"
            };
            _cats.AddEntity(cat);
            var render = new NoteRender
            {
                Name = "GasLog",
                Namespace = "Hmm.Renders",
                Description = "Testing default note render"
            };
            _renders.AddEntity(render);

            // set up infrastructures
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.Add(It.IsAny<HmmNote>())).Returns((HmmNote note) =>
            {
                note.CreateDate = DateTime.Now;
                note.LastModifiedDate = DateTime.Now;
                note.Id = _notes.GetNextId();
                _notes.AddEntity(note);
                var savedRec = _notes.FirstOrDefault(n => n.Id == note.Id);
                return savedRec;
            });
            uowMock.Setup(u => u.Delete(It.IsAny<HmmNote>())).Callback((HmmNote note) =>
            {
                var rec = _notes.FirstOrDefault(n => n.Id == note.Id);
                if (rec != null)
                {
                    _notes.Remove(rec);
                }
            });
            uowMock.Setup(u => u.Update(It.IsAny<HmmNote>())).Callback((HmmNote note) =>
            {
                var rec = _notes.FirstOrDefault(n => n.Id == note.Id);
                if (rec == null)
                {
                    return;
                }
                rec.Author = note.Author;
                rec.Catalog = note.Catalog;
                rec.Render = note.Render;
                rec.Content = note.Content;
                rec.Subject = note.Subject;
                rec.LastModifiedDate = DateTime.Now;
            });

            var lookupMock = new Mock<IEntityLookup>();
            lookupMock.Setup(lk => lk.GetEntity<HmmNote>(It.IsAny<int>())).Returns((int id) =>
            {
                var rec = _notes.FirstOrDefault(n => n.Id == id);
                return rec;
            });

            var validator = new HmmNoteValidator(lookupMock.Object);
            _noteStorage = new NoteStorage(uowMock.Object, validator, lookupMock.Object);
        }

        public void Dispose()
        {
            _notes.Clear();
            _authors.Clear();
            _renders.Clear();
            _cats.Clear();
        }

        [Fact]
        public void CanAddNoteToRepository()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
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

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(1, _notes.Count);
        }
    }
}