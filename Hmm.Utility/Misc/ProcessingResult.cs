using System.Collections.Generic;

namespace Hmm.Utility.Misc
{
    public class ProcessingResult
    {
        public ProcessingResult()
        {
            Success = true;
            MessageList = new List<string>();
        }

        public bool Success { get; set; }

        public List<string> MessageList { get; private set; }

        public void Rest()
        {
            Success = true;
            MessageList.Clear();
        }

        public void AddMessage(string message)
        {
            MessageList.Add(message);
        }
    }
}