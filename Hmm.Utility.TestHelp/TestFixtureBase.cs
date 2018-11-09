using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Data;
using Hmm.Dal.Querys;
using Hmm.Dal.Storage;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hmm.Utility.TestHelp
{
    public class TestFixtureBase : IDisposable
    {
        private List<User> _users;
        private List<NoteRender> _renders;
        private List<NoteCatalog> _catalogs;
        private List<HmmNote> _notes;
        private IHmmDataContext _dbContext;
        private IEntityLookup _lookupRepo;
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

        protected UserStorage UserStorage { get; private set; }

        protected NoteStorage NoteStorage { get; private set; }

        protected NoteCatalogStorage CatalogStorage { get; private set; }

        protected NoteRenderStorage RenderStorage { get; private set; }

        protected void SetupRecords(
            IEnumerable<User> users,
            IEnumerable<NoteRender> renders,
            IEnumerable<NoteCatalog> catalogs)
        {
            Guard.Against<ArgumentNullException>(users == null, nameof(users));
            Guard.Against<ArgumentNullException>(renders == null, nameof(users));
            Guard.Against<ArgumentNullException>(catalogs == null, nameof(users));

            // ReSharper disable PossibleNullReferenceException
            foreach (var user in users)
            {
                UserStorage.Add(user);
            }

            foreach (var render in renders)
            {
                RenderStorage.Add(render);
            }

            foreach (var catalog in catalogs)
            {
                if (catalog.Render != null)
                {
                    var render = _lookupRepo.GetEntities<NoteRender>()
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
                    var render = _lookupRepo.GetEntities<NoteRender>().FirstOrDefault();
                    if (render != null)
                    {
                        catalog.Render = render;
                    }
                    else
                    {
                        throw new InvalidDataException($"Cannot find default render from data source");
                    }
                }

                CatalogStorage.Add(catalog);
            }

            // ReSharper restore PossibleNullReferenceException
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

                var notes = _lookupRepo.GetEntities<HmmNote>();
                foreach (var note in notes)
                {
                    NoteStorage.Delete(note);
                }

                var catalogs = _lookupRepo.GetEntities<NoteCatalog>();
                foreach (var catalog in catalogs)
                {
                    CatalogStorage.Delete(catalog);
                }

                var renders = _lookupRepo.GetEntities<NoteRender>();
                foreach (var render in renders)
                {
                    RenderStorage.Delete(render);
                }

                var users = _lookupRepo.GetEntities<User>();
                foreach (var user in users)
                {
                    UserStorage.Delete(user);
                }
            }
        }

        private void SetMockEnvironment()
        {
            _users = new List<User>();
            _renders = new List<NoteRender>();
            _catalogs = new List<NoteCatalog>();
            _notes = new List<HmmNote>();

            // set up for entity look up
            var lookupMoc = new Mock<IEntityLookup>();
            lookupMoc.Setup(lk => lk.GetEntity<User>(It.IsAny<int>())).Returns((int id) =>
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
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.Add(It.IsAny<User>())).Returns((User user) =>
                {
                    user.Id = _users.GetNextId();
                    _users.AddEntity(user);
                    return user;
                }
            );
            uowMock.Setup(u => u.Delete(It.IsAny<User>())).Callback((User user) =>
            {
                _users.Remove(user);
            });
            uowMock.Setup(u => u.Update(It.IsAny<User>())).Callback((User user) =>
            {
                var ordUser = _users.FirstOrDefault(c => c.Id == user.Id);
                if (ordUser != null)
                {
                    _users.Remove(ordUser);
                    _users.AddEntity(user);
                }
            });

            // set up for note render
            uowMock.Setup(u => u.Add(It.IsAny<NoteRender>())).Returns((NoteRender render) =>
                {
                    render.Id = _renders.GetNextId();
                    _renders.AddEntity(render);
                    return render;
                }
            );
            uowMock.Setup(u => u.Delete(It.IsAny<NoteRender>())).Callback((NoteRender render) =>
            {
                _renders.Remove(render);
            });
            uowMock.Setup(u => u.Update(It.IsAny<NoteRender>())).Callback((NoteRender render) =>
            {
                var orgRender = _renders.FirstOrDefault(c => c.Id == render.Id);
                if (orgRender != null)
                {
                    _renders.Remove(orgRender);
                    _renders.AddEntity(render);
                }
            });

            // set up for note catalog
            uowMock.Setup(u => u.Add(It.IsAny<NoteCatalog>())).Returns((NoteCatalog cat) =>
                {
                    cat.Id = _catalogs.GetNextId();
                    _catalogs.AddEntity(cat);
                    return cat;
                }
            );
            uowMock.Setup(u => u.Delete(It.IsAny<NoteCatalog>())).Callback((NoteCatalog cat) =>
            {
                _catalogs.Remove(cat);
            });

            uowMock.Setup(u => u.Update(It.IsAny<NoteCatalog>())).Callback((NoteCatalog cat) =>
            {
                var orgCat = _catalogs.FirstOrDefault(c => c.Id == cat.Id);
                if (orgCat != null)
                {
                    _catalogs.Remove(orgCat);
                    _catalogs.AddEntity(cat);
                }
            });

            // set up for note
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

            // setup date time provider
            var timeProviderMock = new Mock<IDateTimeProvider>();

            // setup user storage
            UserStorage = new UserStorage(uowMock.Object, lookupMoc.Object, timeProviderMock.Object);

            // set up render storage
            RenderStorage = new NoteRenderStorage(uowMock.Object, lookupMoc.Object, timeProviderMock.Object);

            // set up for catalog storage
            CatalogStorage = new NoteCatalogStorage(uowMock.Object, lookupMoc.Object, timeProviderMock.Object);

            // set up for note storage
            NoteStorage = new NoteStorage(uowMock.Object, lookupMoc.Object, timeProviderMock.Object);
        }

        private void SetRealEnvironment(string connectString)
        {
            var optBuilder = new DbContextOptionsBuilder()
                .UseSqlServer(connectString);
            _dbContext = new HmmDataContext(optBuilder.Options);
            var uow = new EfUnitOfWork(_dbContext);
            _lookupRepo = new EfEntityLookup(_dbContext);
            var dateProvider = new DateTimeAdapter();
            UserStorage = new UserStorage(uow, _lookupRepo, dateProvider);
            NoteStorage = new NoteStorage(uow, _lookupRepo, dateProvider);
            RenderStorage = new NoteRenderStorage(uow, _lookupRepo, dateProvider);
            CatalogStorage = new NoteCatalogStorage(uow, _lookupRepo, dateProvider);
        }
    }
}