using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

#if UNITY_ANDROID

namespace KingKodeStudio.IAB
{
    public class IABEventManager : AbstractManager
    {
        // Fired after init is called when billing is supported on the device
        public static event Action billingSupportedEvent;

        // Fired after init is called when billing is not supported on the device
        public static event Action<string> billingNotSupportedEvent;

        // Fired when the inventory and purchase history query has returned
        public static event Action<List<Purchase>,List<SkuInfo>> queryInventorySucceededEvent;

        // Fired when the inventory and purchase history query fails
        public static event Action<string> queryInventoryFailedEvent;

        // Fired when the SkuDetails query has returned
        public static event Action<List<SkuInfo>> querySkuDetailsSucceededEvent;

        // Fired when the SkuDetails query fails
        public static event Action<string> querySkuDetailsFailedEvent;

        // Fired when the purchase history query has returned
        public static event Action<List<Purchase>> queryPurchasesSucceededEvent;

        // Fired when the purchase history query fails
        public static event Action<string> queryPurchasesFailedEvent;

        // Fired when a purchase succeeds
        public static event Action<Purchase> purchaseSucceededEvent;

        // Fired when a purchase fails
        public static event Action<string> purchaseFailedEvent;

        // Fired when a call to consume a product succeeds
        public static event Action<Purchase> consumePurchaseSucceededEvent;

        // Fired when a call to consume a product fails
        public static event Action<string> consumePurchaseFailedEvent;

        public static IABEventManager Instance;


        static IABEventManager()
        {
            Instance = (IABEventManager) initialize(typeof(IABEventManager));
        }

        public void billingSupported(string empty)
        {
            GooglePlayIAB.Instance.OnStoreInitialized();
            billingSupportedEvent.SafeInvoke();
        }

        public void billingNotSupported(string error)
        {
            billingNotSupportedEvent.SafeInvoke(error);
        }

        public void queryInventorySucceeded(string jsonStr)
        {
            JSONNode dataNode = JSON.Parse(jsonStr);

            JSONArray purchasesJsonArray = dataNode["purchases"].AsArray;
            var purchases = Purchase.fromJsonArray(purchasesJsonArray);

            JSONArray skusJsonArray = dataNode["skus"].AsArray;
            var skus = SkuInfo.fromJsonArray(skusJsonArray);

            queryInventorySucceededEvent.SafeInvoke(purchases, skus);
        }

        public void queryInventoryFailed(string error)
        {
            queryInventoryFailedEvent.SafeInvoke(error);
        }

        public void querySkuDetailsSucceeded(string jsonStr)
        {
            JSONNode dataNode = JSON.Parse(jsonStr);

            JSONArray skusJsonArray = dataNode.AsArray;
            var skus = SkuInfo.fromJsonArray(skusJsonArray);

            querySkuDetailsSucceededEvent.SafeInvoke(skus);
        }

        public void querySkuDetailsFailed(string error)
        {
            querySkuDetailsFailedEvent.SafeInvoke(error);
        }

        public void queryPurchasesSucceeded(string jsonStr)
        {
            JSONNode dataNode = JSON.Parse(jsonStr);

            JSONArray purchasesJsonArray = dataNode.AsArray;
            var purchases = Purchase.fromJsonArray(purchasesJsonArray);

            queryPurchasesSucceededEvent.SafeInvoke(purchases);
        }

        public void queryPurchasesFailed(string error)
        {
            queryPurchasesFailedEvent.SafeInvoke(error);
        }

        public void purchaseSucceeded(string jsonStr)
        {
            JSONNode dataNode = JSON.Parse(jsonStr);
            Purchase bazaarPurchase = new Purchase();
            bazaarPurchase.fromJson(dataNode.AsObject);

            purchaseSucceededEvent.SafeInvoke(bazaarPurchase);
        }

        public void purchaseFailed(string error)
        {
            purchaseFailedEvent.SafeInvoke(error);
        }

        public void consumePurchaseSucceeded(string jsonStr)
        {
            JSONNode dataNode = JSON.Parse(jsonStr);
            Purchase bazaarPurchase = new Purchase();
            bazaarPurchase.fromJson(dataNode.AsObject);

            consumePurchaseSucceededEvent.SafeInvoke(bazaarPurchase);
        }

        public void consumePurchaseFailed(string error)
        {
            consumePurchaseFailedEvent.SafeInvoke(error);
        }

    }
}

#endif
