using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingKodeStudio.IAB
{
    public interface IBillingPlatform
    {
        void Initialize();

        void Purchase(long amount,string desc,string sku);
        void Consume(string sku,int amount);
        bool IsInitialized { get; }
        void QueryInventory(string[] products);
        void QuerySkuDetails(string[] products);
        void QueryPurchases();

        event Action StoreInitialized;
        event Action<string> StoreInitializeFailed;
        event Action<string,string> PurchaseStarted;
        event Action<string> PurchaseFailedToStart;
        event Action<Purchase> PurchaseSucceed;
        event Action<string> PurchaseFailed;
        event Action<Purchase> ConsumeSucceed;
        event Action<string> ConsumeFailed;
        event Action<List<Purchase>,List<SkuInfo>> QueryInventorySucceeded;
        event Action<string> QueryInventoryFailed;
        event Action<List<SkuInfo>> QuerySkuDetailsSucceeded;
        event Action<string> QuerySkuDetailsFailed;
        event Action<List<Purchase>> QueryPurchasesSucceeded;
        event Action<string> QueryPurchasesFailed;

    }
}
