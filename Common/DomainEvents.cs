
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;

namespace INCASOL.IP.Infrastructure.Events
{
    /// <summary>
    /// http://msdn.microsoft.com/en-gb/magazine/ee236415.aspx#id0400046
    /// </summary>
    public class DomainEvents : IDomainEvents
    {
        private List<Delegate> _actions;
        private readonly ILifetimeScope _scope;

        public DomainEvents(ILifetimeScope scope) 
        {
            if (scope == null)
                throw new ArgumentNullException("scope");

            _scope = scope;
        }

        public void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
            {
                _actions = new List<Delegate>();
            }
            _actions.Add(callback);
        }

        public void ClearCallbacks()
        {
            _actions = null;
        }

        public void Raise<T>(T args) where T : IDomainEvent
        {
            //using (var nestedScope = _scope.BeginLifetimeScope("AutofacWebRequest"))
            //{
            //    var handlers = nestedScope.Resolve<IEnumerable<IHandle<T>>>();
            //    //handlers = handlers.ToList().OrderBy(h => h.ExecutionOrder);
            //    foreach (var handler in handlers)
            //    {
            //        handler.Handle(args);
            //    }
            //}
                var handlers = _scope.Resolve<IEnumerable<IHandle<T>>>();
                //handlers = handlers.ToList().OrderBy(h => h.ExecutionOrder);
                foreach (var handler in handlers)
                {
                    handler.Handle(args);
                }

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }
}
