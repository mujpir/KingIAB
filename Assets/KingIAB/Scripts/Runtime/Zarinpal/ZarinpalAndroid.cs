using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingKodeStudio.IAB.Zarinpal
{
    public class ZarinpalAndroid : MonoBehaviour, IZarinpalPlatform
    {
        private AndroidJavaClass _zarinpalJavaClass;
        private AndroidJavaObject _zarinpalJavaObject;
        private static ZarinpalAndroid _instance;

        public static ZarinpalAndroid CreateInstance()
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new GameObject("ZarinpalAndroid").AddComponent<ZarinpalAndroid>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }

            return _instance;
        }

        public string MerchantID { get; private set; }
        public bool AutoVerifyPurchase { get; private set; }
        public string Callback { get; private set; }
        public bool IsInitialized { get; private set; }

        private bool m_purchaseOpen;

        private string m_productID;

        private string m_authority;

        private string m_refID;

        private Guid _transactionID;

        private ZarinpalPurchase.PurchaseStatus m_status;

        public ZarinpalPurchase PurchaseStatus
        {
            get
            {
                return new ZarinpalPurchase(_transactionID.ToString(),
                    m_authority, m_refID, m_productID, m_status);
            }
        }

        public void Initialize(string merchantID, bool verifyPurchase, string schemeCallback,bool autoStartPurchase)
        {
            MerchantID = merchantID;
            AutoVerifyPurchase = verifyPurchase;
            Callback = schemeCallback;
            _zarinpalJavaClass = new AndroidJavaClass("com.kingcodestudio.unityzarinpaliab.ZarinpalActivity");
            _zarinpalJavaClass.CallStatic("initialize", merchantID, verifyPurchase, schemeCallback,autoStartPurchase);
        }

        public void Purchase(long amount, string desc, string productID)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.None;
            m_purchaseOpen = true;
            m_authority = null;
            _transactionID = Guid.NewGuid();
            m_productID = productID;
            m_refID = null;
            _zarinpalJavaClass.CallStatic("startPurchaseFlow", amount, productID, desc);
        }

        public void VerifyPurchase(string authority,int amount)
        {
            m_authority = authority;
            _zarinpalJavaClass.CallStatic("verifyPurchase", authority,amount);

        }

        public event Action StoreInitialized;
        public event Action<string,string> PurchaseStarted;
        public event PurchaseFailedToStartDelegate PurchaseFailedToStart;
        public event PurchaseSucceedDelegate PurchaseSucceed;
        public event PurchaseFailedDelegate PurchaseFailed;
        public event Action PurchaseCanceled;
        public event Action<string> PaymentVerificationStarted;
        public event Action<string> PaymentVerificationSucceed;
        public event Action<string> PaymentVerificationFailed;



        #region Callbacks

        private void OnStoreInitialized(string nullMessage)
        {
            IsInitialized = true;
            var handler = StoreInitialized;
            if (handler != null) handler();
        }

        private void OnPurchaseStarted(string authority)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Started;
            m_authority = authority;
            var handler = PurchaseStarted;
            if (handler != null) handler(m_productID,authority);
        }

        private void OnPurchaseFailedToStart(string error)
        {
            var handler = PurchaseFailedToStart;
            if (handler != null) handler(error);
        }

        private void OnPurchaseSucceed(string authority)
        {
            m_purchaseOpen = false;
            m_authority = authority;
            m_status = ZarinpalPurchase.PurchaseStatus.Succeed;
            var handler = PurchaseSucceed;
            var purchase = new Purchase(m_productID, authority);
            if (handler != null) handler(purchase);
        }

        private void OnPurchaseFailed(string error)
        {
            if (m_purchaseOpen)
            {
                m_status = ZarinpalPurchase.PurchaseStatus.Failed;
                m_purchaseOpen = false;
                var handler = PurchaseFailed;
                if (handler != null) handler(error);
            }
        }

        protected virtual void OnPurchaseCanceled()
        {
            if (m_purchaseOpen)
            {
                m_status = ZarinpalPurchase.PurchaseStatus.Canceled;
                m_purchaseOpen = false;
                var handler = PurchaseCanceled;
                if (handler != null) handler();
            }
        }

        private void OnPaymentVerificationStarted(string url)
        {
            var handler = PaymentVerificationStarted;
            if (handler != null) handler(url);
        }

        private void OnPaymentVerificationSucceed(string refID)
        {
            m_refID = refID;
            m_purchaseOpen = false;
            m_status = ZarinpalPurchase.PurchaseStatus.Verified;
            var handler = PaymentVerificationSucceed;
            if (handler != null) handler(refID);
        }

        private void OnPaymentVerificationFailed(string error)
        {
            if (m_purchaseOpen)
            {
                m_status = ZarinpalPurchase.PurchaseStatus.Failed;
                m_purchaseOpen = false;
                var handler = PaymentVerificationFailed;
                if (handler != null) handler(error);
            }
        }

        #endregion

        public void StartPurchaseActivity()
        {
            _zarinpalJavaClass.CallStatic("startPurchaseActivity");
        }
    }
}
