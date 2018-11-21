using System;
using System.Collections.Generic;
using Hmm.Utility.Dal.Exceptions;

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

        public List<string> MessageList { get; }

        public void Rest()
        {
            Success = true;
            MessageList.Clear();
        }

        public void AddMessage(string message, bool clearOldMessage = false)
        {
            if (clearOldMessage)
            {
                MessageList.Clear();
            }

            MessageList.Add(message);
        }

        public void PropagandaResult(ProcessingResult innerResult)
        {
            Rest();
            Success = innerResult.Success;
            MessageList.AddRange(innerResult.MessageList);
        }

        public void WrapException(Exception ex)
        {
            Rest();
            Success = false;
            AddMessage(ex.GetAllMessage());
        }
    }
}