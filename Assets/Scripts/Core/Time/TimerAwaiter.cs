using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Time
{
    internal sealed class TimerAwaiter : IUniTaskSource, IPlayerLoopItem,
        ITaskPoolNode<TimerAwaiter>
    {
        private static TaskPool<TimerAwaiter> _pool;

        private readonly Action _continuationAction;
        private CancellationToken _cancellationToken;
        private bool _completed;

        private UniTaskCompletionSourceCore<AsyncUnit> _core;
        private Timer _handle;
        private TimerAwaiter _nextNode;
        private IProgress<float> _progress;

        static TimerAwaiter()
        {
            TaskPool.RegisterSizeGetter(typeof(TimerAwaiter), () => _pool.Size);
        }

        private TimerAwaiter()
        {
            _continuationAction = Continuation;
        }

        public bool MoveNext()
        {
            if (_completed)
            {
                TryReturn();
                return false;
            }

            if (_cancellationToken.IsCancellationRequested)
            {
                _completed = true;
                _core.TrySetCanceled(_cancellationToken);
                return false;
            }

            _progress?.Report(_handle.PercentComplete);

            return true;
        }

        public ref TimerAwaiter NextNode => ref _nextNode;

        public void GetResult(short token)
        {
            _core.GetResult(token);
        }

        public UniTaskStatus GetStatus(short token)
        {
            return _core.GetStatus(token);
        }

        public UniTaskStatus UnsafeGetStatus()
        {
            return _core.UnsafeGetStatus();
        }

        public void OnCompleted(Action<object> continuation, object state, short token)
        {
            _core.OnCompleted(continuation, state, token);
        }

        public static IUniTaskSource Create(Timer handle, PlayerLoopTiming timing, IProgress<float> progress,
            CancellationToken cancellationToken, out short token)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
            }

            if (!_pool.TryPop(out var result))
            {
                result = new TimerAwaiter();
            }

            result._handle = handle;
            result._progress = progress;
            result._cancellationToken = cancellationToken;
            result._completed = false;

            TaskTracker.TrackActiveTask(result, 3);

            PlayerLoopHelper.AddAction(timing, result);

            handle.Completed += result._continuationAction;

            token = result._core.Version;
            return result;
        }

        private void Continuation()
        {
            _handle.Completed -= _continuationAction;

            if (_completed)
            {
                TryReturn();
            }
            else
            {
                _completed = true;
                if (_cancellationToken.IsCancellationRequested)
                {
                    _core.TrySetCanceled(_cancellationToken);
                }
                else
                {
                    _core.TrySetResult(AsyncUnit.Default);
                }
            }
        }

        private void TryReturn()
        {
            TaskTracker.RemoveTracking(this);
            _core.Reset();
            _handle = default;
            _progress = default;
            _cancellationToken = default;
            _pool.TryPush(this);
        }
    }
}