using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceRollLibrary
{
    public static class DiceRoller
    {
        public static int RollAD20()
        {
            return Random.Range(1, 21);
        }

        public static int RollAD10()
        {
            return Random.Range(1, 11);
        }

        public static int RollAD12()
        {
            return Random.Range(1, 13);
        }

        public static int RollAD8()
        {
            return Random.Range(1, 9);
        }

        public static int RollAD100()
        {
            return Random.Range(1, 101);
        }

        public static int RollAD6()
        {
            return Random.Range(1, 7);
        }

        public static int RollAD4()
        {
            return Random.Range(1, 5);
        }

        public static int RollAD3()
        {
            return Random.Range(1, 4);
        }

        public static int RollAD2()
        {
            return Random.Range(1, 3);
        }
    }
    

}
