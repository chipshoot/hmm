using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Core.Manager;
using Hmm.Dal.Storages;
using Hmm.Dal.Validation;
using Hmm.Utility.Currency;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.Misc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hmm.Core.Tests
{
    public class GasLogManagerTests : IDisposable
    {
        private readonly List<GasLog> _notes;
        private readonly List<User> _authors;
        private readonly List<NoteCatalog> _cats;
        private readonly List<NoteRender> _renders;
        private readonly DateTime _currentDate;
        private readonly GasLogManager _manager;

        public GasLogManagerTests()
        {
            _notes = new List<GasLog>();
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
            uowMock.Setup(u => u.Add(It.IsAny<GasLog>())).Returns((GasLog note) =>
            {
                note.Id = _notes.GetNextId();
                _notes.AddEntity(note);
                var savedRec = _notes.FirstOrDefault(n => n.Id == note.Id);
                return savedRec;
            });
            uowMock.Setup(u => u.Delete(It.IsAny<GasLog>())).Callback((GasLog note) =>
            {
                var rec = _notes.FirstOrDefault(n => n.Id == note.Id);
                if (rec != null)
                {
                    _notes.Remove(rec);
                }
            });
            uowMock.Setup(u => u.Update(It.IsAny<GasLog>())).Callback((GasLog note) =>
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
            lookupMock.Setup(lk => lk.GetEntity<GasLog>(It.IsAny<int>())).Returns((int id) =>
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

            var noteStorage = new NoteStorage<GasLog>(uowMock.Object, validator, lookupMock.Object, timeProviderMock.Object);
            _manager = new GasLogManager(noteStorage);
        }

        public void Dispose()
        {
            _notes.Clear();
        }

        [Fact]
        public void CanAddGasLog()
        {
            // Arrange
            var user = _authors[0];
            var cat = _cats[1];
            var gaslog = new GasLog
            {
                Author = user,
                Catalog = cat,
                GasStation = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300)
            };

            // Act
            var newgas = _manager.Create(gaslog);

            // Assert
            Assert.Equal(1, newgas.Id);
        }

        [Fact]
        public void CanUpdateGasLog()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanDeleteCasLog()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanFindGasLog()
        {
            throw new NotImplementedException();
        }
    }
}