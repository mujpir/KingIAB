using System;
using Random = UnityEngine.Random;

namespace KingKodeStudio.IAB.Zarinpal
{
    public class ZarinpalEditor : IZarinpalPlatform
    {
        public string MerchantID { get; private set; }
        public bool AutoVerifyPurchase { get; private set; }
        public string Callback { get; private set; }
        public bool IsInitialized { get; private set; }
        
        private string m_productID;

        private string m_authority;

        private string m_refID;

        private Guid _transactionID;
        
        private ZarinpalPurchase.PurchaseStatus m_status;


        public ZarinpalPurchase PurchaseStatus
        {
            get
            {
                return new ZarinpalPurchase(_transactionID.ToString(),m_authority,m_refID,m_productID,m_status);
            }
        }

        public void Initialize(string merchantID, bool verifyPurchase, string schemeCallback,bool autoStartPurchase)
        {
            Log("initializing zarinpal with merchant-id : {0} , autoVerify : {1} , callback : {2}", merchantID,
                verifyPurchase, schemeCallback);
            MerchantID = merchantID;
            AutoVerifyPurchase = verifyPurchase;
            Callback = schemeCallback;
            IsInitialized = true;
            OnStoreInitialized();
        }

        public void Purchase(long amount, string desc, string productID)
        {
            m_authority = "fake_authority_00000000000000000" + Guid.NewGuid();
            OnPurchaseStarted(productID,m_authority);
            Log("purchasing amount of : {0} toman , desc : {1} , productID : {2}", amount, desc, productID);
            m_productID = productID;
            OnPurchaseSucceed(productID, m_authority);
            if (AutoVerifyPurchase)
            {
                VerifyPurchase(productID,(int) amount);
            }
        }

        public void VerifyPurchase(string authority,int amount)
        {
            m_authority = authority;
            OnPaymentVerificationStarted(m_authority);
            OnPaymentVerificationSucceed(m_authority);
        }

        private void Log(string log)
        {
            KKLog.Log(log);
        }

        private void Log(string log, params object[] args)
        {
            KKLog.Log(string.Format(log, args));
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

        protected virtual void OnStoreInitialized()
        {
            var handler = StoreInitialized;
            if (handler != null) handler();
        }

        protected virtual void OnPurchaseStarted(string productCode,string authority)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Started;
            m_authority = authority;
            var handler = PurchaseStarted;
            if (handler != null) handler(productCode,authority);
        }

        protected virtual void OnPurchaseFailedToStart(string error)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Failed;
            var handler = PurchaseFailedToStart;
            if (handler != null) handler(error);
        }

        protected virtual void OnPurchaseSucceed(string productID, string authority)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Succeed;
            var handler = PurchaseSucceed;
            var purchase = new Purchase(productID, authority);
            if (handler != null) handler(purchase);
        }

        protected virtual void OnPurchaseFailed(string error)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Failed;
            var handler = PurchaseFailed;
            if (handler != null) handler(error);
        }

        protected virtual void OnPurchaseCanceled()
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Canceled;
            var handler = PurchaseCanceled;
            if (handler != null) handler();
        }

        protected virtual void OnPaymentVerificationStarted(string obj)
        {
            var handler = PaymentVerificationStarted;
            if (handler != null) handler(obj);
        }

        protected virtual void OnPaymentVerificationSucceed(string obj)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Verified;
            var handler = PaymentVerificationSucceed;
            if (handler != null) handler(obj);
        }

        protected virtual void OnPaymentVerificationFailed(string error)
        {
            m_status = ZarinpalPurchase.PurchaseStatus.Failed;
            var handler = PaymentVerificationFailed;
            if (handler != null) handler(error);
        }
    }
}
