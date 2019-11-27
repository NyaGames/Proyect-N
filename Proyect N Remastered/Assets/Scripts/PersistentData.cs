using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentData
{
    public static Model_Account account = new Model_Account("Dani","0000","123",true, System.DateTime.Now);
    public static bool isGM = true;
}
