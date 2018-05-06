using UnityEngine;

namespace Completed
{
    public static class InputWrapper
    {
        private static bool testMode = false;
        private static KeyCode key;

        public static bool GetKey(KeyCode kc)
        {
            if (!testMode)
            {
                return Input.GetKey(kc);
            }
            return kc.CompareTo(key) == 0;
        }

        public static void SetKey(KeyCode kc)
        {
            testMode = true;
            key = kc;
        }
    }
}