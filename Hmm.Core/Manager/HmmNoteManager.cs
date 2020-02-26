using DomainEntity.Misc;
using Hmm.Contract.Core;
using Hmm.Core.Manager.Validation;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Hmm.Core.Manager
{
    public class HmmNoteManager : IHmmNoteManager
    {
        #region private fields

        private readonly IVersionRepository<HmmNote> _noteNoteRepo;
        private readonly NoteValidator _validator;

        #endregion private fields

        public HmmNoteManager(IVersionRepository<HmmNote> noteRepo, NoteValidator validator)
        {
            Guard.Against<ArgumentNullException>(noteRepo == null, nameof(noteRepo));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));
            _noteNoteRepo = noteRepo;
            _validator = validator;
        }

        public HmmNote Create(HmmNote note)
        {
            if (!_validator.IsValidEntity(note, ProcessResult))
            {
                return null;
            }

            var xmlContent = GetNoteContent(note, true);
            note.Content = xmlContent.ToString(SaveOptions.DisableFormatting);
            var ret = _noteNoteRepo.Add(note);
            if (ret == null)
            {
                ProcessResult.PropagandaResult(_noteNoteRepo.ProcessMessage);
            }
            return ret;
        }

        public HmmNote Update(HmmNote note)
        {
            if (!_validator.IsValidEntity(note, ProcessResult))
            {
                return null;
            }

            var xmlContent = GetNoteContent(note, true);
            note.Content = xmlContent.ToString(SaveOptions.DisableFormatting);

            // make sure not update note which get cached in current session
            var adjustNote = GetCachedNote(note);
            var ret = _noteNoteRepo.Update(adjustNote);
            if (ret == null)
            {
                ProcessResult.PropagandaResult(_noteNoteRepo.ProcessMessage);
            }

            return ret;
        }

        public HmmNote GetNoteById(int id)
        {
            var note = GetNotes().FirstOrDefault(n => n.Id == id);
            return note;
        }

        public IEnumerable<HmmNote> GetNotes()
        {
            var notes = _noteNoteRepo.GetEntities();
            return notes;
        }

        public XNamespace ContentNamespace => "http://schema.hmm.com/2020";

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        private XDocument GetNoteContent(HmmNote note, bool isXmlContent = false)
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-16", "yes"),
                new XElement("Note", new XElement("Content", "")));

            var content = note.Content ?? string.Empty;
            if (isXmlContent)
            {
                try
                {
                    var innerElement = XElement.Parse(content);
                    xml.Root?.Element("Content")?.Add(innerElement);
                }
                catch (Exception)
                {
                    // ReSharper disable PossibleNullReferenceException
                    xml.Root.Element("Content").Value = content;
                }
            }
            else
            {
                xml.Root.Element("Content").Value = content;
                // ReSharper restore PossibleNullReferenceException
            }

            foreach (var el in xml.Descendants())
            {
                el.Name = ContentNamespace + el.Name.LocalName;
            }

            return xml;
        }

        /// <summary>
        /// The method is used to mapped changed note to cached note in current context to avoid
        /// data source throw exception of already getting tracking note with same id
        /// </summary>
        /// <param name="note">The note need to be updated</param>
        /// <returns>
        /// If there is cached note, return it with updated content, otherwise return current note
        /// </returns>
        private HmmNote GetCachedNote(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var cachedNote = _noteNoteRepo.GetEntities().FirstOrDefault(n => n.Id == note.Id);
            if (cachedNote == null)
            {
                return note;
            }

            cachedNote.Subject = note.Subject;
            cachedNote.Content = note.Content;
            cachedNote.Catalog = note.Catalog;
            cachedNote.LastModifiedDate = note.LastModifiedDate;

            return cachedNote;
        }
    }
}