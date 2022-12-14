using System.Threading;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public class AutomaticBulletMechanic : IBulletMechanic
    {
        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            
        }

        public void SetDirectionFunc(Func<Vector3> getDirection)
        {
            throw new System.NotImplementedException();
        }
    }
}