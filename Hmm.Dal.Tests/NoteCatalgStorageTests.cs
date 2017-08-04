using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteCatalgStorageTests : IDisposable
    {
        private readonly List<NoteCatalog> _catalogs;
        private readonly NoteCatalogStorage _catalogStorage;

        public NoteCatalgStorageTests()
        {
            _catalogs = new List<NoteCatalog>();

            var uowmock = new Mock<IUnitOfWork>();
            uowmock.Setup(u => u.Add(It.IsAny<NoteCatalog>())).Returns((NoteCatalog cat) =>
                {
                    cat.Id = _catalogs.Count + 1;
                    _catalogs.Add(cat);
                    return cat;
                }
            );
            uowmock.Setup(u => u.Delete(It.IsAny<NoteCatalog>())).Callback((NoteCatalog cat) =>
            {
                _catalogs.Remove(cat);
            });
            uowmock.Setup(u => u.Update(It.IsAny<NoteCatalog>())).Callback((NoteCatalog cat) =>
            {
                var orgCat = _catalogs.FirstOrDefault(c => c.Id == cat.Id);
                if (orgCat != null)
                {
                    _catalogs.Remove(orgCat);
                    _catalogs.Add(cat);
                }
            });

            var queryMock = new Mock<IQueryHandler<NoteCatalogQueryByName, NoteCatalog>>();
            queryMock.Setup(q => q.Execute(It.IsAny<NoteCatalogQueryByName>())).Returns((NoteCatalogQueryByName q) =>
            {
                var catfound = _catalogs.FirstOrDefault(c => c.Name == q.CatalogName);
                return catfound;
            });
            _catalogStorage = new NoteCatalogStorage(uowmock.Object, queryMock.Object);
        }

        public void Dispose()
        {
            _catalogs.Clear();
        }

        [Fact]
        public void CanAddNoteCatalogToDataSource()
        {
            // Arrange
            var cat = new NoteCatalog
            {
                Name = "GasLog",
                Description = "testing note",
            };

            // Act
            var savedRec = _catalogStorage.Add(cat);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(1, cat.Id);
        }

        [Fact]
        public void CanNotAddAlreadyExistedNoteCatalogToDataSource()
        {
            // Arrange
            _catalogs.Add(new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note",
            });

            var cat = new NoteCatalog
            {
                Name = "GasLog",
                Description = "testing note",
            };

            // Act
            var savedRec = _catalogStorage.Add(cat);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, cat.Id);
        }

        [Fact]
        public void CanDeleteNoteCatalogFromDataSource()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };

            _catalogs.Add(catalog);

            // Act
            var result = _catalogStorage.Delete(catalog);

            // Assert
            Assert.True(result);
            Assert.Equal(0, _catalogs.Count);
        }

        [Fact]
        public void CannotDeleteNonExistsCatalogFromDataSource()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };

            _catalogs.Add(catalog);

            var catalog2 = new NoteCatalog
            {
                Id = 2,
                Name = "GasLog2",
                Description = "testing note"
            };

            // Act
            var result = _catalogStorage.Delete(catalog2);

            // Assert
            Assert.False(result);
            Assert.Equal(1, _catalogs.Count);
        }

        [Fact]
        public void CannotDeleteCatalogWithNoteAssociated()
        {

        }

        [Fact]
        public void CanUpdateCatalog()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };

            _catalogs.Add(catalog);

            catalog.Name = "GasLog2";


            // Act
            var result = _catalogStorage.Update(catalog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", _catalogs[0].Name);
        }

        [Fact]
        public void CannotUpdateCatalogForNonExistsCatalog()
        {
        }

        [Fact]
        public void CannotUpdateCatalogWithInvalidName()
        {
        }
    }
}