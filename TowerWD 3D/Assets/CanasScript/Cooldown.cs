using UnityEngine;

namespace CanasSource
{
    [System.Serializable]
    public class Cooldown
    {
        [System.NonSerialized]
        public float elapse;
        public float duration;

        public float remain { get { return Mathf.Max(duration - elapse, 0); } }

        public bool isCooling
        {
            get
            {
                return elapse < duration;
            }
        }

        public bool isFinished
        {
            get
            {
                return elapse > duration;
            }
        }

        public float progress
        {
            get
            {
                return Mathf.Clamp01(elapse / duration);
            }
        }

        public void End()
        {
            elapse = duration;
        }

        public void Restart()
        {
            elapse = 0;
        }

        public void Restart(float duration)
        {
            elapse = 0;
            this.duration = duration;
        }

        public void Update(float deltaTime)
        {
            elapse += deltaTime;
        }

        public Cooldown()
        { }

        public Cooldown(float duration)
        {
            this.duration = duration;
        }


        public static explicit operator Cooldown(float duration)
        {
            return new Cooldown(duration);
        }

        public static implicit operator bool(Cooldown cooldown)
        {
            return cooldown.isCooling;
        }
    }
}

