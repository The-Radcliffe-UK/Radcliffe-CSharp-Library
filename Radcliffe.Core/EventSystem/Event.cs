using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radcliffe.Core.EventSystem {
    public class Event {

        protected List<Action> pListeners = [];
        protected List<Func<Task>> pAsyncListeners = [];

        public virtual void Raise()
        {
            for (int i = pListeners.Count - 1; i >= 0; i--)
            {
                pListeners[i].Invoke();
            }
        }

        public virtual async Task AsyncRaise()
        {
            Action[] _listenerSnapshot = pListeners.ToArray();
            Func<Task>[] _asyncListenerSnapshot = pAsyncListeners.ToArray();

            List<Task> _tasks = new(_listenerSnapshot.Length);

            for (int i = _listenerSnapshot.Length - 1; i >= 0; i--)
            {
                _tasks.Add( Task.Run( () => _listenerSnapshot[i].Invoke() ) );
            }

            for (int i = _asyncListenerSnapshot.Length - 1; i >= 0; i--)
            {
                _tasks.Add( Task.Run( () => _asyncListenerSnapshot[i].Invoke() ) );
            }

            await Task.WhenAll( _tasks );
        }

        public void RegisterAsyncListener(Func<Task> listener)
        {
            pAsyncListeners.Add( listener );
        }

        public void UnregisterAsyncListener(Func<Task> listener)
        {
            pAsyncListeners.Remove( listener );
        }

        public void RegisterListener(Action listener)
        {
            pListeners.Add( listener );
        }

        public void UnregisterListener(Action listener)
        {
            pListeners.Remove( listener );
        }
    }

    public class Event<T0> {
        protected  List<Action<T0>> pListeners = [];
        public virtual void Raise(T0 arg1) // 4
        {
            for (int i = pListeners.Count - 1; i >= 0; i--)
            {
                pListeners[i].Invoke( arg1 );
            }
        }

        public virtual async Task AsyncRaise(T0 arg1)
        {
            Action<T0>[] _listenerSnapshot = pListeners.ToArray();

            List<Task> _tasks = new(_listenerSnapshot.Length);

            for (int i = _listenerSnapshot.Length - 1; i >= 0; i--)
            {
                _tasks.Add( Task.Run( () => _listenerSnapshot[i].Invoke( arg1 ) ) );
            }

            await Task.WhenAll( _tasks );
        }

        public void RegisterListener(Action<T0> listener)
        {
            pListeners.Add( listener );
        }

        public void UnregisterListener(Action<T0> listener)
        {
            pListeners.Remove( listener );
        }
    }

    public class Event<T0, T1> {
        protected  List<Action<T0, T1>> pListeners = [];
        public void Raise(T0 arg0, T1 arg1) // 4
        {
            for (int i = pListeners.Count - 1; i >= 0; i--)
            {
                if (pListeners[i] != null)
                {
                    pListeners[i].Invoke( arg0, arg1 );
                }
            }
        }

        public void RegisterListener(Action<T0, T1> listener)
        {
            pListeners.Add( listener );
        }

        public void UnregisterListener(Action<T0, T1> listener)
        {
            pListeners.Remove( listener );
        }
    }

    public class Event<T0, T1, T2> {
        protected  List<Action<T0, T1, T2>> pListeners = [];
        public void Raise(T0 arg0, T1 arg1, T2 arg2) // 4
        {
            for (int i = pListeners.Count - 1; i >= 0; i--)
            {
                pListeners[i].Invoke( arg0, arg1, arg2 );
            }
        }

        public void RegisterListener(Action<T0, T1, T2> listener)
        {
            pListeners.Add( listener );
        }

        public void UnregisterListener(Action<T0, T1, T2> listener)
        {
            pListeners.Remove( listener );
        }
    }

    public class Event<T0, T1, T2, T3> {
        protected List<Action<T0, T1, T2, T3>> pListeners = [];
        protected List<Func<T0, T1, T2, T3, Task>> pAsyncListeners = [];
        public void Raise(T0 arg0, T1 arg1, T2 arg2, T3 arg3) // 4
        {
            Action<T0, T1, T2, T3>[] _listenerSnapshot = pListeners.ToArray();

            for (int i = _listenerSnapshot.Length - 1; i >= 0; i--)
            {
                pListeners[i].Invoke( arg0, arg1, arg2, arg3 );
            }
        }

        public virtual async Task AsyncRaise(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            Action<T0, T1, T2, T3>[] _listenerSnapshot = pListeners.ToArray();
            Func<T0, T1, T2, T3, Task>[] _asyncListenerSnapshop = pAsyncListeners.ToArray();

            List<Task> _tasks = new(_listenerSnapshot.Length);

            for (int i = _listenerSnapshot.Length - 1; i >= 0; i--)
            {
                _tasks.Add( Task.Run( () => _listenerSnapshot[i].Invoke( arg0, arg1, arg2, arg3 ) ) );
            }

            for (int i = _asyncListenerSnapshop.Length - 1; i >= 0; i--)
            {
                _tasks.Add( Task.Run( () => _asyncListenerSnapshop[i].Invoke( arg0, arg1, arg2, arg3 ) ) );
            }

            await Task.WhenAll( _tasks );
        }

        public void RegisterListener(Action<T0, T1, T2, T3> listener)
        {
            pListeners.Add( listener );
        }

        public void UnregisterListener(Action<T0, T1, T2, T3> listener)
        {
            pListeners.Remove( listener );
        }

        public void RegisterAsyncListener(Func<T0, T1, T2, T3, Task> listener)
        {
            pAsyncListeners.Add( listener );
        }

        public void UnregisterAsyncListener(Func<T0, T1, T2, T3, Task> listener)
        {
            pAsyncListeners.Remove( listener );
        }
    }
}