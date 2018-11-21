//using DomainEntity.Misc;
//using Hmm.Contract.Core;
//using Hmm.Core.Manager;
//using Hmm.Utility.TestHelp;
//using System;
//using System.Linq;
//using System.Xml;
//using Xunit;

//namespace Hmm.Core.Tests
//{
//    public class HmmNoteManagerTests : TestFixtureBase
//    {
//        private readonly IHmmNoteManager<HmmNote> _manager;

//        public HmmNoteManagerTests()
//        {
//            _manager = new HmmNoteManager(NoteStorage, LookupRepo);
//        }

//        [Fact]
//        public void CanAddNoteToDataSource()
//        {
//            // Arrange
//            var user = UserStorage.GetEntities().FirstOrDefault();
//            var cat = CatalogStorage.GetEntities().FirstOrDefault();
//            var note = new HmmNote
//            {
//                Author = user,
//                Catalog = cat,
//                Subject = "Testing note",
//                Content = "Test content"
//            };
//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml(
//                "<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Test content</content></note>");

//            // Act
//            var newNote = _manager.Create(note);

//            // Assert
//            Assert.NotNull(newNote);
//            Assert.Equal(1, newNote.Id);
//            Assert.Equal("Testing note", newNote.Subject);
//            Assert.Equal(xmldoc.InnerXml, newNote.Content);
//            Assert.Equal(_currentDate, newNote.CreateDate);
//            Assert.Equal(_currentDate, newNote.LastModifiedDate);

//            Assert.Single(_notes);
//            Assert.Equal(1, _notes[0].Id);
//            Assert.Equal("Testing note", _notes[0].Subject);
//            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
//            Assert.Equal(_currentDate, _notes[0].CreateDate);
//            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
//        }

//        [Fact]
//        public void CanGetRightXmlWithNullNoteContentToDataSource()
//        {
//            // Arrange - note with null content
//            var user = _authors[0];
//            var cat = _cats[0];
//            var note = new HmmNote
//            {
//                Author = user,
//                Catalog = cat,
//                Subject = "Testing note",
//                Content = null
//            };
//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content></content></note>");

//            // Act
//            var newNote = _manager.Create(note);

//            // Assert
//            Assert.NotNull(newNote);

//            Assert.Equal(xmldoc.InnerXml, newNote.Content);

//            Assert.Single(_notes);
//            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
//        }

//        [Fact]
//        public void CanGetRightXmlWithNoteContentContainsInvalidCharToDataSource()
//        {
//            // Arrange - note with null content
//            var user = _authors[0];
//            var cat = _cats[0];
//            var note = new HmmNote
//            {
//                Author = user,
//                Catalog = cat,
//                Subject = "Testing note",
//                Content = "Testing content with < and >"
//            };
//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Testing content with &lt; and &gt;</content></note>");

//            // Act
//            var newNote = _manager.Create(note);

//            // Assert
//            Assert.NotNull(newNote);

//            Assert.Equal(xmldoc.InnerXml, newNote.Content);

//            Assert.Single(_notes);
//            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
//        }

//        [Fact]
//        public void CanUpdateNote()
//        {
//            // Arrange - note with null content
//            var user = _authors[0];
//            var cat = _cats[0];
//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>Testing content with &lt; and &gt;</content></note>");
//            var note = new HmmNote
//            {
//                Id = 1,
//                Author = user,
//                Catalog = cat,
//                Subject = "Testing note",
//                Content = xmldoc.InnerXml,
//                CreateDate = _currentDate,
//                LastModifiedDate = _currentDate
//            };
//            _notes.AddEntity(note);
//            Assert.Single(_notes);

//            // Act
//            note.Subject = "new note subject";
//            note.Content = "This is new note content";
//            var oldDate = _currentDate;
//            _currentDate = DateTime.Now.AddDays(1);
//            var newNote = _manager.Update(note);

//            // Assert
//            Assert.NotNull(newNote);
//            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"http://schema.hmm.com/2017\"><content>This is new note content</content></note>");
//            Assert.Equal(xmldoc.InnerXml, newNote.Content);
//            Assert.Equal("new note subject", newNote.Subject);
//            Assert.Equal(oldDate, newNote.CreateDate);
//            Assert.Equal(_currentDate, newNote.LastModifiedDate);

//            Assert.Single(_notes);
//            Assert.Equal("new note subject", _notes[0].Subject);
//            Assert.Equal(xmldoc.InnerXml, _notes[0].Content);
//            Assert.Equal(oldDate, _notes[0].CreateDate);
//            Assert.Equal(_currentDate, _notes[0].LastModifiedDate);
//        }

//        [Fact]
//        public void CannotUpdateNoteAuthor()
//        {
//            // todo: add checking to stop user change author of the note
//            //// Arrange
//            //var xmldoc = new XmlDocument();
//            //xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
//            //var note = new HmmNote
//            //{
//            //    Author = _author,
//            //    Catalog = _catalog,
//            //    Description = "testing note",
//            //    CreateDate = DateTime.Now,
//            //    LastModifiedDate = DateTime.Now,
//            //    Subject = "testing note is here",
//            //    Content = xmldoc.InnerXml
//            //};
//            //NoteStorage.Add(note);
//            //Assert.True(NoteStorage.ProcessMessage.Success);
//            //var savedRec = NoteStorage.GetEntities().FirstOrDefault();
//            //Assert.NotNull(savedRec);
//            //Assert.Equal("jfang", savedRec.Author.AccountName);

//            //// change the note render
//            //var newUser = UserStorage.GetEntities().FirstOrDefault(u=>u.AccountName != "jfang");
//            //Assert.NotNull(newUser);
//            //note.Author = newUser;

//            //// Act
//            //savedRec = NoteStorage.Update(note);

//            //// Assert
//            //Assert.False(NoteStorage.ProcessMessage.Success);
//            //Assert.Null(savedRec);
//            //Assert.Equal("jfang", note.Author.AccountName);
//        }

//        [Fact]
//        public void CanSearchNoteById()
//        {
//            throw new NotImplementedException();
//        }

//        [Fact]
//        public void CanDeleteNote()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}