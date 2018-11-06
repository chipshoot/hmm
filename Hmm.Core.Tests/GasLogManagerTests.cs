using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Core.Manager;
using Hmm.Core.Manager.GasLogMan;
using Hmm.Dal.Storage;
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
        private readonly List<HmmNote> _notes;
        private readonly List<User> _authors;
        private readonly List<NoteCatalog> _cats;
        private readonly GasLogManager _manager;

        public GasLogManagerTests()
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
            var renders = new List<NoteRender>
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
                var rec = renders.FirstOrDefault(n => n.Id == id);
                return rec;
            });

            // set up date time provider
            var currentDate = DateTime.Now;
            var timeProviderMock = new Mock<IDateTimeProvider>();
            timeProviderMock.Setup(t => t.UtcNow).Returns(() => currentDate);

            var noteStorage = new NoteStorage(uowMock.Object, lookupMock.Object, timeProviderMock.Object);
            var noteManager = new HmmNoteManager(noteStorage, lookupMock.Object);
            _manager = new GasLogManager(noteManager, lookupMock.Object);
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
            var gasLog = new GasLog
            {
                Author = user,
                Catalog = cat,
                GasStation = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300),
                CreateDate = DateTime.UtcNow,
                Discounts = new List<GasDiscountInfo>
                {
                    new GasDiscountInfo
                    {
                        Amount = new Money(0.8),
                        Program = "Patrol Canada RBC connection"
                    }
                }
            };

            // Act
            var newGas = _manager.CreateLog(gasLog);

            // Assert
            Assert.Equal(1, newGas.Id);
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