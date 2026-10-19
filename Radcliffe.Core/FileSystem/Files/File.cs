using Radcliffe.Core.FileSystem.Directories;
using Radcliffe.Core.EventSystem;

using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Radcliffe.Core.FileSystem.Files {
    public class BaseFile { }
    public abstract class File<TData, TEncoding> : BaseFile {

        protected string pFileExt;
        protected string pFileName;
        protected Directory pParent;
        private ConcurrentQueue<(Func<(OperationResult, TData)> operation, Event<OperationResult, TData> onComplete)> _operationQueue = new();
        private ConcurrentDictionary<int, bool> _dequeueOperations = new();
        private Task<(OperationResult, TData)> _currentOperation;
        protected int _dequeueCount = 0;

        public File(string name, ref Directory parent) {
            pFileExt = "txt";
            pFileName = name;
            pParent = parent;
        }

        public void Save(TData data, Event<OperationResult, TData> onComplete)
        {
            Do( () => DoSave( data ), onComplete );
        }

        public void Load(Event<OperationResult, TData> onComplete)
        {
            Do( () => DoLoad( ), onComplete );
        }

        private void Do(Func<(OperationResult, TData)> operation, Event<OperationResult, TData> onComplete)
        {
            GarbageCollection();
            (Func<(OperationResult, TData)> operation, Event<OperationResult, TData> onComplete) item = new();
            item.operation = operation;
            item.onComplete = onComplete;
            _operationQueue.Enqueue( item );
            int _dequeueID = _dequeueCount++;

            _dequeueOperations.TryAdd(_dequeueID, true );
            _ = DequeueOperations(_dequeueID);
        }

        private async Task DequeueOperations(int id)
        {
            while (_operationQueue.Count > 0)
            {
                if(_dequeueOperations.Count > 1)
                {
                    int newestOperation = 0;

                    foreach (int operationId in _dequeueOperations.Keys)
                    {
                        if (operationId > newestOperation)
                        {
                            newestOperation = operationId;
                        }
                    }
                    if (id != newestOperation)
                    {
                        _dequeueOperations.TryRemove( id, out bool _removed);
                        return;
                    }
                }
                bool dequeued = _operationQueue.TryDequeue(out (Func<(OperationResult, TData)> operation, Event<OperationResult, TData> onComplete) currentItem);
                if (dequeued)
                {
                    _currentOperation = Task.Run( currentItem.operation );
                    (OperationResult result, TData data) result = await _currentOperation;
                    currentItem.onComplete.Raise( result.result, result.data );
                }
            }

            _dequeueOperations.TryRemove( id, out bool removed );
        }

        private void GarbageCollection()
        {
            if (_operationQueue.IsEmpty)
            {
                _dequeueOperations.Clear();
                _dequeueCount = 0;
            }
        }
        protected abstract TEncoding Encode(TData data);
        protected abstract TData Decode(TEncoding data);
        protected abstract (OperationResult, TData) DoSave(TData data);
        protected abstract (OperationResult, TData) DoLoad();

    }
}
