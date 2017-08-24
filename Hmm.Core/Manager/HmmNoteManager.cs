using DomainEntity.Misc;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using System;
using System.Xml;

namespace Hmm.Core.Manager
{
    public class HmmNoteManager : IHmmNoteManager<HmmNote>
    {
        #region private fields

        private readonly IDataStore<HmmNote> _noteStorage;

        #endregion private fields

        public HmmNoteManager(IDataStore<HmmNote> storage)
        {
            Guard.Against<ArgumentNullException>(storage == null, nameof(storage));
            _noteStorage = storage;
        }

        public HmmNote Create(HmmNote note)
        {
            var xmlContent = GetNoteContent(note);
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

        public virtual XmlDocument GetNoteContent(HmmNote note)
        {
            var xmldoc = new XmlDocument();
            var content = note.Content;
            xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-16\" ?><note><content></content></note>");
            xmldoc.DocumentElement.SetAttribute("xmlns", "http://schema.hmm.com/2017");
            var ns = new XmlNamespaceManager(xmldoc.NameTable);
            ns.AddNamespace("", "http://schema.hmm.com/2017");
            var contNode = xmldoc.SelectNodes("/note/content", ns);
            if (contNode != null)
            {
                contNode[0].InnerText = content;
            }

            return xmldoc;
        }
    }
}