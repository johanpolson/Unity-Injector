/*

namespace JohanPolosn.UnityInjector
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PhotonNetworkInjector : MonoBehaviour
    {
        public static IDependencyInjector injector;

        public bool includeInactive;

        private void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var tempDependencys = new Dictionary<Type, object>
            {
                { typeof(PhotonMessageInfo?), new PhotonMessageInfo?(info) }
            };

            injector.Inject(this.gameObject, this.includeInactive, tempDependencys);
        }
    }
}

*/