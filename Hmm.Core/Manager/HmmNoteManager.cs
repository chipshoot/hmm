using DomainEntity.Misc;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
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
            var xmlContent = GetNoteContent(note);
            note.Content = xmlContent.ToString(SaveOptions.DisableFormatting);
            var ret = _noteStorage.Update(note);

            return ret;
        }

        HmmNote IHmmNoteManager<HmmNote>.GetNoteById(int id)
        {
            var note = _lookup.GetEntity<HmmNote>(id);
            return note;
        }

        public ProcessingResult ErrorMessage { get; } = new ProcessingResult();

        private static XDocument GetNoteContent(HmmNote note, bool isXmlConent = false)
        {
            XNamespace ns = "http://schema.hmm.com/2017";
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-16", "yes"),
                new XElement("Note",
                    new XElement("Content", "")));

            // ReSharper disable once PossibleNullReferenceException
            xml.Root.Add(new XAttribute(XNamespace.Xmlns + "hmm", ns));

            if (isXmlConent)
            {
                var innerElement = XElement.Parse(note.Content);
                xml.Root.Element("Content")?.Add(innerElement);
            }
            else
            {
                // ReSharper disable once PossibleNullReferenceException
                xml.Root.Element("Content").Value = note.Content;
            }
            //var xmldoc = new XmlDocument();
            //var content = note.Content;
            //xmldoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\" ?><Note><Content></Content></Note>");
            //xmldoc.DocumentElement.SetAttribute("xmlns", "http://schema.hmm.com/2017");
            //var ns = new XmlNamespaceManager(xmldoc.NameTable);
            //ns.AddNamespace("", "http://schema.hmm.com/2017");
            //var contNode = xmldoc.SelectNodes("/Note/Content", ns);
            //if (contNode != null)
            //{
            //    if (isXmlConent)
            //    {
            //        contNode[0].InnerXml = content;
            //    }
            //    else
            //    {
            //        contNode[0].InnerText = content;
            //    }
            //}

            return xml;
        }
    }
}