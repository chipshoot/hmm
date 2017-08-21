using DomainEntity.Misc;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using System;
using System.Xml;

namespace Hmm.Core.Manager
{
    public class HmmNoteManager : IHmmNoteManager
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
            var xmlContent = GetNoteContent(note.Content);
            note.Content = xmlContent.InnerXml;
            var ret = _noteStorage.Add(note);
            return ret;
        }

        private XmlDocument GetNoteContent(string content)
        {
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(
                "<?xml version=\"1.0\" encoding=\"UTF-16\" ?><HmmNote xmlns=\"http://schema.hmm.com/2017\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://schema.hmm.com/2017 NotebaseSchema.xsd\"><Content></Content></HmmNote>");

            var contNode = xmldoc.SelectNodes("/HmmNote/Content");
            if (contNode != null)
            {
                contNode[0].InnerText = content;
            }

            return xmldoc;
        }
    }
}