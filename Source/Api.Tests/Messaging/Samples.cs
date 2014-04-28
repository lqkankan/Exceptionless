﻿using System;
using System.Threading;

namespace Exceptionless.Api.Tests.Messaging {
    public class SimpleMessageA : ISimpleMessage {
        public string Data { get; set; }
    }

    public class SimpleMessageB : ISimpleMessage {
        public string Data { get; set; }
    }

    public class SimpleMessageC {
        public string Data { get; set; }
    }

    public interface ISimpleMessage {
        string Data { get; set; }
    }

    public class CountDownLatch {
        private int _remaining;
        private EventWaitHandle _event;

        public CountDownLatch(int count) {
            Reset(count);
        }

        public void Reset(int count) {
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            _remaining = count;
            _event = new ManualResetEvent(false);
            if (_remaining == 0)
                _event.Set();
        }

        public void Signal() {
            // The last thread to signal also sets the event.
            if (Interlocked.Decrement(ref _remaining) == 0)
                _event.Set();
        }

        public bool Wait(int millisecondsTimeout) {
            return _event.WaitOne(millisecondsTimeout);
        }
    }
}