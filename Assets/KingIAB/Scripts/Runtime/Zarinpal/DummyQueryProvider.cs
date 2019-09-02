using System;
using System.Collections;
using System.Collections.Generic;
using KingKodeStudio.IAB;
using UnityEngine;

public class DummyQueryProvider : IQueryProvider 
{
    public void QueryPurchases(Action<List<Purchase>> succeedAction, Action<string> failedAction)
    {
        if (succeedAction != null)
        {
            succeedAction(new List<Purchase>());
        }
    }
}
