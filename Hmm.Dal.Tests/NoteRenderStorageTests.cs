using DomainEntity.Misc;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Hmm.Dal.Validation;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class NoteRenderStorageTests : IDisposable
    {
        private readonly List<NoteRender> _renders;
        private readonly NoteRenderStorage _renderStorage;

        public NoteRenderStorageTests()
        {
            _renders = new List<NoteRender>();

            var lookupMoc = new Mock<IEntityLookup>();
            lookupMoc.Setup(lk => lk.GetEntity<NoteRender>(It.IsAny<int>())).Returns((int id) =>
            {
                var recFound = _renders.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            var uowmock = new Mock<IUnitOfWork>();
            uowmock.Setup(u => u.Add(It.IsAny<NoteRender>())).Returns((NoteRender render) =>
                {
                    render.Id = _renders.Count + 1;
                    _renders.AddEntity(render);
                    return render;
                }
            );
            uowmock.Setup(u => u.Delete(It.IsAny<NoteRender>())).Callback((NoteRender render) =>
            {
                _renders.Remove(render);
            });
            uowmock.Setup(u => u.Update(It.IsAny<NoteRender>())).Callback((NoteRender render) =>
            {
                var orgRender = _renders.FirstOrDefault(c => c.Id == render.Id);
                if (orgRender != null)
                {
                    _renders.Remove(orgRender);
                    _renders.AddEntity(render);
                }
            });

            var queryMock = new Mock<IQueryHandler<NoteRenderQueryByName, NoteRender>>();
            queryMock.Setup(q => q.Execute(It.IsAny<NoteRenderQueryByName>())).Returns((NoteRenderQueryByName q) =>
            {
                var catfound = _renders.FirstOrDefault(c => c.Name == q.RenderName);
                return catfound;
            });

            var validator = new NoteRenderValidator(lookupMoc.Object, queryMock.Object);
            _renderStorage = new NoteRenderStorage(uowmock.Object, validator, lookupMoc.Object);
        }

        public void Dispose()
        {
            _renders.Clear();
        }

        [Fact]
        public void CanAddNoteRenderToDataSource()
        {
            // Arrange
            var render = new NoteRender
            {
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note",
            };

            // Act
            var savedRec = _renderStorage.Add(render);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(1, render.Id);
            Assert.Equal(1, _renders.Count);
        }

        [Fact]
        public void CanNotAddAlreadyExistedNoteRenderToDataSource()
        {
            // Arrange
            _renders.AddEntity(new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note",
            });

            var render = new NoteRender
            {
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note",
            };

            // Act
            var savedRec = _renderStorage.Add(render);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, render.Id);
            Assert.Equal(1, _renders.Count);
        }

        [Fact]
        public void CanDeleteNoteRenderFromDataSource()
        {
            // Arrange
            var render = new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note"
            };

            _renders.AddEntity(render);
            Assert.Equal(1, _renders.Count);

            // Act
            var result = _renderStorage.Delete(render);

            // Assert
            Assert.True(result);
            Assert.Equal(0, _renders.Count);
        }

        [Fact]
        public void CannotDeleteNonExistsNoteRenderFromDataSource()
        {
            // Arrange
            var render = new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note"
            };

            _renders.AddEntity(render);

            var render2 = new NoteRender
            {
                Id = 2,
                Name = "GasLog2",
                Namespace = "Note.GasLog2",
                Description = "testing note"
            };

            // Act
            var result = _renderStorage.Delete(render2);

            // Assert
            Assert.False(result);
            Assert.Equal(1, _renders.Count);
        }

        [Fact]
        public void CannotDeleteNoteRenderWithNoteAssociated()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanUpdateNoteRender()
        {
            // Arrange - update name
            var render = new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note"
            };

            _renders.AddEntity(render);

            render.Name = "GasLog2";

            // Act
            var result = _renderStorage.Update(render);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", _renders[0].Name);

            // Arrange - update description
            render.Description = "new testing note";

            // Act
            result = _renderStorage.Update(render);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new testing note", _renders[0].Description);
        }

        [Fact]
        public void CannotUpdateNoteRenderForNonExistsCatalog()
        {
            // Arrange
            var render = new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Namespace = "Note.GasLog",
                Description = "testing note"
            };

            _renders.AddEntity(render);

            var render2 = new NoteRender
            {
                Id = 2,
                Name = "GasLog2",
                Namespace = "Note.GasLog",
                Description = "testing note"
            };

            // Act
            var result = _renderStorage.Update(render2);

            // Assert
            Assert.Null(result);
            Assert.Equal(1, _renders.Count);
            Assert.Equal("GasLog", _renders[0].Name);
        }

        [Fact]
        public void CannotUpdateNoteRenderWithDuplicatedName()
        {
            // Arrange
            var render = new NoteRender
            {
                Id = 1,
                Name = "GasLog",
                Description = "testing note"
            };
            _renders.AddEntity(render);

            var render2 = new NoteRender
            {
                Id = 2,
                Name = "GasLog2",
                Description = "testing note2"
            };
            _renders.AddEntity(render2);

            render.Name = render2.Name;

            // Act
            var result = _renderStorage.Update(render);

            // Assert
            Assert.Null(result);
            Assert.Equal(2, _renders.Count);
            Assert.Equal("GasLog", _renders[0].Name);
            Assert.Equal("GasLog2", _renders[1].Name);
        }
    }
}