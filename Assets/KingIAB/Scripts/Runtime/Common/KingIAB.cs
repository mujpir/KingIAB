using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingKodeStudio.IAB
{
    public class KingIAB
    {
        private static IBillingPlatform _platform;
        private static IABConfig m_setting;

        /// <summary>
        /// Occures when store is successfully initialized
        /// </summary>
        public static event Action StoreInitialized;

        /// <summary>
        /// Occures when store failed to initialized
        /// </summary>
        public static event Action<string> StoreInitializeFailed;

        /// <summary>
        /// Occures when zarinpal initiaate a purchase
        /// </summary>
        public static event Action<string,string> PurchaseStarted;

        /// <summary>
        /// Occures when zarinpal can not start a purchase flow.It may be caused by invalid merchant id or unavailabilty of zarinpal service.
        /// </summary>
        public static event PurchaseFailedDelegate PurchaseFailedToStart;

        /// <summary>
        /// Occures when a purchase completed by user but still not verified.
        /// </summary>
        public static event PurchaseSucceedDelegate PurchaseSucceed;

        /// <summary>
        /// Occures when a purchase failed.
        /// </summary>
        public static event PurchaseFailedDelegate PurchaseFailed;
        
        
        /// <summary>
        /// Occures when a purchase failed.
        /// </summary>
        public static event ConsumeSucceedDelegate ConsumeSucceed;
        
        
        /// <summary>
        /// Occures when a purchase failed.
        /// </summary>
        public static event ConsumeFailedDelegate ConsumeFailed;
        
        
        public static event QueryInventorySucceededDelegate QueryInventorySucceeded;
        public static event QueryInventoryFailedDelegate QueryInventoryFailed;
        public static event QuerySkuDetailsSucceededDelegate QuerySkuDetailsSucceeded;
        public static event QuerySkuDetailsFailedDelegate QuerySkuDetailsFailed;
        public static event QueryPurchasesSucceedDelegate QueryPurchasesSucceeded;
        public static event QueryPurchasesFailedDelegate QueryPurchasesFailed;

        public static bool Initialized
        {
            get
            {
                if (_platform == null)
                {
                    return false;
                }

                return _platform.IsInitialized;
            }
        }

        public static IABConfig Setting
        {
            get
            {
                if (m_setting == null)
                {
                    m_setting = Resources.Load<IABConfig>("IABSetting");
                    if (m_setting == null)
                    {
                        KKLog.LogError("'IABSetting' not found at Resources.Reimport plugin to fix this problem");
                    }
                }

                return m_setting;
            }
        }

        public static IBillingPlatform BillingPlatform 
        {
            get { return _platform; }
        }

        public static BillingPlatformName BillingPlatformName
        {
            get
            {
                if (_platform == null)
                    return BillingPlatformName.None;
                else if (_platform is Zarinpal.Zarinpal)
                {
                    return BillingPlatformName.Zarinpal;
                }
                else if(_platform is DummyBillingPlatform)
                {
                    return BillingPlatformName.None;
                }
                else
                {
                    if (Setting.IsGooglePlay) return BillingPlatformName.GooglePlay;
                    if (Setting.IsBazaar) return BillingPlatformName.Bazaar;
                    if (Setting.IsMyket) return BillingPlatformName.Myket;
                    if (Setting.IsIranApps) return BillingPlatformName.IranApps;
                }

                return BillingPlatformName.None;
            }
        }
        
        

        /// <summary>
        /// Initialize Zarinpal . Call this once is start up of your game.
        /// </summary>
        public static void Initialize(bool forceZarinpal = false)
        {
            if (_platform != null)
            {
                if (Initialized)
                {
                    var message = "KingIAB is already initialized.Please make sure you call 'Initialize' once.";
                    OnStoreInitializeFailed(message);
                    KKLog.LogWarning(message);
                }
                else
                {
                    var message =
                        "Platform has been created but not initialized . There may be an error. Please see logs for more details";
                    OnStoreInitializeFailed(message);
                    KKLog.LogError(message);
                }

                return;
            }
            


            if (Setting == null)
            {
                var message =
                    "Could not find IABSetting file.Make sure you have setup the plugin. Use KingIAB/Setting menu to setup the plugin";
                OnStoreInitializeFailed(message);
                KKLog.LogWarning(message);
                return;
            }

            
            if(Setting.IsAndroidZarinpal || forceZarinpal)
                    _platform = new Zarinpal.Zarinpal();
            else if (Setting.IsGooglePlay || Setting.IsBazaar || Setting.IsMyket || Setting.IsIranApps)
            {
                _platform = GooglePlayIAB.Instance;
                //AbstractManager.initialize(typeof(IABEventManager));
            }
            else
            {
                _platform = new DummyBillingPlatform();
                var warning =
                    "It seems you have not selected any market or have not setup the plugin.Plugin will use dummy implementation in this case.Use KingIAB/Setting menu to setup the plugin first!";
                KKLog.LogWarning(warning);
            
                return;
            }

            //Subscribing events
            _platform.StoreInitialized += OnStoreInitialized;
            _platform.StoreInitializeFailed += OnStoreInitializeFailed;
            _platform.PurchaseStarted += OnPurchaseStarted;
            _platform.PurchaseFailedToStart += OnPurchaseFailedToStart;
            _platform.PurchaseSucceed += OnPurchaseSucceed;
            _platform.PurchaseFailed += OnPurchaseFailed;
            _platform.ConsumeSucceed += OnConsumeSucceed;
            _platform.ConsumeFailed += OnConsumeFailed;
            _platform.QueryInventorySucceeded += OnQueryInventorySucceeded;
            _platform.QueryInventoryFailed += OnQueryInventoryFailed;
            _platform.QuerySkuDetailsSucceeded += OnQuerySkuDetailsSucceeded;
            _platform.QuerySkuDetailsFailed += OnQuerySkuDetailsFailed;
            _platform.QueryPurchasesSucceeded += OnQueryPurchasesSucceeded;
            _platform.QueryPurchasesFailed += OnQueryPurchasesFailed;


            if (Initialized)
            {
                var message = "KingIAB is already initialized.Please make sure you call 'Initialize' once.";
                OnStoreInitializeFailed(message);
                KKLog.LogWarning(message);
                return;
            }

            if (_platform == null)
            {
                var message = "Platform is not supported";
                OnStoreInitializeFailed(message);
                KKLog.LogError(message);
                return;
            }

            _platform.Initialize();
        }

        /// <summary>
        /// Start a purchase flow . 
        /// </summary>
        /// <param name="amount">your product/service price in toman</param>
        /// <param name="desc">payment description.please note it can not be null or empty when using zarinpal.</param>
        /// <param name="productID">the id of product you are purchasing</param>
        public static void Purchase(string productID,long amount, string desc)
        {
            if (!Initialized)
            {
                var message =
                    "Purchase is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
                OnPurchaseFailedToStart(message);
                KKLog.LogError(message);
                return;
            }

            _platform.Purchase(amount, desc, productID);
        }
        
        
        
        /// <summary>
        /// Consume the product that is purchased
        /// </summary>
        /// <param name="productID">ID of product to consume</param>
        public static void Consume(string productID,string authority,int amount)
        {
            if (!Initialized)
            {
                var message =
                    "Consume is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
                KKLog.LogError(message);
                return;
            }

            if (_platform is Zarinpal.Zarinpal)
            {
                _platform.Consume(authority,amount);
            }
            else
            {
                _platform.Consume(productID,amount);
            }
        }



        /// <summary>
        ///  Sends a request to get all completed purchases and product information
        /// </summary>
        /// <param name="skus">List of products to get information about it</param>
        public static void QueryInventory(string[] skus)
        {
            if (!Initialized)
            {
                var message =
                    "QueryInventory is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
                KKLog.LogError(message);
                return;
            }

            _platform.QueryInventory(skus);
        }



        /// <summary>
        /// Sends a request to get all completed purchases
        /// </summary>
        public static void QueryPurchases()
        {
            if (!Initialized)
            {
                var message =
                    "QueryPurchases is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
                KKLog.LogError(message);
                return;
            }

            _platform.QueryPurchases();
        }


        /// <summary>
        /// Sends a request to get all product information as setup in the CafeBazaar portal about the provided skus
        /// </summary>
        /// <param name="skus">List of products to get information about it</param>
        public static void QuerySkuDetails(string[] skus)
        {
            if (!Initialized)
            {
                var message =
                    "QuerySkuDetails is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
                KKLog.LogError(message);
                return;
            }

            _platform.QuerySkuDetails(skus);
        }



        #region Callbacks

        protected static void OnStoreInitialized()
        {
            var handler = StoreInitialized;
            if (handler != null) handler();
        }

        private static void OnStoreInitializeFailed(string error)
        {
            var handler = StoreInitializeFailed;
            if (handler != null) handler(error);
        }

        protected static void OnPurchaseStarted(string productCode, string authority)
        {
            var handler = PurchaseStarted;
            if (handler != null) handler(productCode,authority);
        }

        protected static void OnPurchaseFailedToStart(string message)
        {
            var handler = PurchaseFailedToStart;
            if (handler != null) handler(message);
        }

        protected static void OnPurchaseSucceed(Purchase purchase)
        {
            var handler = PurchaseSucceed;
            if (handler != null) handler(purchase);
        }

        protected static void OnPurchaseFailed(string error)
        {
            var handler = PurchaseFailed;
            if (handler != null) handler(error);
        }
        
        protected static void OnConsumeFailed(string error)
        {
            var handler = ConsumeFailed;
            if (handler != null) handler(error);
        }

        protected static void OnConsumeSucceed(Purchase purchase)
        {
            var handler = ConsumeSucceed;
            if (handler != null) handler(purchase);
        }

        #endregion

        private static void OnQueryInventorySucceeded(List<Purchase> purchases, List<SkuInfo> skuinfos)
        {
            var handler = QueryInventorySucceeded;
            if (handler != null) handler(purchases, skuinfos);
        }

        private static void OnQueryInventoryFailed(string error)
        {
            var handler = QueryInventoryFailed;
            if (handler != null) handler(error);
        }

        private static void OnQuerySkuDetailsSucceeded(List<SkuInfo> skuinfos)
        {
            var handler = QuerySkuDetailsSucceeded;
            if (handler != null) handler(skuinfos);
        }

        private static void OnQuerySkuDetailsFailed(string error)
        {
            var handler = QuerySkuDetailsFailed;
            if (handler != null) handler(error);
        }

        private static void OnQueryPurchasesSucceeded(List<Purchase> purchases)
        {
            var handler = QueryPurchasesSucceeded;
            if (handler != null) handler(purchases);
        }

        private static void OnQueryPurchasesFailed(string error)
        {
            var handler = QueryPurchasesFailed;
            if (handler != null) handler(error);
        }
    }
}
