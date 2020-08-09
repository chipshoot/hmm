﻿using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.TestHelp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteRepositoryTests : TestFixtureBase
    {
        private readonly User _author;
        private readonly NoteCatalog _catalog;

        public NoteRepositoryTests()
        {
            var authors = new List<User>
            {
                new User
                {
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
            var renders = new List<NoteRender>
            {
                new NoteRender
                {
                    Name = "DefaultNoteRender",
                    Namespace = "Hmm.Renders",
                    IsDefault = true,
                    Description = "Testing default note render"
                },
                new NoteRender
                {
                    Name = "GasLog",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                }
            };
            var catalogs = new List<NoteCatalog>
            {
                new NoteCatalog
                {
                    Name = "DefaultNoteCatalog",
                    Schema = "DefaultSchema",
                    Render = renders[0],
                    IsDefault = true,
                    Description = "Testing catalog"
                },
                new NoteCatalog
                {
                    Name = "Gas Log",
                    Schema = "GasLogSchema",
                    Render = renders[1],
                    Description = "Testing catalog"
                }
            };

            SetupRecords(authors, renders, catalogs, new List<GasDiscount>(), new List<Automobile>());

            _author = UserRepository.GetEntities().FirstOrDefault();
            _catalog = CatalogRepository.GetEntities().FirstOrDefault(cat => cat.IsDefault);
        }

        [Fact]
        public void CanAddNoteToRepository()
        {
            // Arrange
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");

            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmlDocument.InnerXml
            };

            // Act
            var savedRec = NoteRepository.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id >= 1, "savedRec.Id>=1");
            Assert.True(savedRec.Id == note.Id, "savedRec.Id==note.Id");
        }

        [Fact]
        public void CannotAddNullNote()
        {
            // Arrange
            HmmNote note = null;

            // Act
            // Asset
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => NoteRepository.Add(note));
        }

        [Theory]
        [ClassData(typeof(UserTestData))]
        public void CannotAddNoteWithNonExistAuthor(User user)
        {
            // Arrange - null author for note
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = user,
                Catalog = _catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };

            // Act
            var savedRec = NoteRepository.Add(note);

            // Assert
            Assert.Null(savedRec);
            Assert.False(NoteRepository.GetEntities().Any());
            Assert.False(NoteRepository.ProcessMessage.Success);
            Assert.Single(NoteRepository.ProcessMessage.MessageList);
        }

        [Theory]
        [ClassData(typeof(CatalogTestData))]
        public void CanAddNoteWithNonExistCatalogDefaultCatalogApplied(NoteCatalog catalog)
        {
            // Arrange - null catalog for note
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = catalog,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };

            // Act
            var savedRec = NoteRepository.Add(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id > 0, "savedRec.Id>0");
            Assert.True(savedRec.Id == note.Id, "savedRec.Id==note.Id");
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteDescription()
        {
            // Arrange
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
            };
            NoteRepository.Add(note);

            // Act
            note.Description = "testing note2";
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id >= 1, "savedRec.Id >= 1");
            Assert.True(savedRec.Id == note.Id, "savedRec.Id == note.Id");
            Assert.Equal("testing note2", savedRec.Description);
            Assert.Equal("testing note2", note.Description);
            Assert.True(NoteRepository.ProcessMessage.Success);
        }

        [Fact]
        public void CanUpdateNoteSubject()
        {
            // Arrange
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            // Act
            note.Subject = "This is new subject";
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id >= 1, "savedRec.Id >=1");
            Assert.Equal("This is new subject", savedRec.Subject);
            Assert.Equal(note.Id, savedRec.Id);
            Assert.Equal("This is new subject", note.Subject);
            Assert.True(NoteRepository.ProcessMessage.Success);
        }

        [Fact]
        public void CanUpdateNoteContent()
        {
            // Arrange
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            // Act
            var newXml = new XmlDocument();
            newXml.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><GasLog></GasLog>");
            note.Content = newXml.InnerXml;
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id >= 1, "savedRec.Id >=1");
            Assert.True(savedRec.Id == note.Id, "savedRec.Id == note.Id");
            Assert.Equal(newXml.InnerXml, savedRec.Content);
            Assert.NotEqual(xmldoc.InnerXml, savedRec.Content);
        }

        [Fact]
        public void CanUpdateNoteCatalog()
        {
            // Arrange
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            // changed the note catalog
            note.Catalog = CatalogRepository.GetEntities().FirstOrDefault(cat => !cat.IsDefault);

            // Act
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.NotNull(note.Catalog);
            Assert.Equal("Gas Log", savedRec.Catalog.Name);
            Assert.Equal("Gas Log", note.Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteCatalogToNullCatalogDefaultCatalogApplied()
        {
            // Arrange - null catalog for note
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var catalog = CatalogRepository.GetEntities().FirstOrDefault(cat => !cat.IsDefault);
            var note = new HmmNote
            {
                Author = _author,
                Catalog = catalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);
            var savedRec = NoteRepository.GetEntities().FirstOrDefault();
            Assert.NotNull(savedRec);
            Assert.Equal("Gas Log", savedRec.Catalog.Name);

            // null the catalog
            note.Catalog = null;

            // Act
            savedRec = NoteRepository.Update(note);

            // Assert
            Assert.True(NoteRepository.ProcessMessage.Success);
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteCatalogToNonExistsCatalogDefaultCatalogApplied()
        {
            // Arrange - none exists catalog
            var catalog = new NoteCatalog
            {
                Id = 200,
                Name = "Gas Log",
                Description = "Testing catalog"
            };

            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var initialCatalog = CatalogRepository.GetEntities().FirstOrDefault(cat => !cat.IsDefault);
            var note = new HmmNote
            {
                Author = _author,
                Catalog = initialCatalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            note.Catalog = catalog;

            // Act
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.True(NoteRepository.ProcessMessage.Success);
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);
        }

        [Fact]
        public void CanUpdateNoteCatalogToCatalogWithInvalidIdDefaultCatalogApplied()
        {
            // Arrange - none exists catalog
            var catalog = new NoteCatalog
            {
                Id = -1,
                Name = "Gas Log",
                Description = "Testing catalog"
            };

            var initialCatalog = CatalogRepository.GetEntities().FirstOrDefault(cat => !cat.IsDefault);
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = initialCatalog,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmldoc.InnerXml
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            note.Catalog = catalog;

            // Act
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.True(NoteRepository.ProcessMessage.Success);
            Assert.NotNull(savedRec);
            Assert.NotNull(savedRec.Catalog);
            Assert.Equal("DefaultNoteCatalog", savedRec.Catalog.Name);
        }

        [Fact]
        public void CannotUpdateNonExitsNote()
        {
            // Arrange - non exists id
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note2",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
            };
            NoteRepository.Add(note);

            // Act
            var orgId = note.Id;
            note.Id = 2;
            var savedRec = NoteRepository.Update(note);

            // Assert
            Assert.False(NoteRepository.ProcessMessage.Success);
            Assert.Null(savedRec);

            // Arrange - invalid id
            note.Id = 0;

            // Act
            savedRec = NoteRepository.Update(note);

            // Assert
            Assert.False(NoteRepository.ProcessMessage.Success);
            Assert.Null(savedRec);

            // do this to make clear up code pass
            note.Id = orgId;
        }

        [Fact]
        public void CanDeleteNote()
        {
            // Arrange
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            // Act
            var result = NoteRepository.Delete(note);

            // Assert
            Assert.True(NoteRepository.ProcessMessage.Success);
            Assert.True(result);
            Assert.False(NoteRepository.GetEntities().Any());
        }

        [Fact]
        public void CannotDeleteNonExistsNote()
        {
            // Arrange
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _author,
                Catalog = _catalog,
                Description = "testing note",
                Subject = "testing note is here",
                Content = xmldoc.InnerXml,
            };
            NoteRepository.Add(note);
            Assert.True(NoteRepository.ProcessMessage.Success);

            // change the note id to create a new note
            var orgId = note.Id;
            note.Id = 2;

            // Act
            var result = NoteRepository.Delete(note);

            // Assert
            Assert.False(NoteRepository.ProcessMessage.Success);
            Assert.False(result);

            // do this just to make clear up code pass
            note.Id = orgId;
        }

        private class UserTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null };

                // Arrange - none exists author
                yield return new object[]
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Amy",
                        LastName = "Wang",
                        AccountName = "jfang",
                        BirthDay = new DateTime(1977, 05, 21),
                        Password = "lucky1",
                        Salt = "passwordSalt",
                        IsActivated = true,
                        Description = "testing user"
                    }
                };

                // Arrange - author with invalid author id
                yield return new object[]
                {
                    new User
                    {
                        Id = Guid.Empty,
                        FirstName = "Amy",
                        LastName = "Wang",
                        AccountName = "jfang",
                        BirthDay = new DateTime(1977, 05, 21),
                        Password = "lucky1",
                        Salt = "passwordSalt",
                        IsActivated = true,
                        Description = "testing user"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class CatalogTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null };

                // Arrange - none exists author
                yield return new object[]
                {
                    new NoteCatalog
                    {
                        Id = 200,
                        Name = "Gas Log",
                        Schema = "Test Schema",
                        Render = new NoteRender
                        {
                            Name = "TestRender",
                            Namespace = "TestNameSpace",
                            IsDefault = true,
                            Description = "Description"
                        },
                        IsDefault = true,
                        Description = "Testing catalog"
                    }
                };

                // Arrange - author with invalid author id
                yield return new object[]
                {
                    new NoteCatalog
                    {
                        Id = 0,
                        Name = "Gas Log",
                        Schema = "Test Schema",
                        Render = new NoteRender
                        {
                            Name = "TestRender",
                            Namespace = "TestNameSpace",
                            IsDefault = true,
                            Description = "Description"
                        },
                        Description = "Testing catalog"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}