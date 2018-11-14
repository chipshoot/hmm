using DomainEntity.Misc;
using Hmm.Contract.Core;
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

        #endregion private fields

        public HmmNoteManager(IDataStore<HmmNote> storage, IEntityLookup lookup)
        {
            Guard.Against<ArgumentNullException>(storage == null, nameof(storage));
            Guard.Against<ArgumentNullException>(lookup == null, nameof(lookup));
            _noteStorage = storage;
            _lookup = lookup;
        }

        public HmmNote Create(HmmNote note)
        {
            var xmlContent = GetNoteContent(note, true);
            note.Content = xmlContent.ToString(SaveOptions.DisableFormatting);
            var ret = _noteStorage.Add(note);
            return ret;
        }

        public HmmNote Update(HmmNote note)
        {
            var xmlContent = GetNoteContent(note, true);
            note.Content = xmlContent.ToString(SaveOptions.DisableFormatting);
            var ret = _noteStorage.Update(note);

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

            if (isXmlContent)
            {
                var innerElement = XElement.Parse(note.Content);
                xml.Root?.Element("Content")?.Add(innerElement);
                foreach (var el in xml.Descendants())
                {
                    el.Name = ContentNamespace + el.Name.LocalName;
                }
            }
            else
            {
                // ReSharper disable PossibleNullReferenceException
                xml.Root.Element("Content").Value = note.Content;
                // ReSharper restore PossibleNullReferenceException
            }

            return xml;
        }
    }
}