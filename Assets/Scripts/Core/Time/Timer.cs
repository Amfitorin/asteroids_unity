using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Core.Time
{
    public class Timer
    {
        private readonly CancellationTokenSource _globalToken;
        private readonly float _seconds;
        private readonly PlayerLoopTiming _timing;

        private float _time;
        private CancellationTokenSource _timerToken;

        public Timer(float seconds, CancellationTokenSource globalToken)
        {
            _seconds = seconds;
            _timing = PlayerLoopTiming.PreUpdate;
            _globalToken = globalToken;
            Reset();
        }

        public float TotalSeconds => _seconds;
        public float CurrentTime => _time;

        public bool IsDone => Mathf.Approximately(_seconds, 0f) || _time > _seconds;
        public float PercentComplete => Mathf.Approximately(_seconds, 0f) ? 1f : Mathf.Clamp01(_time / _seconds);

        public bool IsRunned { get; private set; }
        public bool IsCancelled { get; private set; }

        private void ResetTimerToken()
        {
            if (_timerToken is { IsCancellationRequested: false })
            {
                _timerToken.Cancel();
            }

            _timerToken = new CancellationTokenSource();
        }

        public event Action Completed;

        public void Run()
        {
            if (CheckComplete())
            {
                return;
            }

            IsRunned = true;

            AsyncRun().Forget();
        }

        public void Reset()
        {
            _time = 0f;
            IsRunned = false;
            IsCancelled = false;
            ResetTimerToken();
        }

        public void Restart()
        {
            Reset();
            Run();
        }


        private bool CheckComplete()
        {
            if (!IsDone)
                return false;
            IsRunned = false;
            IsCancelled = true;
            return true;
        }

        private async UniTaskVoid AsyncRun()
        {
            var tokenSource = _globalToken != null
                ? CancellationTokenSource.CreateLinkedTokenSource(_globalToken.Token, _timerToken.Token)
                : _timerToken;

            await UniTask.Yield();
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate(_timing)
                               .WithCancellation(tokenSource.Token))
            {
                _time += UnityEngine.Time.deltaTime;
                if (!CheckComplete()) continue;
                _timerToken.Cancel();
                Completed?.Invoke();
            }
        }
    }
}