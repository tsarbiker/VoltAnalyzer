using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.ValueObjects.Types.Common
{
    public static class VoltAnalyzerMessage
    {
        public static void Subscribe(object subscriber, string message, Action action)
        {
            Messenger.Default.Register<string>(subscriber, (object)message, (Action<string>)(s => action()));
        }

        public static void Subscribe<T>(object subscriber, string message, Action<T> action)
        {
            Messenger.Default.Register<T>(subscriber, (object)message, action);
        }

        public static void UnSubscribeAll(object subscriber)
        {
            Messenger.Default.Unregister(subscriber);
        }

        public static void Send<T>(string message, T parameter)
        {
            Messenger.Default.Send<T>(parameter, (object)message);
        }
    }
}
