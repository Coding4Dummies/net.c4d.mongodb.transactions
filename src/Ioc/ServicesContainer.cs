using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Net.C4D.Mongodb.Transactions.Ioc
{
    /// Pour man's Inversion of Controls
    public static class ServicesContainer
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void RegisterService<T>(T instance) where T : class
        {
            _services.Add(typeof(T), instance);
        }

        public static T GetService<T>() where T : class
        {

            var requestedServiceObject = _services.GetValueOrDefault(typeof(T));
            var requestedService = requestedServiceObject as T;

            if (requestedService == null)
                throw new InvalidOperationException("Requested service is was not registered");

            return requestedService;
        }
    }
}