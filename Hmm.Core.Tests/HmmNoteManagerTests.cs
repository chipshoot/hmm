using System;
using DomainEntity.Misc;
using Hmm.Contract.Core;
using Hmm.Core.Manager;
using Hmm.Utility.TestHelp;
using System.Linq;
using System.Xml;
using DomainEntity.User;
using Hmm.Core.Manager.Validation;
using Xunit;

namespace Hmm.Core.Tests
{
    public class HmmNoteManagerTests : TestFixtureBase
    {
        private const string Namespace = @"http://schema.hmm.com/2020";
        private readonly IHmmNoteManager _manager;
        private readonly User _user;

        public HmmNoteManagerTests()
        {
            InsertSeedRecords();
            _user = UserRepository.GetEntities().FirstOrDefault();
            _manager = new HmmNoteManager(NoteRepository, new NoteValidator(NoteRepository));
        }

        [Fact]
        public void CanAddNoteToDataSource()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                $"<?xml version=\"1.0\" encoding=\"utf-16\" ?><Note xmlns=\"{Namespace}\"><Content>Test content</Content></Note>");

            var note = new HmmNote
            {
                Author = _user,
                Subject = "Testing note",
                Content = "Test content"
            };

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(newNote);
            Assert.True(newNote.Id >= 1, "newNote.Id >=1");
            Assert.Equal("Testing note", newNote.Subject);
            Assert.Equal(newNote.CreateDate, newNote.LastModifiedDate);
        }

        [Fact]
        public void CanGetRightXmlWithNullNoteContentToDataSource()
        {
            // Arrange - note with null content
            var note = new HmmNote
            {
                Author = _user,
                Subject = "Testing note",
                Content = null
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml($"<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"{Namespace}\"><content></content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(newNote);
        }

        [Fact]
        public void CanGetRightXmlWithNoteContentContainsInvalidCharToDataSource()
        {
            // Arrange - note with null content
            var note = new HmmNote
            {
                Author = _user,
                Subject = "Testing note",
                Content = "Testing content with < and >"
            };
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml($"<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"{Namespace}\"><content>Testing content with &lt; and &gt;</content></note>");

            // Act
            var newNote = _manager.Create(note);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(newNote);
            Assert.Contains("&lt;", note.Content);
            Assert.Contains("&gt;", note.Content);
        }

        [Fact]
        public void CanUpdateNote()
        {
            // Arrange - note with null content
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<?xml version=\"1.0\" encoding=\"utf-16\" ?><note xmlns=\"{Namespace}\"><content>Testing content with &lt; and &gt;</content></note>");
            var note = new HmmNote
            {
                Author = _user,
                Subject = "Testing note",
                Content = xmlDoc.InnerXml,
            };
            _manager.Create(note);

            // Act
            note.Subject = "new note subject";
            note.Content = "This is new note content";
            var newNote = _manager.Update(note);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(newNote);
            Assert.Equal("new note subject", newNote.Subject);
        }

        [Fact]
        public void CannotUpdateNoteAuthor()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><root><time>2017-08-01</time></root>");
            var note = new HmmNote
            {
                Author = _user,
                Description = "testing note",
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Subject = "testing note is here",
                Content = xmlDoc.InnerXml
            };
            _manager.Create(note);

            Assert.True(NoteRepository.ProcessMessage.Success);
            var savedRec = NoteRepository.GetEntities().FirstOrDefault();
            Assert.NotNull(savedRec);
            Assert.Equal("jfang", savedRec.Author.AccountName);

            // change the note render
            var newUser = UserRepository.GetEntities().FirstOrDefault(u => u.AccountName != "jfang");
            Assert.NotNull(newUser);
            note.Author = newUser;

            // Act
            savedRec = _manager.Update(note);

            // Assert
            Assert.False(_manager.ProcessResult.Success);
            Assert.Null(savedRec);
        }

        [Fact]
        public void CanSearchNoteById()
        {
        //    throw new NotImplementedException();
        }

        [Fact]
        public void CanDeleteNote()
        {
         //   throw new NotImplementedException();
        }
    }
}