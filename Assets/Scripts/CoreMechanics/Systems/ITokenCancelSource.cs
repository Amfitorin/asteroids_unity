using System.Threading;

namespace CoreMechanics.Systems
{
    public interface ITokenCancelSource
    {
        void SetupTokenSource(CancellationTokenSource tokenSource);
    }
}