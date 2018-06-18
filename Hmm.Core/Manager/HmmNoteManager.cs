using DomainEntity.Misc;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Xml;

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
            note.Content = xmlContent.InnerXml;
            var ret = _noteStorage.Add(note);
            return ret;
        }

        public HmmNote Update(HmmNote note)
        {
            var xmlContent = GetNoteContent(note);
            note.Content = xmlContent.InnerXml;
            var ret = _noteStorage.Update(note);

            return ret;
        }

        HmmNote IHmmNoteManager<HmmNote>.GetNoteById(int id)
        {
            var note = _lookup.GetEntity<HmmNote>(id);
            return note;
        }

        public ProcessingResult ErrorMessage { get; } = new ProcessingResult();

        public  XmlDocument GetNoteContent(HmmNote note, bool isXmlConent = false)
        {
            var xmldoc = new XmlDocument();
            var content = note.Content;
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><note><content></content></note>");
            xmldoc.DocumentElement.SetAttribute("xmlns", "http://schema.hmm.com/2017");
            var ns = new XmlNamespaceManager(xmldoc.NameTable);
            ns.AddNamespace("", "http://schema.hmm.com/2017");
            var contNode = xmldoc.SelectNodes("/note/content", ns);
            if (contNode != null)
            {
                if (isXmlConent)
                {
                    contNode[0].InnerXml = content;
                }
                else
                {
                    contNode[0].InnerText = content;
                }
            }

            return xmldoc;
        }
    }
}