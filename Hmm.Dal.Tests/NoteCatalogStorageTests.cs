using DomainEntity.Misc;
using DomainEntity.User;
using System;
using Hmm.Utility.TestHelp;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteCatalogStorageTests : TestFixtureBase
    {
        private readonly NoteRender _render;
        private readonly User _user;

        public NoteCatalogStorageTests()
        {
            var render = new NoteRender
            {
                Name = "TestRender",
                Namespace = "TestNamespace",
                Description = "Description"
            };

            _render = RenderStorage.Add(render);

            var user = new User
            {
                FirstName = "Chaoyang",
                LastName = "Fang",
                AccountName = "fchy",
                BirthDay = new DateTime(1967, 3, 13),
                Description = "Testing User",
                IsActivated = true,
                Password = "1234",
                Salt = "5678"
            };
            _user = UserStorage.Add(user);
        }

        [Fact]
        public void CanAddNoteCatalogToDataSource()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                Description = "testing note",
            };

            // Act
            var savedRec = CatalogStorage.Add(catalog);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id > 0, "savedRec.Id > 0");
            Assert.True(catalog.Id == savedRec.Id, "cat.Id == savedRec.Id");
            Assert.True(CatalogStorage.ProcessMessage.Success);
        }

        [Fact]
        public void CanNotAddAlreadyExistedNoteCatalogToDataSource()
        {
            // Arrange
            CatalogStorage.Add(new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note",
            });

            var cat = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note",
            };

            // Act
            var savedRec = CatalogStorage.Add(cat);

            // Assert
            Assert.Null(savedRec);
            Assert.True(cat.Id <= 0, "cat.Id <=0");
            Assert.False(CatalogStorage.ProcessMessage.Success);
            Assert.Single(CatalogStorage.ProcessMessage.MessageList);
        }

        [Fact]
        public void CanDeleteNoteCatalogFromDataSource()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note"
            };

            CatalogStorage.Add(catalog);

            // Act
            var result = CatalogStorage.Delete(catalog);

            // Assert
            Assert.True(result);
            Assert.True(CatalogStorage.ProcessMessage.Success);
        }

        [Fact]
        public void CannotDeleteNonExistsCatalogFromDataSource()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note"
            };

            CatalogStorage.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Name = "GasLog2",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note"
            };

            // Act
            var result = CatalogStorage.Delete(catalog2);

            // Assert
            Assert.False(result);
            Assert.False(CatalogStorage.ProcessMessage.Success);
            Assert.Single(CatalogStorage.ProcessMessage.MessageList);
        }

        [Fact]
        public void CannotDeleteCatalogWithNoteAssociated()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note"
            };
            var savedCatalog = CatalogStorage.Add(catalog);

            var note = new HmmNote
            {
                Subject = "Testing subject",
                Content = "Testing content",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Author = _user,
                Catalog = savedCatalog
            };
            NoteStorage.Add(note);

            // Act
            var result = CatalogStorage.Delete(catalog);

            // Assert
            Assert.False(result, "Error: deleted catalog with note attached to it");
            Assert.False(CatalogStorage.ProcessMessage.Success);
            Assert.Single(CatalogStorage.ProcessMessage.MessageList);
        }

        [Fact]
        public void CanUpdateCatalog()
        {
            // Arrange - update name
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note"
            };

            CatalogStorage.Add(catalog);

            catalog.Name = "GasLog2";

            // Act
            var result = CatalogStorage.Update(catalog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", result.Name);

            // Arrange - update description
            catalog.Description = "new testing note";

            // Act
            result = CatalogStorage.Update(catalog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new testing note", result.Description);
        }

        [Fact]
        public void CannotUpdateCatalogForNonExistsCatalog()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note"
            };

            CatalogStorage.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Name = "GasLog2",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note"
            };

            // Act
            var result = CatalogStorage.Update(catalog2);

            // Assert
            Assert.Null(result);
            Assert.False(CatalogStorage.ProcessMessage.Success);
            Assert.Single(CatalogStorage.ProcessMessage.MessageList);
        }

        [Fact]
        public void CannotUpdateCatalogWithDuplicatedName()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Name = "GasLog",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = true,
                Description = "testing note"
            };
            CatalogStorage.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Name = "GasLog2",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note2"
            };
            CatalogStorage.Add(catalog2);

            catalog.Name = catalog2.Name;

            // Act
            var result = CatalogStorage.Update(catalog);

            // Assert
            Assert.Null(result);
            Assert.False(CatalogStorage.ProcessMessage.Success);
            Assert.Single(CatalogStorage.ProcessMessage.MessageList);
        }
    }
}