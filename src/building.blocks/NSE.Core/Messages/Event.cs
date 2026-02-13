using System;
using MediatR;

namespace NSE.Core.Messages
{
    public class Event : Messagens, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}