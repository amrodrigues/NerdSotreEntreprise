using System;

namespace NSE.Core.Messages
{
    public abstract class Messagens
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Messagens()
        {
            MessageType = GetType().Name;
        }
    }
}