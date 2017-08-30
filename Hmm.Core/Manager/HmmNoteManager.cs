using DomainEntity.Misc;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using System;
using System.Xml;

namespace Hmm.Core.Manager
{
    public class HmmNoteManager<T> : IHmmNoteManager<T> where T : HmmNote
    {
        #region private fields

        private readonly IDataStore<T> _noteStorage;

        #endregion private fields

        public HmmNoteManager(IDataStore<T> storage)
        {
            Guard.Against<ArgumentNullException>(storage == null, nameof(storage));
            _noteStorage = storage;
        }

        public virtual T Create(T note)
        {
            var xmlContent = GetNoteContent(note);
            note.Content = xmlContent.InnerXml;
            var ret = _noteStorage.Add(note);
            return ret;
        }

        public virtual T Update(T note)
        {
            var xmlContent = GetNoteContent(note);
            note.Content = xmlContent.InnerXml;
            var ret = _noteStorage.Update(note);

            return ret;
        }

        protected virtual XmlDocument GetNoteContent(T note, bool isXmlConent = false)
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