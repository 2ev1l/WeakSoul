using UnityEngine;

namespace Universal
{
    public class AudioStorage : MonoBehaviour
    {
        #region fields & properties
        public static AudioStorage Instance { get; private set; }
        [field: SerializeField] public AudioClip MenuMusic { get; private set; }
        [field: SerializeField] public AudioClip GameMusic { get; private set; }
        [field: SerializeField] public AudioClip BuySound { get; private set; }
        [field: SerializeField] public AudioClip CorrectSound { get; private set; }
        [field: SerializeField] public AudioClip ErrorSound { get; private set; }
        [field: SerializeField] public AudioClip MessySound { get; private set; }
        [field: SerializeField] public AudioClip SuccessCraftSound { get; private set; }
        [field: SerializeField] public AudioClip HammerSound { get; private set; }
        [field: SerializeField] public AudioClip BadRandomSound { get; private set; }
        [field: SerializeField] public AudioClip GoodRandomSound { get; private set; }
        [field: SerializeField] public AudioClip BurnSound { get; private set; }

        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        #endregion methods
    }
}