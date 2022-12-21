using System;
using System.Threading;
using Core.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Timer = Core.Time.Timer;

namespace Core.Utils.Extensions
{
    public static class TimerExtensions
    {
        public static UniTask.Awaiter GetAwaiter(this Timer handle)
        {
            return ToUniTask(handle).GetAwaiter();
        }

        public static UniTask WithCancellation(this Timer handle, CancellationToken cancellationToken)
        {
            return ToUniTask(handle, cancellationToken: cancellationToken);
        }

        public static UniTask ToUniTask(this Timer handle, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return UniTask.FromCanceled(cancellationToken);

            if (handle.IsCancelled)
            {
                Debug.LogWarning("Timer is cancelled, force await");
                return UniTask.CompletedTask;
            }

            if (handle.IsDone)
            {
                return UniTask.CompletedTask;
            }

            if (!handle.IsRunned)
            {
                handle.Run();
            }

            return new UniTask(
                TimerAwaiter.Create(handle, timing, progress, cancellationToken, out var token), token);
        }
    }
}