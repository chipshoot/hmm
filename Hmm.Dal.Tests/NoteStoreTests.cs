using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Storages;
using Hmm.Dal.Validation;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.TestHelp;
using Microsoft.Extensions.Configuration;
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
        private readonly NoteStorage<HmmNote> _noteStorage;
        private DateTime _currentDate = DateTime.Now;
        private IDataSourceProvider _dsp;

        public NoteStorageTests()
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

            var isUseMoc = IsUsingMocEnv();
            _dsp = isUseMoc ? GetMockDataSource() : GetDatabase();

            // set up note validator
            var validator = new HmmNoteValidator(_dsp.Lookup);

            _noteStorage = new NoteStorage<HmmNote>(_dsp.UnitOfWork, validator, _dsp.Lookup, _dsp.DateTimeAdapter);
        }

        private IDataSourceProvider GetMockDataSource()
        {
            // set up unit of work
            IUnitOfWork Uowp()
            {
                var uowMock = new Mock<IUnitOfWork>();
                uowMock.Setup(u => u.Add(It.IsAny<HmmNote>()))
                    .Returns((HmmNote note) =>
                    {
                        note.Id = _notes.GetNextId();
                        _notes.AddEntity(note);
                        var savedRec = _notes.FirstOrDefault(n => n.Id == note.Id);
                        return savedRec;
                    });
                uowMock.Setup(u => u.Delete(It.IsAny<HmmNote>()))
                    .Callback((HmmNote note) =>
                    {
                        var rec = _notes.FirstOrDefault(n => n.Id == note.Id);
                        if (rec != null)
                        {
                            _notes.Remove(rec);
                        }
                    });
                uowMock.Setup(u => u.Update(It.IsAny<HmmNote>()))
                    .Callback((HmmNote note) =>
                    {
                        var rec = _notes.FirstOrDefault(n => n.Id == note.Id);
                        if (rec == null)
                        {
                            return;
                        }

                        _notes.Remove(rec);
                        _notes.AddEntity(note);
                    });

                return uowMock.Object;
            }

            // set up look up repository
            IEntityLookup Lkp()
            {
                var lookupMock = new Mock<IEntityLookup>();
                lookupMock.Setup(lk => lk.GetEntity<HmmNote>(It.IsAny<int>()))
                    .Returns((int id) =>
                    {
                        var rec = _notes.FirstOrDefault(n => n.Id == id);
                        return rec;
                    });
                lookupMock.Setup(lk => lk.GetEntity<User>(It.IsAny<int>()))
                    .Returns((int id) =>
                    {
                        var rec = _authors.FirstOrDefault(n => n.Id == id);
                        return rec;
                    });
                lookupMock.Setup(lk => lk.GetEntity<NoteCatalog>(It.IsAny<int>()))
                    .Returns((int id) =>
                    {
                        var rec = _cats.FirstOrDefault(n => n.Id == id);
                        return rec;
                    });
                lookupMock.Setup(lk => lk.GetEntity<NoteRender>(It.IsAny<int>()))
                    .Returns((int id) =>
                    {
                        var rec = _renders.FirstOrDefault(n => n.Id == id);
                        return rec;
                    });

                return lookupMock.Object;
            }

            // set up date time provider
            IDateTimeProvider Dtp()
            {
                //_currentDate = DateTime.Now;
                var timeProviderMock = new Mock<IDateTimeProvider>();
                timeProviderMock.Setup(t => t.UtcNow).Returns(() => _currentDate);
                return timeProviderMock.Object;
            }

            var dsp = new DataSourceProvider(Lkp, Uowp, Dtp);

            return dsp;
        }

        private IDataSourceProvider GetDatabase()
        {
            return null;
        }

        private static bool IsUsingMocEnv()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            var useMocconf = config["TestEnviornment:UseMoc"];
            if (useMocconf == null || !bool.TryParse(useMocconf, out var isUseMoc))
            {
                isUseMoc = true;
            }

            return isUseMoc;
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
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(_currentDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
            Assert.Single(_notes);
            Assert.Equal(_currentDate, _notes[0].CreateDate);
            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
        }

        [Fact]
        public void CannotAddNullNote()
        {
            // Arrange
            HmmNote note = null;
            Assert.Empty(_notes);

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = _noteStorage.Add(note);

            // Asset
            Assert.Null(result);
            Assert.True(_noteStorage.Validator.ValidationErrors.Count == 1);
            Assert.Empty(_notes);
        }

        [Fact(Skip = "Just ignore this for the time being, need figure out the way to test transaction")]
        public void CanNotAddSameNoteTwiceToRepository()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
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
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);
            var savedRec2 = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Single(_notes);
            Assert.Null(savedRec2);
        }

        [Fact]
        public void CanAddNoteWithNullRenderDefaultRenderApplied()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(_currentDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
            Assert.Single(_notes);
            Assert.Equal(_currentDate, _notes[0].CreateDate);
            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
        }

        [Fact]
        public void CanAddNoteWithNonExistRenderDefaultRenderApplied()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Single(_notes);
            Assert.Equal(_currentDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
        }

        [Fact]
        public void CanAddNoteWithRenderWithInvalidIdDefaultRenderApplied()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Single(_notes);
            Assert.Equal(_currentDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
        }

        [Fact]
        public void CannotAddNoteWithNonExistAuthor()
        {
            // Arrange - null author for note
            var cat = _cats[0];
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
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Empty(_notes);
            Assert.Single(_noteStorage.Validator.ValidationErrors);

            // Arrange - none exists author
            var author = new User
            {
                Id = 3,
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
            Assert.Empty(_notes);
            Assert.Single(_noteStorage.Validator.ValidationErrors);

            // Arrange - author with invalid author id
            author.Id = 0;
            note.Author = author;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Empty(_notes);
            Assert.Single(_noteStorage.Validator.ValidationErrors);
        }

        [Fact]
        public void CanAddNoteWithNonExistCatalogDefaultCatalogApplied()
        {
            // Arrange - null catalog for note
            var author = _authors[0];
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
                Content = xmldoc.InnerXml
            };
            Assert.Empty(_notes);

            // Act
            var savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Single(_notes);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);

            // Arrange - none exists catalog
            var cat = new NoteCatalog
            {
                Id = 200,
                Name = "Gas Log",
                Description = "Testing catalog"
            };
            note.Catalog = cat;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(2, _notes.Count);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);

            // Arrange - catalog with invalid catalog id
            cat.Id = 0;
            note.Catalog = cat;

            // Act
            savedRec = _noteStorage.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(3, _notes.Count);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteDescriptioin()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            var oldDate = _currentDate;
            _currentDate = _currentDate.AddDays(1);
            Assert.Single(_notes);

            // Act
            note.Description = "testing note2";
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal("testing note2", savedRec.Description);
            Assert.Single(_notes);
            Assert.Equal(oldDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
            Assert.Equal("testing note2", _notes[0].Description);
        }

        [Fact]
        public void CanUpdateNoteSubject()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            var oldDate = _currentDate;
            _currentDate = _currentDate.AddDays(1);
            Assert.Single(_notes);

            // Act
            note.Subject = "This is new subject";
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal("This is new subject", savedRec.Subject);
            Assert.Equal(oldDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);
            Assert.Single(_notes);
            Assert.Equal(1, _notes[0].Id);
            Assert.Equal("This is new subject", _notes[0].Subject);
            Assert.Equal(oldDate, _notes[0].CreateDate);
            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
        }

        [Fact]
        public void CanUpdateNoteContent()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            var oldDate = _currentDate;
            _currentDate = _currentDate.AddDays(1);
            Assert.Single(_notes);

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
            Assert.Equal(oldDate, savedRec.CreateDate);
            Assert.Equal(_currentDate, savedRec.LastModifiedDate);

            Assert.Single(_notes);
            Assert.Equal(1, _notes[0].Id);
            Assert.Equal(newXml.InnerXml, _notes[0].Content);
            Assert.NotEqual(xmldoc.InnerXml, _notes[0].Content);
            Assert.Equal(oldDate, _notes[0].CreateDate);
            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
        }

        [Fact]
        public void CanUpdateNoteCatalog()
        {
            // Arrange
            var author = _authors[0];
            var catalog = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            _notes.AddEntity(note);
            Assert.Equal("DefaultNoteCatalog", _notes[0].Catalog.Name);
            Assert.Single(_notes);

            // changed the note catalog
            note.Catalog = _cats[1];

            // Act
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("Gas Log", savedRec.Catalog.Name);

            Assert.Single(_notes);
            Assert.Equal("Gas Log", _notes[0].Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteCatalogToNullCatalogDefaultCatalogApplied()
        {
            // Arrange - null catalog for note
            var author = _authors[0];
            var catalog = _cats[1];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            _notes.AddEntity(note);
            Assert.Equal("Gas Log", _notes[0].Catalog.Name);

            // null the catalog
            note.Catalog = null;

            // Act
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);

            Assert.Single(_notes);
            Assert.Equal("DefaultNoteCatalog", _notes[0].Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteCatalogToNonExistsCatalogDefaultCatalogApplied()
        {
            // Arrange - none exists catalog
            var author = _authors[0];
            var catalog = new NoteCatalog
            {
                Id = 200,
                Name = "Gas Log",
                Description = "Testing catalog"
            };

            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            _notes.AddEntity(note);
            Assert.Single(_notes);

            // Act
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);

            Assert.Single(_notes);
            Assert.Equal("DefaultNoteCatalog", _notes[0].Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteCatalogToCatalogWithInvalidIdDefaultCatalogApplied()
        {
            // Arrange - none exists catalog
            var author = _authors[0];
            var catalog = new NoteCatalog
            {
                Id = -1,
                Name = "Gas Log",
                Description = "Testing catalog"
            };

            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            _notes.AddEntity(note);

            // Act
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);

            Assert.Single(_notes);
            Assert.Equal("DefaultNoteCatalog", _notes[0].Catalog.Name);
        }

        [Fact]
        public void CannotUpdateNoteAuthor()
        {
            // Arrange
            var author = _authors[0];
            var catalog = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            _notes.AddEntity(note);
            Assert.Equal("jfang", _notes[0].Author.AccountName);
            Assert.Single(_notes);

            // change the note render
            var newUser = _authors[1];
            note.Author = newUser;

            // Act
            var savedRec = _noteStorage.Update(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Single(_noteStorage.Validator.ValidationErrors);

            Assert.Single(_notes);
            Assert.Equal("jfang", _notes[0].Author.AccountName);
        }

        [Fact]
        public void CannotUpdateNonExitsNote()
        {
            // Arrange - non exists id
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note2",
                Subject = "testing note is here",
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
            Assert.Single(_notes);
            Assert.Single(_noteStorage.Validator.ValidationErrors);

            // Arrange - invalid id
            note.Id = 0;

            // Act
            savedRec = _noteStorage.Update(note);

            // Assert
            Assert.Null(savedRec);
            Assert.Single(_notes);
            Assert.Single(_noteStorage.Validator.ValidationErrors);
        }

        [Fact]
        public void CanDeleteNote()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            Assert.Single(_notes);

            // Act
            var result = _noteStorage.Delete(note);

            // Assert
            Assert.True(result);
            Assert.Empty(_notes);
        }

        [Fact]
        public void CannotDeleteNonExistsNote()
        {
            // Arrange
            var author = _authors[0];
            var cat = _cats[0];
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Id = 1,
                Author = author,
                Catalog = cat,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
                CreateDate = _currentDate,
                LastModifiedDate = _currentDate
            };
            _notes.AddEntity(note);
            Assert.Single(_notes);

            // change the note id to create a new note
            note.Id = 2;
            Assert.NotEqual(_notes[0].Id, note.Id);

            // Act
            var result = _noteStorage.Delete(note);

            // Assert
            Assert.False(result);
            Assert.Single(_notes);
            Assert.NotEmpty(_noteStorage.Validator.ValidationErrors);
        }
    }
}