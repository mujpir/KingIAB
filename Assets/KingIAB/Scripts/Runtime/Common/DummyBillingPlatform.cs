using System;
using System.Collections;
using System.Collections.Generic;
using KingKodeStudio.IAB;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyBillingPlatform : IBillingPlatform 
{
    public void Initialize()
    {
        OnStoreInitialized();
    }

    public void Purchase(long amount, string desc, string sku)
    {
        OnPurchaseStarted(sku);
        var purchase = new Purchase(sku, "fake_order_id_" + Random.Range(1000, 9999));
        OnPurchaseSucceed(purchase);
    }

    public void Consume(string sku,int amount)
    {
        OnConsumeSucceed(sku, "fake_order_id_" + Random.Range(1000, 9999));
    }

    public bool IsInitialized { get; private set; }
    public void QueryInventory(string[] products)
    {
        OnQueryInventorySucceeded(new List<Purchase>(),new List<SkuInfo>() );
    }

    public void QuerySkuDetails(string[] products)
    {
        OnQuerySkuDetailsSucceeded(new List<SkuInfo>());
    }

    public void QueryPurchases()
    {
        
    }

    public event Action StoreInitialized;
    public event Action<string> StoreInitializeFailed;
    public event Action<string,string> PurchaseStarted;
    public event Action<string> PurchaseFailedToStart;
    public event Action<Purchase> PurchaseSucceed;
    public event Action<string> PurchaseFailed;
    public event Action<Purchase> ConsumeSucceed;
    public event Action<string> ConsumeFailed;
    public event Action<List<Purchase>, List<SkuInfo>> QueryInventorySucceeded;
    public event Action<string> QueryInventoryFailed;
    public event Action<List<SkuInfo>> QuerySkuDetailsSucceeded;
    public event Action<string> QuerySkuDetailsFailed;
    public event Action<List<Purchase>> QueryPurchasesSucceeded;
    public event Action<string> QueryPurchasesFailed;

    protected virtual void OnStoreInitialized()
    {
        IsInitialized = true;
        var handler = StoreInitialized;
        if (handler != null) handler();
    }

    protected virtual void OnPurchaseStarted(string productCode)
    {
        var handler = PurchaseStarted;
        if (handler != null) handler(productCode,null);
    }

    protected virtual void OnPurchaseFailedToStart(string error)
    {
        var handler = PurchaseFailedToStart;
        if (handler != null) handler(error);
    }

    protected virtual void OnPurchaseSucceed(Purchase purchase)
    {
        var handler = PurchaseSucceed;
        if (handler != null) handler(purchase);
    }

    protected virtual void OnPurchaseFailed(string error)
    {
        var handler = PurchaseFailed;
        if (handler != null) handler(error);
    }

    protected virtual void OnConsumeSucceed(string productid, string orderid)
    {
        var handler = ConsumeSucceed;
        var purchase = new Purchase(productid, orderid);
        if (handler != null) handler(purchase);
    }

    protected virtual void OnConsumeFailed(string error)
    {
        var handler = ConsumeFailed;
        if (handler != null) handler(error);
    }

    protected virtual void OnStoreInitializeFailed(string error)
    {
        var handler = StoreInitializeFailed;
        if (handler != null) handler(error);
    }

    protected virtual void OnQueryInventorySucceeded(List<Purchase> purchases, List<SkuInfo> skuinfos)
    {
        var handler = QueryInventorySucceeded;
        if (handler != null) handler(purchases, skuinfos);
    }

    protected virtual void OnQueryInventoryFailed(string error)
    {
        var handler = QueryInventoryFailed;
        if (handler != null) handler(error);
    }

    protected virtual void OnQuerySkuDetailsSucceeded(List<SkuInfo> skuinfos)
    {
        var handler = QuerySkuDetailsSucceeded;
        if (handler != null) handler(skuinfos);
    }

    protected virtual void OnQuerySkuDetailsFailed(string error)
    {
        var handler = QuerySkuDetailsFailed;
        if (handler != null) handler(error);
    }

    protected virtual void OnQueryPurchasesSucceeded(List<Purchase> purchases)
    {
        var handler = QueryPurchasesSucceeded;
        if (handler != null) handler(purchases);
    }

    protected virtual void OnQueryPurchasesFailed(string error)
    {
        var handler = QueryPurchasesFailed;
        if (handler != null) handler(error);
    }
}
