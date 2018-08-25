/*

namespace JohanPolosn.UnityInjector
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Unity Injector/Photon Network Injector")]
    public class PhotonNetworkInjector : MonoBehaviour
    {
        public bool includeInactive;
        public Component[] components;

        private void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var tempDependencys = new Dictionary<Type, object>
            {
                { typeof(PhotonMessageInfo?), new PhotonMessageInfo?(info) }
            };

            if (this.components.Length == 0)
            {
                GlobalInjector.singleton.Inject(this.gameObject, includeInactive);
            }
            else
            {
                for (int i = 0; i < this.components.Length; i++)
                {
                    GlobalInjector.singleton.Inject(this.components[i]);
                }
            }
        }
    }
}

*/