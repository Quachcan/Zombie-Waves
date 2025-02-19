using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager Instance { get; private set; }

        private readonly Dictionary<Animator, Dictionary<string, int>> animatorHashes = new Dictionary<Animator, Dictionary<string, int>>();

        private void Awake()
        {
            Instance = this;
        }

        public void InitializeAnimator(Animator animator, params string[] parameters)
        {
            if (!animatorHashes.ContainsKey(animator))
            {
                var hashes = new Dictionary<string, int>();
                foreach (var parameter in parameters)
                {
                    hashes[parameter] = Animator.StringToHash(parameter);
                }
                animatorHashes[animator] = hashes;
            }
        }

        public void SetBool(Animator animator, string parameter, bool value)
        {
            if (animatorHashes.TryGetValue(animator, out var hashes) && hashes.TryGetValue(parameter, out var hash))
            {
                animator.SetBool(hash, value);
            }
        }

        public void SetTrigger(Animator animator, string parameter)
        {
            if (animatorHashes.TryGetValue(animator, out var hashes) && hashes.TryGetValue(parameter, out var hash))
            {
                animator.SetTrigger(hash);
            }
        }
    }
}
