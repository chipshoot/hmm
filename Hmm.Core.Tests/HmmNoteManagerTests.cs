using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Contract;
using Hmm.Core.Manager;
using Hmm.Dal;
using Hmm.Dal.Validation;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;

namespace Hmm.Core.Tests
{
    public class HmmNoteManagerTests : IDisposable
    {
        private readonly List<HmmNote> _notes;
        private readonly List<NoteCatalog> _cats;
        private readonly List<NoteRender> _renders;
        private readonly List<User> _authors;
        private readonly DateTime _currentDate;
        private readonly IHmmNoteManager _manager;

        public HmmNoteManagerTests()
        {
            _notes = new List<HmmNote>();
            _authors = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Jack",
                    LastName = "Fang",
                    AccountName = "jfang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Amy",
                    LastName = "Wang",
                    AccountName = "awang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                }
            };
            _cats = new List<NoteCatalog>
            {
                new NoteCatalog
                {
                    Id = 1,
                    Name = "DefaultNoteCatalog",
                    Description = "Testing catalog"
                },
                new NoteCatalog
                {
                    Id = 2,
                    Name = "Gas Log",
                    Description = "Testing catalog"
                }
            };
            _renders = new List<NoteRender>
            {
                new NoteRender
                {
                    Id = 1,
                    Name = "DefaultNoteRender",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                },
                new NoteRender
                {
                    Id = 2,
                    Name = "GasLog",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                }
            };

            // set up unit of work
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.Add(It.IsAny<HmmNote>())).Returns((HmmNote note) =>
            {
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

                _notes.Remove(rec);
                _notes.AddEntity(note);
            });

            // set up look up repository
            var lookupMock = new Mock<IEntityLookup>();
            lookupMock.Setup(lk => lk.GetEntity<HmmNote>(It.IsAny<int>())).Returns((int id) =>
            {
                var rec = _notes.FirstOrDefault(n => n.Id == id);
                return rec;
            });
            lookupMock.Setup(lk => lk.GetEntity<User>(It.IsAny<int>())).Returns((int id) =>
            {
                var rec = _authors.FirstOrDefault(n => n.Id == id);
                return rec;
            });
            lookupMock.Setup(lk => lk.GetEntity<NoteCatalog>(It.IsAny<int>())).Returns((int id) =>
            {
                var rec = _cats.FirstOrDefault(n => n.Id == id);
                return rec;
            });
            lookupMock.Setup(lk => lk.GetEntity<NoteRender>(It.IsAny<int>())).Returns((int id) =>
            {
                var rec = _renders.FirstOrDefault(n => n.Id == id);
                return rec;
            });

            // set up note validator
            var validator = new HmmNoteValidator(lookupMock.Object);

            // set up date time provider
            _currentDate = DateTime.Now;
            var timeProviderMock = new Mock<IDateTimeProvider>();
            timeProviderMock.Setup(t => t.UtcNow).Returns(() => _currentDate);

            var noteStorage = new NoteStorage(uowMock.Object, validator, lookupMock.Object, timeProviderMock.Object);
            _manager = new HmmNoteManager(noteStorage);
        }

        public void Dispose()
        {
            _notes.Clear();
        }

        [Fact]
        public void CanAddNoteToDataSouce()
        {
            // Arrange
            var user = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var note = new HmmNote
            {
                Author = user,
                Catalog = cat,
                Render = render,
                Subject = "Testing note",
                Content = "Test content"
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(
                "<?xml version=\"1.0\" encoding=\"UTF-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Test content</content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.NotNull(newNote);
            Assert.Equal(1, newNote.Id);
            Assert.Equal("Testing note", newNote.Subject);
            Assert.Equal(xmldoc.InnerXml, newNote.Content);
            Assert.Equal(_currentDate, newNote.CreateDate);
            Assert.Equal(_currentDate, newNote.LastModifiedDate);

            Assert.Equal(1, _notes.Count);
            Assert.Equal(1, _notes[0].Id);
            Assert.Equal("Testing note", _notes[0].Subject);
            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
            Assert.Equal(_currentDate, _notes[0].CreateDate);
            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
        }

        [Fact]
        public void CanGetRightXmlWithNullNoteContentToDataSource()
        {
            // Arrange - note with null content
            var user = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var note = new HmmNote
            {
                Author = user,
                Catalog = cat,
                Render = render,
                Subject = "Testing note",
                Content = null
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content></content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.NotNull(newNote);

            Assert.Equal<string>(xmldoc.InnerXml, newNote.Content);

            Assert.Equal(1, _notes.Count);
            Assert.Equal<string>(xmldoc.InnerXml, _notes[0].Content);
        }

        [Fact]
        public void CanGetRightXmlWithNoteContentContainsInvalidCharToDataSource()
        {
            // Arrange - note with null content
            var user = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var note = new HmmNote
            {
                Author = user,
                Catalog = cat,
                Render = render,
                Subject = "Testing note",
                Content = "Testing content with < and >"
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Testing content with &lt; and &gt;</content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.NotNull(newNote);

            Assert.Equal<string>(xmldoc.InnerXml, newNote.Content);

            Assert.Equal(1, _notes.Count);
            Assert.Equal<string>(xmldoc.InnerXml, _notes[0].Content);
        }

        [Fact]
        public void CanUpdateNote()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanSearchNoteById()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanDeleteNote()
        {
            throw new NotImplementedException();
        }
    }
}