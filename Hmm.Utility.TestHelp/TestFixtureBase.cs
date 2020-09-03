using Hmm.Contract.Core;
using Hmm.Core.Manager;
using Hmm.Core.Manager.Validation;
using Hmm.Dal.Data;
using Hmm.Dal.DataRepository;
using Hmm.Dal.Queries;
using Hmm.DomainEntity.Enumerations;
using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Currency;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VehicleInfoManager.GasLogMan;

namespace Hmm.Utility.TestHelp
{
    public class TestFixtureBase : IDisposable
    {
        private List<Author> _users;
        private List<NoteRender> _renders;
        private List<NoteCatalog> _catalogs;
        private List<HmmNote> _notes;
        private IHmmDataContext _dbContext;
        private readonly bool _isUsingMock;

        protected TestFixtureBase()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var envSetting = config["TestEnvironment:UseMoc"];
            if (!bool.TryParse(envSetting, out _isUsingMock))
            {
                throw new InvalidOperationException($"Cannot get environment setting UseMoc {envSetting}");
            }

            if (_isUsingMock)
            {
                SetMockEnvironment();
            }
            else
            {
                var connectString = config["ConnectionStrings:DefaultConnection"];
                SetRealEnvironment(connectString);
            }
        }

        protected IGuidRepository<Author> AuthorRepository { get; private set; }

        protected IVersionRepository<HmmNote> NoteRepository { get; private set; }

        protected IHmmNoteManager NoteManager { get; private set; }

        protected IRepository<NoteCatalog> CatalogRepository { get; private set; }

        protected IRepository<NoteRender> RenderRepository { get; private set; }

        protected IEntityLookup LookupRepo { get; private set; }

        protected IDateTimeProvider DateProvider { get; private set; }

        protected void InsertSeedRecords(bool isSetupDiscount = false, bool isSetupAutomobile = false)
        {
            var authors = new List<Author>
            {
                new Author
                {
                    AccountName = "jfang",
                    IsActivated = true,
                    Description = "testing user"
                },
                new Author
                {
                    AccountName = "awang",
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
                    Name = "GasLog",
                    Schema = "GasLogSchema",
                    Render = renders[1],
                    Description = "Testing catalog"
                },
                new NoteCatalog
                {
                    Name = "Automobile",
                    Schema = "AutomobileSchema",
                    Render = renders[0],
                    Description = "Testing automobile note"
                },
                new NoteCatalog
                {
                    Name = "GasDiscount",
                    Schema = "GasDiscount",
                    Render = renders[0],
                    Description = "Testing discount note"
                }
            };

            var discounts = isSetupDiscount
                ? new List<GasDiscount>
                {
                    new GasDiscount
                    {
                        Program = "Costco membership",
                        Amount = new Money(0.6),
                        DiscountType = GasDiscountType.PreLiter,
                        Comment = "Test Discount",
                        IsActive = true,
                    },

                    new GasDiscount
                    {
                        Program = "Petro-Canada membership",
                        Amount = new Money(0.2),
                        DiscountType = GasDiscountType.PreLiter,
                        Comment = "Test Discount 2",
                        IsActive = true,
                    }
                }
                : new List<GasDiscount>();

            var cars = isSetupAutomobile
                ? new List<Automobile>
                {
                    new Automobile
                    {
                        Brand = "AutoBack",
                        Maker = "Subaru",
                        MeterReading = 100,
                        Year = "2018",
                        Pin = "1234",
                    }
                }
                : new List<Automobile>();

            SetupRecords(authors, renders, catalogs, discounts, cars);
        }

        protected void SetupRecords(
            IEnumerable<Author> users,
            IEnumerable<NoteRender> renders,
            IEnumerable<NoteCatalog> catalogs,
            IEnumerable<GasDiscount> discounts,
            IEnumerable<Automobile> cars)
        {
            Guard.Against<ArgumentNullException>(users == null, nameof(users));
            Guard.Against<ArgumentNullException>(renders == null, nameof(users));
            Guard.Against<ArgumentNullException>(catalogs == null, nameof(users));

            // ReSharper disable PossibleNullReferenceException
            foreach (var user in users)
            {
                AuthorRepository.Add(user);
            }

            foreach (var render in renders)
            {
                RenderRepository.Add(render);
            }

            foreach (var catalog in catalogs)
            {
                if (catalog.Render != null)
                {
                    var render = LookupRepo.GetEntities<NoteRender>()
                        .FirstOrDefault(r => r.Name == catalog.Render.Name);
                    if (render != null)
                    {
                        catalog.Render = render;
                    }
                    else
                    {
                        throw new InvalidDataException($"Cannot find render {catalog.Render.Name} from data source");
                    }
                }
                else
                {
                    var render = LookupRepo.GetEntities<NoteRender>().FirstOrDefault();
                    if (render != null)
                    {
                        catalog.Render = render;
                    }
                    else
                    {
                        throw new InvalidDataException("Cannot find default render from data source");
                    }
                }

                CatalogRepository.Add(catalog);
            }

            NoteManager = new HmmNoteManager(NoteRepository, new NoteValidator(NoteRepository));
            var discountMan = new DiscountManager(NoteManager, LookupRepo, DateProvider);
            foreach (var discount in discounts)
            {
                var user = LookupRepo.GetEntities<Author>().OrderBy(u => u.Id).FirstOrDefault();
                discountMan.Create(discount, user);
            }

            var autoMan = new AutomobileManager(NoteManager, LookupRepo, DateProvider);
            foreach (var car in cars)
            {
                var user = LookupRepo.GetEntities<Author>().OrderBy(u => u.Id).FirstOrDefault();
                autoMan.Create(car, user);
            }

            // ReSharper restore PossibleNullReferenceException
        }

        protected void NoTrackingEntities()
        {
            if (_dbContext is DbContext context)
            {
                context.NoTracking();
            }
        }

        public void Dispose()
        {
            if (_isUsingMock)
            {
                _users.Clear();
                _renders.Clear();
                _catalogs.Clear();
                _notes.Clear();
            }
            else
            {
                if (_dbContext is DbContext context)
                {
                    context.Reset();
                }

                var notes = LookupRepo.GetEntities<HmmNote>().ToList();
                foreach (var note in notes)
                {
                    NoteRepository.Delete(note);
                }

                var catalogs = LookupRepo.GetEntities<NoteCatalog>().ToList();
                foreach (var catalog in catalogs)
                {
                    CatalogRepository.Delete(catalog);
                }

                var renders = LookupRepo.GetEntities<NoteRender>().ToList();
                foreach (var render in renders)
                {
                    RenderRepository.Delete(render);
                }

                var users = LookupRepo.GetEntities<Author>().ToList();
                foreach (var user in users)
                {
                    AuthorRepository.Delete(user);
                }

                if (_dbContext is DbContext newContext)
                {
                    newContext.Reset();
                }
            }
        }

        private void SetMockEnvironment()
        {
            _users = new List<Author>();
            _renders = new List<NoteRender>();
            _catalogs = new List<NoteCatalog>();
            _notes = new List<HmmNote>();

            // set up for entity look up
            var lookupMoc = new Mock<IEntityLookup>();
            lookupMoc.Setup(lk => lk.GetEntity<Author>(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                var recFound = _users.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            lookupMoc.Setup(lk => lk.GetEntity<NoteRender>(It.IsAny<int>())).Returns((int id) =>
            {
                var recFound = _renders.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            lookupMoc.Setup(lk => lk.GetEntity<NoteCatalog>(It.IsAny<int>())).Returns((int id) =>
            {
                var recFound = _catalogs.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            lookupMoc.Setup(lk => lk.GetEntity<HmmNote>(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var rec = _notes.FirstOrDefault(n => n.Id == id);
                    return rec;
                });

            // set up unit of work
            // set up for user
            var dataContextMock = new Mock<IHmmDataContext>();
            dataContextMock.Setup(u => u.Authors.Add(It.IsAny<Author>())).Returns((Author user) =>
                {
                    user.Id = Guid.NewGuid();
                    _users.AddEntity(user);
                    return null;
                }
            );
            dataContextMock.Setup(u => u.Authors.Remove(It.IsAny<Author>())).Callback((Author user) =>
            {
                _users.Remove(user);
            });
            dataContextMock.Setup(u => u.Authors.Update(It.IsAny<Author>())).Callback((Author user) =>
            {
                var ordUser = _users.FirstOrDefault(c => c.Id == user.Id);
                if (ordUser != null)
                {
                    _users.Remove(ordUser);
                    _users.AddEntity(user);
                }
            });

            // set up for note render
            dataContextMock.Setup(u => u.Renders.Add(It.IsAny<NoteRender>())).Returns((NoteRender render) =>
                {
                    render.Id = _renders.GetNextId();
                    _renders.AddEntity(render);
                    return null;
                }
            );
            dataContextMock.Setup(u => u.Renders.Remove(It.IsAny<NoteRender>())).Callback((NoteRender render) =>
            {
                _renders.Remove(render);
            });
            dataContextMock.Setup(u => u.Renders.Update(It.IsAny<NoteRender>())).Callback((NoteRender render) =>
            {
                var orgRender = _renders.FirstOrDefault(c => c.Id == render.Id);
                if (orgRender != null)
                {
                    _renders.Remove(orgRender);
                    _renders.AddEntity(render);
                }
            });

            // set up for note catalog
            dataContextMock.Setup(u => u.Catalogs.Add(It.IsAny<NoteCatalog>())).Returns((NoteCatalog cat) =>
                {
                    cat.Id = _catalogs.GetNextId();
                    _catalogs.AddEntity(cat);
                    return null;
                }
            );
            dataContextMock.Setup(u => u.Catalogs.Remove(It.IsAny<NoteCatalog>())).Callback((NoteCatalog cat) =>
            {
                _catalogs.Remove(cat);
            });

            dataContextMock.Setup(u => u.Catalogs.Update(It.IsAny<NoteCatalog>())).Callback((NoteCatalog cat) =>
            {
                var orgCat = _catalogs.FirstOrDefault(c => c.Id == cat.Id);
                if (orgCat != null)
                {
                    _catalogs.Remove(orgCat);
                    _catalogs.AddEntity(cat);
                }
            });

            // set up for note
            dataContextMock.Setup(u => u.Notes.Add(It.IsAny<HmmNote>()))
                .Returns((HmmNote note) =>
                {
                    note.Id = _notes.GetNextId();
                    _notes.AddEntity(note);
                    return null;
                });
            dataContextMock.Setup(u => u.Notes.Remove(It.IsAny<HmmNote>()))
                .Callback((HmmNote note) =>
                {
                    var rec = _notes.FirstOrDefault(n => n.Id == note.Id);
                    if (rec != null)
                    {
                        _notes.Remove(rec);
                    }
                });
            dataContextMock.Setup(u => u.Notes.Update(It.IsAny<HmmNote>()))
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

            // setup date time provider
            var timeProviderMock = new Mock<IDateTimeProvider>();

            // setup user storage
            AuthorRepository = new AuthorEfRepository(dataContextMock.Object, lookupMoc.Object);

            // set up render storage
            RenderRepository = new NoteRenderEfRepository(dataContextMock.Object, lookupMoc.Object, timeProviderMock.Object);

            // set up for catalog storage
            CatalogRepository = new NoteCatalogEfRepository(dataContextMock.Object, lookupMoc.Object, timeProviderMock.Object);

            // set up for note storage
            NoteRepository = new NoteEfRepository(dataContextMock.Object, lookupMoc.Object, timeProviderMock.Object);

            DateProvider = new DateTimeAdapter();
        }

        private void SetRealEnvironment(string connectString)
        {
            var optBuilder = new DbContextOptionsBuilder()
                .UseSqlServer(connectString);
            _dbContext = new HmmDataContext(optBuilder.Options);
            LookupRepo = new EfEntityLookup(_dbContext);
            var dateProvider = new DateTimeAdapter();
            AuthorRepository = new AuthorEfRepository(_dbContext, LookupRepo);
            NoteRepository = new NoteEfRepository(_dbContext, LookupRepo, dateProvider);
            RenderRepository = new NoteRenderEfRepository(_dbContext, LookupRepo, dateProvider);
            CatalogRepository = new NoteCatalogEfRepository(_dbContext, LookupRepo, dateProvider);
            DateProvider = new DateTimeAdapter();
            //NoteManager = new HmmNoteEfRepository(NoteRepository, new NoteValidator(NoteRepository));
        }
    }
}