using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.Utility.TestHelp;
using System;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteCatalogRepositoryTests : TestFixtureBase
    {
        private readonly NoteRender _render;
        private readonly User _user;

        public NoteCatalogRepositoryTests()
        {
            var render = new NoteRender
            {
                Name = "TestRender",
                Namespace = "TestNamespace",
                Description = "Description"
            };

            _render = RenderRepository.Add(render);

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
            _user = UserRepository.Add(user);
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
            var savedRec = CatalogRepository.Add(catalog);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id > 0, "savedRec.Id > 0");
            Assert.True(catalog.Id == savedRec.Id, "cat.Id == savedRec.Id");
            Assert.True(CatalogRepository.ProcessMessage.Success);
        }

        [Fact]
        public void CanNotAddAlreadyExistedNoteCatalogToDataSource()
        {
            // Arrange
            CatalogRepository.Add(new NoteCatalog
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
            var savedRec = CatalogRepository.Add(cat);

            // Assert
            Assert.Null(savedRec);
            Assert.True(cat.Id <= 0, "cat.Id <=0");
            Assert.False(CatalogRepository.ProcessMessage.Success);
            Assert.Single(CatalogRepository.ProcessMessage.MessageList);
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

            CatalogRepository.Add(catalog);

            // Act
            var result = CatalogRepository.Delete(catalog);

            // Assert
            Assert.True(result);
            Assert.True(CatalogRepository.ProcessMessage.Success);
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

            CatalogRepository.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Name = "GasLog2",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note"
            };

            // Act
            var result = CatalogRepository.Delete(catalog2);

            // Assert
            Assert.False(result);
            Assert.False(CatalogRepository.ProcessMessage.Success);
            Assert.Single(CatalogRepository.ProcessMessage.MessageList);
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
            var savedCatalog = CatalogRepository.Add(catalog);

            var note = new HmmNote
            {
                Subject = "Testing subject",
                Content = "Testing content",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Author = _user,
                Catalog = savedCatalog
            };
            NoteRepository.Add(note);

            // Act
            var result = CatalogRepository.Delete(catalog);

            // Assert
            Assert.False(result, "Error: deleted catalog with note attached to it");
            Assert.False(CatalogRepository.ProcessMessage.Success);
            Assert.Single(CatalogRepository.ProcessMessage.MessageList);
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

            CatalogRepository.Add(catalog);

            catalog.Name = "GasLog2";

            // Act
            var result = CatalogRepository.Update(catalog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", result.Name);

            // Arrange - update description
            catalog.Description = "new testing note";

            // Act
            result = CatalogRepository.Update(catalog);

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

            CatalogRepository.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Name = "GasLog2",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note"
            };

            // Act
            var result = CatalogRepository.Update(catalog2);

            // Assert
            Assert.Null(result);
            Assert.False(CatalogRepository.ProcessMessage.Success);
            Assert.Single(CatalogRepository.ProcessMessage.MessageList);
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
            CatalogRepository.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Name = "GasLog2",
                Render = _render,
                Schema = "TestSchema",
                IsDefault = false,
                Description = "testing note2"
            };
            CatalogRepository.Add(catalog2);

            catalog.Name = catalog2.Name;

            // Act
            var result = CatalogRepository.Update(catalog);

            // Assert
            Assert.Null(result);
            Assert.False(CatalogRepository.ProcessMessage.Success);
            Assert.Single(CatalogRepository.ProcessMessage.MessageList);
        }
    }
}