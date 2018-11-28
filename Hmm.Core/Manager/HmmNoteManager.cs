using DomainEntity.Misc;
using Hmm.Contract.Core;
using Hmm.Core.Manager.Validation;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Hmm.Core.Manager
{
    public class HmmNoteManager : IHmmNoteManager<HmmNote>
    {
        #region private fields

        private readonly IDataStore<HmmNote> _noteStorage;
        private readonly IEntityLookup _lookup;
        private readonly NoteValidator _validator;

        #endregion private fields

        public HmmNoteManager(IDataStore<HmmNote> storage, IEntityLookup lookup, NoteValidator validator)
        {
            Guard.Against<ArgumentNullException>(storage == null, nameof(storage));
            Guard.Against<ArgumentNullException>(lookup == null, nameof(lookup));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));
            _noteStorage = storage;
            _lookup = lookup;
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
            var ret = _noteStorage.Add(note);
            if (ret == null)
            {
                ProcessResult.PropagandaResult(_noteStorage.ProcessMessage);
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
            var ret = _noteStorage.Update(note);
            if (ret == null)
            {
                ProcessResult.PropagandaResult(_noteStorage.ProcessMessage);
            }

            return ret;
        }

        public HmmNote GetNoteById(int id)
        {
            var note = _lookup.GetEntity<HmmNote>(id);
            return note;
        }

        public IEnumerable<HmmNote> GetNotes()
        {
            var notes = _noteStorage.GetEntities();
            return notes;
        }

        public XNamespace ContentNamespace => "http://schema.hmm.com/2017";

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
    }
}