using System.Collections;
using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static void RunAfter(this MonoBehaviour b, float delay, System.Action func)
        {
            IEnumerator AuxFunc()
            {
                yield return new WaitForSeconds(delay);
                func();
            }

            b.StartCoroutine(AuxFunc());
        }

        public static bool RandomBool(float probability)
        {
            return Random.Range(0f, 1f) < probability;
        }
    }
}