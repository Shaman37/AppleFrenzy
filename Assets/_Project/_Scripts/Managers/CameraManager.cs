using UnityEngine;

namespace AppleFrenzy
{    
    /// <summary>
    ///     Responsible for saving references of the various cameras in game.
    /// </summary>
    public class CameraManager : Singleton<CameraManager>
    {
        public Camera gameCamera;      
    }
}