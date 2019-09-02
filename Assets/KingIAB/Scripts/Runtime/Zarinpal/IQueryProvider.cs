using System;
using System.Collections;
using System.Collections.Generic;
using KingKodeStudio.IAB;
using UnityEngine;

public interface IQueryProvider
{
    void QueryPurchases(Action<List<Purchase>> succeedAction, Action<string> failedAction);
}
