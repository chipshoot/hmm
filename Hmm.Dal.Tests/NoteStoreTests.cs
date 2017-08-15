using Castle.Components.DictionaryAdapter;
using DomainEntity.Misc;
using DomainEntity.User;
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

namespace Hmm.Dal.Tests
{
    public class NoteStorageTests : IDisposable
    {
        private readonly List<HmmNote> _notes;
        private readonly List<User> _authors;
        private readonly List<NoteCatalog> _cats;
        private readonly List<NoteRender> _renders;
        private readonly NoteStorage _noteStorage;
        private DateTime _currentDate = DateTime.Now;

        public NoteStorageTests()
        {
            _notes = new List<HmmNote>();
            _authors = new List<User>();
            _cats = new List<NoteCatalog>();
            _renders = new EditableList<NoteRender>();

            // add seed data
            var user = new User
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
            };
            _authors.AddEntity(user);
            var cat = new NoteCatalog
            {
                Id = 1,
                Name = "Gas Log",
                Description = "Testing catalog"
            };
            _cats.AddEntity(cat);
            var render = new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Namespace = "Hmm.Renders",
                Description = "Testing default note render"
            };
            _renders.AddEntity(render);

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

            // set up note validator
            var validator = new HmmNoteValidator(lookupMock.Object);

            // set up date time provider
            //_currentDate = DateTime.Now;
            var timeProviderMock = new Mock<IDateTimeProvider>();
            timeProviderMock.Setup(t => t.UtcNow).Returns(() => _currentDate);

            _noteStorage = new NoteStorage(uowMock.Object, validator, lookupMock.Object, timeProviderMock.Object);
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
            Assert.Equal(_currentDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
        }

        [Fact]
        public void CannotAddNullNote()
        {
            // Arrange
            HmmNote note = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = _noteStorage.Add(note);

            // Asset
            Assert.Null(result);
            Assert.True(_noteStorage.Validator.ValidationErrors.Count == 1);
        }

        [Fact]
        public void CanNotAddSameNoteTwiceToRepository()
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
            var savedRec2 = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(1, _notes.Count);
            Assert.Null(savedRec2);
        }

        [Fact]
        public void CanAddNoteWithNonExistRender()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotAddNoteWithNonExistAuthor()
        {
            // Arrange - null author for note
            var cat = _cats[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = null,
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
            Assert.Null(savedRec);
            Assert.Equal(0, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);

            // Arrange - none exists author
            var author = new User
            {
                Id = 2,
                FirstName = "Amy",
                LastName = "Wang",
                AccountName = "jfang",
                BirthDay = new DateTime(1977, 05, 21),
                Password = "lucky1",
                Salt = "passwordSalt",
                IsActivated = true,
                Description = "testing user"
            };
            note.Author = author;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);

            // Arrange - author with invalid author id
            author.Id = 0;
            note.Author = author;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);
        }

        [Fact]
        public void CannotAddNoteWithNonExistCatalog()
        {
            // Arrange - null catalog for note
            var author = _authors[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = author,
                Catalog = null,
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
            Assert.Null(savedRec);
            Assert.Equal(0, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);

            // Arrange - none exists catalog
            var cat = new NoteCatalog
            {
                Id = 2,
                Name = "Gas Log",
                Description = "Testing catalog"
            };
            note.Catalog = cat;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);

            // Arrange - catalog with invalid catalog id
            cat.Id = 0;
            note.Catalog = cat;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);
        }

        [Fact]
        public void CanUpdateNoteDescriptioin()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Render = render,
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            var oldDate = _currentDate;
            _currentDate = _currentDate.AddDays(1);

            // Act
            note.Description = "testing note2";
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal("testing note2", savedRec.Description);
            Assert.Equal(1, _notes.Count);
            Assert.Equal(oldDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
        }

        [Fact]
        public void CanUpdateNoteSubject()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Render = render,
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            var oldDate = _currentDate;
            _currentDate = _currentDate.AddDays(1);

            // Act
            note.Subject = "This is new subject";
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal("This is new subject", savedRec.Subject);
            Assert.Equal(1, _notes.Count);
            Assert.Equal(oldDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
        }

        [Fact]
        public void CanUpdateNoteContent()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Render = render,
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            var oldDate = _currentDate;
            _currentDate = _currentDate.AddDays(1);

            // Act
            var newXml = new XmlDocument();
            newXml.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><GasLog></GasLog>");
            note.Content = newXml.InnerXml;
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(newXml.InnerXml, savedRec.Content);
            Assert.NotEqual(xmldoc.InnerXml, savedRec.Content);
            Assert.Equal(1, _notes.Count);
            Assert.Equal(oldDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
        }

        [Fact]
        public void CanUpdateNoteCatalog()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotUpdateNoteCatalogToNonExistsCatalog()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanUpdateNoteRender()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotUpdateNoteRenderToNonExistsRender()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotUpdateNoteAuthor()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotUpdateNonExitsNote()
        {
            // Arrange - non exists id
            var author = _authors[0];
            var cat = _cats[0];
            var render = _renders[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note2",
                Subject = "testing note is here",
                Render = render,
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);

            // Act
            note.Id = 2;
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(1, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);

            // Arrange - invalid id
            note.Id = 0;

            // Act
            savedRec = _noteStorage.Update(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(1, _notes.Count);
            Assert.Equal(1, _noteStorage.Validator.ValidationErrors.Count);
        }

        [Fact]
        public void CanDeleteNote()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotDeleteNonExistsNote()
        {
            throw new NotImplementedException();
        }
    }
}