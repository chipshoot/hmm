using Hmm.Utility.Dal.Exceptions;
using System;
using System.Collections.Generic;

namespace Hmm.Utility.Misc
{
    public class ProcessingResult
    {
        public ProcessingResult()
        {
            Success = true;
            MessageList = new List<ReturnMessage>();
        }

        public bool Success { get; set; }

        public List<ReturnMessage> MessageList { get; }

        public void Rest()
        {
            Success = true;
            MessageList.Clear();
        }

        public void AddErrorMessage(string message, bool clearOldMessage = false)
        {
            if (clearOldMessage)
            {
                MessageList.Clear();
            }

            MessageList.Add(new ReturnMessage { Message = message, Type = MessageType.Error });
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
            AddErrorMessage(ex.GetAllMessage());
        }
    }
}