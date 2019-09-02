
using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
namespace KingKodeStudio.IAB
{
    public class GooglePlayIAB : IBillingPlatform
    {
        private static AndroidJavaObject mPlugin;
        private static GooglePlayIAB _instance;

        public bool IsInitialized { get; private set; }

        public event Action StoreInitialized
        {
            add { IABEventManager.billingSupportedEvent += value; }
            remove { IABEventManager.billingSupportedEvent -= value; }
        }
        
        public event Action<string> StoreInitializeFailed
        {
            add { IABEventManager.billingNotSupportedEvent += value; }
            remove { IABEventManager.billingNotSupportedEvent -= value; }
        }
        public event Action<string,string> PurchaseStarted;
        public event Action<string> PurchaseFailedToStart;
        public event Action<Purchase> PurchaseSucceed
        {
            add { IABEventManager.purchaseSucceededEvent += value; }
            remove { IABEventManager.purchaseSucceededEvent -= value; }
        }
        public event Action<string> PurchaseFailed
        {
            add { IABEventManager.purchaseFailedEvent += value; }
            remove { IABEventManager.purchaseFailedEvent -= value; }
        }
        public event Action<Purchase> ConsumeSucceed
        {
            add { IABEventManager.consumePurchaseSucceededEvent += value; }
            remove { IABEventManager.consumePurchaseSucceededEvent -= value; }
        }
        public event Action<string> ConsumeFailed
        {
            add { IABEventManager.consumePurchaseFailedEvent += value; }
            remove { IABEventManager.consumePurchaseFailedEvent -= value; }
        }
        public event Action<List<Purchase>, List<SkuInfo>> QueryInventorySucceeded
        {
            add { IABEventManager.queryInventorySucceededEvent += value; }
            remove { IABEventManager.queryInventorySucceededEvent -= value; }
        }
        public event Action<string> QueryInventoryFailed
        {
            add { IABEventManager.queryInventoryFailedEvent += value; }
            remove { IABEventManager.queryInventoryFailedEvent -= value; }
        }
        public event Action<List<SkuInfo>> QuerySkuDetailsSucceeded
        {
            add { IABEventManager.querySkuDetailsSucceededEvent += value; }
            remove { IABEventManager.querySkuDetailsSucceededEvent -= value; }
        }
        public event Action<string> QuerySkuDetailsFailed
        {
            add { IABEventManager.querySkuDetailsFailedEvent += value; }
            remove { IABEventManager.querySkuDetailsFailedEvent -= value; }
        }
        public event Action<List<Purchase>> QueryPurchasesSucceeded
        {
            add { IABEventManager.queryPurchasesSucceededEvent += value; }
            remove { IABEventManager.queryPurchasesSucceededEvent -= value; }
        }
        public event Action<string> QueryPurchasesFailed
        {
            add { IABEventManager.queryPurchasesFailedEvent += value; }
            remove { IABEventManager.queryPurchasesFailedEvent -= value; }
        }

        public static GooglePlayIAB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GooglePlayIAB();
                }

                return _instance;
            }
        }

        public static string GetVersion()
        {
            return "1.0.4";
        }

        static GooglePlayIAB()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            // Get the plugin instance
            using (var pluginClass = new AndroidJavaClass("com.bazaar.BazaarIABPlugin"))
                mPlugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
        }

        // Toggles high detail logging on/off
        public static void enableLogging(bool enable)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            if (enable)
                Debug.LogWarning("YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!");

            mPlugin.Call("enableLogging", enable);
        }


        // Initializes the billing system
        public static void init()
        {
#if UNITY_EDITOR
            Instance.OnStoreInitialized();
#elif UNITY_ANDROID
            var iabSetting = KingIAB.Setting;
            MarketType marketType = MarketType.None;
            var publicKey = "";
            
            if (iabSetting.IsGooglePlay)
            {
                marketType = MarketType.GooglePlay;
                publicKey = iabSetting.GooglePlay64Key;
            }
            if (iabSetting.IsBazaar)
            {
                marketType = MarketType.Bazaar;
                publicKey = iabSetting.Bazaar64Key;
            }
            if (iabSetting.IsMyket)
            {
                marketType = MarketType.Myket;
                publicKey = iabSetting.Myket64Key;
            }
            if (iabSetting.IsIranApps)
            {
                marketType = MarketType.Iranapps;
                publicKey = iabSetting.Iranapps64Key;
            }

            if (marketType == MarketType.None)
            {
                //var error = "You choose 'None' as selected market . Please choose a valid market in KingIAB/Setting menu otherwise don't call this method!";
                return;
            }
            
            if (string.IsNullOrEmpty(publicKey))
            {
                //var error = "public key is null , Please provide a valid public key!";
                return;
            }
            
            mPlugin.Call("init",publicKey, (int) marketType);

#else
            var error = "GooglePlay-like markets is not supported on iOS . Consider using zarinpal or appstore instead when building for iOS.You can configure this using KingIAB/Setting menu.";
            Instance.OnStoreInitializeFailed(error);
            return;
#endif
        }


        // Unbinds and shuts down the billing service
        public static void unbindService()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            mPlugin.Call("unbindService");
        }


        // Returns whether subscriptions are supported on the current device
        public static bool areSubscriptionsSupported()
        {
            if (Application.platform != RuntimePlatform.Android)
                return false;

            return mPlugin.Call<bool>("areSubscriptionsSupported");
        }
        
        public void QueryInventory(string[] products)
        {
            queryInventory(products);
        }


        // Sends a request to get all completed purchases and product information as setup in the CafeBazaar portal about the provided skus
        public static void queryInventory(string[] skus)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            
            mPlugin.Call("queryInventory", new object[] { skus });
        }
        
        
        public void QuerySkuDetails(string[] products)
        {
            querySkuDetails(products);
        }

        // Sends a request to get all product information as setup in the CafeBazaar portal about the provided skus
        public static void querySkuDetails(string[] skus)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            
            mPlugin.Call("querySkuDetails", new object[] { skus });
        }
        
        
        public void QueryPurchases()
        {
            queryPurchases();
        }

        // Sends a request to get all completed purchases
        public static void queryPurchases()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            
            mPlugin.Call("queryPurchases");
        }


        // Sends out a request to purchase the product
        public static void purchaseProduct(string sku)
        {
            purchaseProduct(sku, string.Empty);
        }

        public static void purchaseProduct(string sku, string developerPayload)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            
            mPlugin.Call("purchaseProduct", sku, developerPayload);
        }


        // Sends out a request to consume the product
        public static void consumeProduct(string sku)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            
            mPlugin.Call("consumeProduct", sku);
        }


        // Sends out a request to consume all of the provided products
        public static void consumeProducts(string[] skus)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            mPlugin.Call("consumeProducts", new object[] { skus });
        }

        public void Initialize()
        {
            init();
        }

        public void Purchase(long amount, string desc, string sku)
        {
            purchaseProduct(sku);
        }

        public void Consume(string sku,int amount=0)
        {
            consumeProduct(sku);
        }

        
        #region CallBacks
        //These are callbacks that called from IABEventManager object ,but not from plugin or android itself
        public virtual void OnStoreInitialized()
        {
            IsInitialized = true;
        }
        
        #endregion

        protected virtual void OnPurchaseStarted(string productCode,string authority)
        {
            var handler = PurchaseStarted;
            if (handler != null) handler(productCode,authority);
        }

        protected virtual void OnPurchaseFailedToStart(string obj)
        {
            var handler = PurchaseFailedToStart;
            if (handler != null) handler(obj);
        }
    }
}

#endif
