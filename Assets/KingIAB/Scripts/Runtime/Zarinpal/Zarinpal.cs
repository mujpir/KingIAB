using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KingKodeStudio.IAB.Zarinpal
{
    public class Zarinpal : IBillingPlatform
    {
        private readonly object _lockObject = new object();

        private static IQueryProvider m_queryPurchaseProvider;
        
        private static IZarinpalPlatform _platform;

        event Action IBillingPlatform.StoreInitialized
        {
            add
            {
                lock (_lockObject)
                {
                    StoreInitialized += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    StoreInitialized -= value;
                }
            }
        }

        event Action<string> IBillingPlatform.StoreInitializeFailed
        {
            add
            {
                lock (_lockObject)
                {
                    StoreInitializeFailed += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    StoreInitializeFailed -= value;
                }
            }
        }

        event Action<string,string> IBillingPlatform.PurchaseStarted
        {
            add
            {
                lock (_lockObject)
                {
                    PurchaseStarted += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    PurchaseStarted -= value;
                }
            }
        }

        event Action<string> IBillingPlatform.PurchaseFailedToStart
        {
            add
            {
                lock (_lockObject)
                {
                    PurchaseFailedToStart += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    PurchaseFailedToStart -= value;
                }
            }
        }

        event Action<Purchase> IBillingPlatform.PurchaseSucceed
        {
            add
            {
                lock (_lockObject)
                {
                        PurchaseSucceed += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    PurchaseSucceed -= value;
                }
            }
        }

        event Action<string> IBillingPlatform.PurchaseFailed
        {
            add
            {
                lock (_lockObject)
                {
                    PurchaseFailed += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    PurchaseFailed -= value;
                }
            }
        }

        /// <summary>
        /// occures when verification succeeded
        /// </summary>
        event Action<Purchase> IBillingPlatform.ConsumeSucceed
        {
            add
            {
                lock (_lockObject)
                {
                    PaymentVerificationSucceed += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    PaymentVerificationSucceed -= value;
                }
            }
        }
        
        /// <summary>
        /// Not supported in Zarinpal
        /// </summary>
        event Action<string> IBillingPlatform.ConsumeFailed
        {
            add
            {
                lock (_lockObject)
                {
                    PaymentVerificationFailed += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    PaymentVerificationFailed -= value;
                }
            }
        }

        public event Action<List<Purchase>, List<SkuInfo>> QueryInventorySucceeded;
        public event Action<string> QueryInventoryFailed;
        public event Action<List<SkuInfo>> QuerySkuDetailsSucceeded;
        public event Action<string> QuerySkuDetailsFailed;
        public event Action<List<Purchase>> QueryPurchasesSucceeded;
        public event Action<string> QueryPurchasesFailed;

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
        public static event Action<string> PurchaseFailedToStart;

        /// <summary>
        /// Occures when a purchase completed by user but still not verified.
        /// </summary>
        public static event Action<Purchase> PurchaseSucceed;

        /// <summary>
        /// Occures when a purchase failed.
        /// </summary>
        public static event Action<string> PurchaseFailed;

        /// <summary>
        /// Occures when zarinpal started to verify purchase
        /// </summary>
        public static event Action<string> PaymentVerificationStarted;

        /// <summary>
        /// Occures when payment verified by zarinpal and would be valid.You can award your user here
        /// </summary>
        public static event Action<Purchase> PaymentVerificationSucceed;

        /// <summary>
        /// Occures when payment verified by zarinpal and would NOT be valid or verification failed at first.
        /// </summary>
        public static event Action<string> PaymentVerificationFailed;

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


        public static ZarinpalPurchase PurchaseStatus
        {
            get
            {
                if (_platform == null)
                {
                    return null;
                }

                return _platform.PurchaseStatus;
            }
        }
        
        //interface implementation
        void IBillingPlatform.Initialize()
        {
            Initialize();
        }

        public static void SetQueryProvider(IQueryProvider provider)
        {
            m_queryPurchaseProvider = provider;
        }
        

        /// <summary>
        /// Initialize Zarinpal . Call this once is start up of your game.
        /// </summary>
        public static void Initialize()
        {
            if (m_queryPurchaseProvider == null)
            {
                m_queryPurchaseProvider = new DummyQueryProvider();
            }

            if (_platform != null)
            {
                if (Initialized)
                {
                    var message = "Zarinpal is already initialized.Please make sure you call 'Initialize' once.";
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

#if UNITY_EDITOR
            _platform = new ZarinpalEditor();
#elif UNITY_IOS
        _platform = ZarinpaliOS.CreateInstance();
#elif UNITY_ANDROID
        _platform = ZarinpalAndroid.CreateInstance();
#endif

            //Subscribing events
            _platform.StoreInitialized += OnStoreInitialized;
            _platform.PurchaseStarted += OnPurchaseStarted;
            _platform.PurchaseFailedToStart += OnPurchaseFailedToStart;
            _platform.PurchaseSucceed += OnPurchaseSucceed;
            _platform.PurchaseFailed += OnPurchaseFailed;
            _platform.PurchaseCanceled += OnPurchaseCanceled;
            _platform.PaymentVerificationStarted += OnPaymentVerificationStarted;
            _platform.PaymentVerificationSucceed += OnPaymentVerificationSucceed;
            _platform.PaymentVerificationFailed += OnPaymentVerificationFailed;


            if (Initialized)
            {
                var message = "Zarinpal is already initialized.Please make sure you call 'Initialize' once.";
                OnStoreInitializeFailed(message);
                KKLog.LogWarning(message);
                return;
            }

            var setting = KingIAB.Setting;

            if (setting == null)
            {
                var message =
                    "Could not find zarinpal config file.Make sure you have setup zarinpal setting in KingIAB/Setting";
                OnStoreInitializeFailed(message);
                KKLog.LogWarning(message);
                return;
            }

            if (string.IsNullOrEmpty(setting.MerchantID) || setting.MerchantID == "MY_ZARINPAL_MERCHANT_ID")
            {
                var message = "Invalid MerchantID.Please go to menu : KingIAB/Setting to set a valid merchant id";
                OnStoreInitializeFailed(message);
                KKLog.LogWarning(message);
                return;
            }

            var scheme = setting.Scheme;
            var host = setting.Host;

#if !UNITY_EDITOR && UNITY_ANDROID
        if (string.IsNullOrEmpty(setting.Scheme) || string.IsNullOrEmpty(setting.Host)
            || setting.Scheme=="MY_SCHEME" || setting.Host=="MY_HOST")
        {
            var message =
 "Scheme or Host Can not be null or Empty.Please go to menu : Zarinpal/Setting to set a valid Scheme and Host";
            OnStoreInitializeFailed(message);
            KKLog.LogWarning(message);
            return;
        }
#else
            scheme = string.Empty;
            host = string.Empty;
#endif

            if (_platform == null)
            {
                var message = "Platform is not supported";
                OnStoreInitializeFailed(message);
                KKLog.LogError(message);
                return;
            }

            _platform.Initialize(setting.MerchantID, setting.AutoVerifyPurchase,setting.CallbackUrl,setting.AutoStartPurchase);
        }

        void IBillingPlatform.Purchase(long amount,string desc,string sku)
        {
            Purchase(amount, desc, sku);
        }

        /// <summary>
        /// Provider authority to verify purchase
        /// </summary>
        /// <param name="authority"></param>
        public void Consume(string authority,int amount)
        {
            _platform.VerifyPurchase(authority,amount);
        }

        public bool IsInitialized
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

        public IZarinpalPlatform Platform
        {
            get { return _platform; }
        }

        public void QueryInventory(string[] products)
        {
            OnQueryInventorySucceeded(new List<Purchase>(), new List<SkuInfo>());
        }

        public void QuerySkuDetails(string[] products)
        {
            OnQuerySkuDetailsSucceeded(new List<SkuInfo>());
        }

        public void QueryPurchases()
        {            
            m_queryPurchaseProvider.QueryPurchases(OnQueryPurchasesSucceeded, OnQueryPurchasesFailed);
        }


        /// <summary>
        /// Start a zarinpal purchase
        /// </summary>
        /// <param name="amount">your product/service price in toman</param>
        /// <param name="desc">payment description.please note it can not be null or empty</param>
        /// <param name="productID">the id of product you are purchasing</param>
        public static void Purchase(long amount, string desc, string productID = "na")
        {
            if (amount < 100)
            {
                var message = "Purchase is not valid.Amount can not be less than 100 toman.";
                OnPurchaseFailedToStart(message);
                KKLog.LogError(message);
                return;
            }

            if (string.IsNullOrEmpty(desc))
            {
                var message =
                    "Purchase is not valid.Description can not be null or empty .Please provide a valid description";
                OnPurchaseFailedToStart(message);
                KKLog.LogError(message);
                return;
            }

            if (_platform == null || !_platform.IsInitialized)
            {
                var message =
                    "Purchase is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
                OnPurchaseFailedToStart(message);
                KKLog.LogError(message);
                return;
            }

            if (string.IsNullOrEmpty(productID))
            {
                productID = "unknown product";
            }

            _platform.Purchase(amount, desc, productID);
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

        protected static void OnPurchaseStarted(string productCode,string authority)
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

        protected static void OnPurchaseCanceled()
        {
            var handler = PurchaseFailed;
            if (handler != null) handler("User canceled the purchase.");
        }

        protected static void OnPaymentVerificationStarted(string obj)
        {
            var handler = PaymentVerificationStarted;
            if (handler != null) handler(obj);
        }

        protected static void OnPaymentVerificationSucceed(string obj)
        {
            var purchase = new Purchase(PurchaseStatus.ProductID, PurchaseStatus.RefID);
            purchase.SetSignature(PurchaseStatus.Authority);
            var handler = PaymentVerificationSucceed;
            if (handler != null) handler(purchase);
        }

        protected static void OnPaymentVerificationFailed(string error)
        {
            var handler = PaymentVerificationFailed;
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

        #endregion


    }
}
