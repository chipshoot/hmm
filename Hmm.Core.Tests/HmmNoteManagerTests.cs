using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Contract;
using Hmm.Core.Manager;
using Hmm.Dal.Storages;
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
        private DateTime _currentDate;
        private readonly IHmmNoteManager<HmmNote> _manager;

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

            var noteStorage = new NoteStorage<HmmNote>(uowMock.Object, validator, lookupMock.Object, timeProviderMock.Object);
            var lkmoc = new Mock<IEntityLookup>();
            _manager = new HmmNoteManager<HmmNote>(noteStorage, lkmoc.Object);
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
            var note = new HmmNote
            {
                Author = user,
                Catalog = cat,
                Subject = "Testing note",
                Content = "Test content"
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(
                "<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Test content</content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.NotNull(newNote);
            Assert.Equal(1, newNote.Id);
            Assert.Equal("Testing note", newNote.Subject);
            Assert.Equal(xmldoc.InnerXml, newNote.Content);
            Assert.Equal(_currentDate, newNote.CreateDate);
            Assert.Equal(_currentDate, newNote.LastModifiedDate);

            Assert.Single(_notes);
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
            var note = new HmmNote
            {
                Author = user,
                Catalog = cat,
                Subject = "Testing note",
                Content = null
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content></content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.NotNull(newNote);

            Assert.Equal(xmldoc.InnerXml, newNote.Content);

            Assert.Single(_notes);
            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
        }

        [Fact]
        public void CanGetRightXmlWithNoteContentContainsInvalidCharToDataSource()
        {
            // Arrange - note with null content
            var user = _authors[0];
            var cat = _cats[0];
            var note = new HmmNote
            {
                Author = user,
                Catalog = cat,
                Subject = "Testing note",
                Content = "Testing content with < and >"
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Testing content with &lt; and &gt;</content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.NotNull(newNote);

            Assert.Equal(xmldoc.InnerXml, newNote.Content);

            Assert.Single(_notes);
            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
        }

        [Fact]
        public void CanUpdateNote()
        {
            // Arrange - note with null content
            var user = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Testing content with &lt; and &gt;</content></note>");
            var note = new HmmNote
            {
                Id = 1,
                Author = user,
                Catalog = cat,
                Subject = "Testing note",
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            Assert.Single(_notes);

            // Act
            note.Subject = "new note subject";
            note.Content = "This is new note content";
            var oldDate = _currentDate;
            _currentDate = DateTime.Now.AddDays(1);
            var newNote = _manager.Update(note);

            // Assert
            Assert.NotNull(newNote);
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>This is new note content</content></note>");
            Assert.Equal(xmldoc.InnerXml, newNote.Content);
            Assert.Equal("new note subject", newNote.Subject);
            Assert.Equal(oldDate, newNote.CreateDate);
            Assert.Equal(_currentDate, newNote.LastModifiedDate);

            Assert.Single(_notes);
            Assert.Equal("new note subject", _notes[0].Subject);
            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
            Assert.Equal(oldDate, _notes[0].CreateDate);
            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
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