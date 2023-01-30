using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class InputManager
{
    private static InputMaster instance;

    public static InputMaster GetMaster()
    {
        if (instance == null) instance = new InputMaster();
        return instance;
    }

}
