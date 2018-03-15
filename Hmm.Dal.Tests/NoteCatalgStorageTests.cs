using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Querys;
using Hmm.Dal.Storages;
using Hmm.Dal.Validation;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
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
        private readonly List<HmmNote> _notes;
        private readonly NoteCatalogStorage _catalogStorage;

        public NoteCatalgStorageTests()
        {
            _catalogs = new List<NoteCatalog>();
            _notes = new List<HmmNote>();

            // set up look up repository
            var lookupMoc = new Mock<IEntityLookup>();
            lookupMoc.Setup(lk => lk.GetEntity<NoteCatalog>(It.IsAny<int>())).Returns((int id) =>
            {
                var recFound = _catalogs.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            // set up unit of work
            var uowmock = new Mock<IUnitOfWork>();
            uowmock.Setup(u => u.Add(It.IsAny<NoteCatalog>())).Returns((NoteCatalog cat) =>
                {
                    cat.Id = _catalogs.GetNextId();
                    _catalogs.AddEntity(cat);
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
                    _catalogs.AddEntity(cat);
                }
            });

            // set up query handler
            var queryMock = new Mock<IQueryHandler<NoteCatalogQueryByName, NoteCatalog>>();
            queryMock.Setup(q => q.Execute(It.IsAny<NoteCatalogQueryByName>())).Returns((NoteCatalogQueryByName q) =>
            {
                var catfound = _catalogs.FirstOrDefault(c => c.Name == q.CatalogName);
                return catfound;
            });

            // set up note query handler
            var noteQueryMock = new Mock<IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>>>();
            noteQueryMock.Setup(q => q.Execute(It.IsAny<NoteQueryByCatalog>())).Returns((NoteQueryByCatalog query) =>
            {
                IEnumerable<HmmNote> notes = new List<HmmNote>();
                if (query.Catalog.Id > 0)
                {
                    notes = _notes.Where(n => n.Catalog.Id == query.Catalog.Id).Select(n => n).AsEnumerable();
                }

                return notes;
            });

            // set up note catalog validator
            var validator = new NoteCatalogValidator(lookupMoc.Object, queryMock.Object);

            // set up date time provider
            var timeProviderMock = new Mock<IDateTimeProvider>();

            // set up catalog repository
            _catalogStorage = new NoteCatalogStorage(uowmock.Object, validator, lookupMoc.Object, noteQueryMock.Object, timeProviderMock.Object);
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
            Assert.Equal(1, _catalogs.Count);
        }

        [Fact]
        public void CanNotAddAlreadyExistedNoteCatalogToDataSource()
        {
            // Arrange
            _catalogs.AddEntity(new NoteCatalog
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
            Assert.Equal(1, _catalogs.Count);
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

            _catalogs.AddEntity(catalog);
            Assert.Equal(1, _catalogs.Count);

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

            _catalogs.AddEntity(catalog);

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
            // Arrange
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };
            _catalogs.AddEntity(catalog);

            var note = new HmmNote
            {
                Id = 1,
                Subject = string.Empty,
                Content = string.Empty,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Author = new User(),
                Catalog = _catalogs[0],
            };
            _notes.AddEntity(note);
            Assert.Equal(1, _catalogs.Count);
            Assert.Equal(1, _notes.Count);

            // Act
            var result = _catalogStorage.Delete(catalog);

            // Assert
            Assert.False(result, "Error: deleted catalog with note attached to it");
            Assert.Equal(1, _catalogs.Count);
            Assert.True(_catalogStorage.Validator.ValidationErrors.Count > 0);
        }

        [Fact]
        public void CanUpdateCatalog()
        {
            // Arrange - update name
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };

            _catalogs.AddEntity(catalog);

            catalog.Name = "GasLog2";

            // Act
            var result = _catalogStorage.Update(catalog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", _catalogs[0].Name);

            // Arrange - update description
            catalog.Description = "new testing note";

            // Act
            result = _catalogStorage.Update(catalog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new testing note", _catalogs[0].Description);
        }

        [Fact]
        public void CannotUpdateCatalogForNonExistsCatalog()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };

            _catalogs.AddEntity(catalog);

            var catalog2 = new NoteCatalog
            {
                Id = 2,
                Name = "GasLog2",
                Description = "testing note"
            };

            // Act
            var result = _catalogStorage.Update(catalog2);

            // Assert
            Assert.Null(result);
            Assert.Equal(1, _catalogs.Count);
            Assert.Equal("GasLog", _catalogs[0].Name);
        }

        [Fact]
        public void CannotUpdateCatalogWithDuplicatedName()
        {
            // Arrange
            var catalog = new NoteCatalog
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };
            _catalogs.AddEntity(catalog);

            var catalog2 = new NoteCatalog
            {
                Id = 2,
                Name = "GasLog2",
                Description = "testing note2"
            };
            _catalogs.AddEntity(catalog2);

            catalog.Name = catalog2.Name;

            // Act
            var result = _catalogStorage.Update(catalog);

            // Assert
            Assert.Null(result);
            Assert.Equal(2, _catalogs.Count);
            Assert.Equal("GasLog", _catalogs[0].Name);
            Assert.Equal("GasLog2", _catalogs[1].Name);
        }
    }
}