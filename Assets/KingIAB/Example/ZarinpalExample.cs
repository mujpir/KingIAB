﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace KingKodeStudio.IAB.Zarinpal.Example
{
    public class ZarinpalExample : MonoBehaviour
    {
        [SerializeField] private Text m_text;

        [SerializeField] private InputField m_amount;
        [SerializeField] private InputField m_desc;
        [SerializeField] private InputField m_productID;

        void Start()
        {
            Zarinpal.StoreInitialized += Zarinpal_StoreInitialized;
            Zarinpal.StoreInitializeFailed += Zarinpal_StoreInitializeFailed;
            Zarinpal.PurchaseStarted += Zarinpal_PurchaseStarted;
            Zarinpal.PurchaseFailedToStart += Zarinpal_PurchaseFailedToStart;
            Zarinpal.PurchaseSucceed += Zarinpal_PurchaseSucceed;
            Zarinpal.PurchaseFailed += Zarinpal_PurchaseFailed;
            Zarinpal.PaymentVerificationStarted += Zarinpal_PaymentVerificationStarted;
            Zarinpal.PaymentVerificationSucceed += Zarinpal_PaymentVerificationSucceed;
            Zarinpal.PaymentVerificationFailed += Zarinpal_PaymentVerificationFailed;
        }

        void OnDestroy()
        {
            Zarinpal.StoreInitialized -= Zarinpal_StoreInitialized;
            Zarinpal.StoreInitializeFailed -= Zarinpal_StoreInitializeFailed;
            Zarinpal.PurchaseStarted -= Zarinpal_PurchaseStarted;
            Zarinpal.PurchaseFailedToStart -= Zarinpal_PurchaseFailedToStart;
            Zarinpal.PurchaseSucceed -= Zarinpal_PurchaseSucceed;
            Zarinpal.PurchaseFailed -= Zarinpal_PurchaseFailed;
            Zarinpal.PaymentVerificationStarted -= Zarinpal_PaymentVerificationStarted;
            Zarinpal.PaymentVerificationSucceed -= Zarinpal_PaymentVerificationSucceed;
            Zarinpal.PaymentVerificationFailed -= Zarinpal_PaymentVerificationFailed;
        }



        private void Zarinpal_StoreInitialized()
        {
            Log("Store initialized");
        }

        private void Zarinpal_StoreInitializeFailed(string error)
        {
            LogError(error);
        }

        private void Zarinpal_PurchaseStarted(string productcode, string authority)
        {
            Log("Purchase started . authority : " + authority +" and product code : "+productcode);
        }

        private void Zarinpal_PurchaseFailedToStart(string error)
        {
            LogError("Purchase failed to start : " + error);
        }

        private void Zarinpal_PurchaseSucceed(Purchase purchase)
        {
            Log(string.Format("Purchase success : productID : {0} , authority : {1} ", purchase.ProductId, purchase.OrderId));
        }

        private void Zarinpal_PurchaseFailed(string error)
        {
            LogError("Purchase failed : "+error);
        }

        private void Zarinpal_PurchaseCanceled()
        {
            Log("Purchase canceled by user");
        }

        private void Zarinpal_PaymentVerificationStarted(string authority)
        {
            Log("Start verifying purchase for : url : " + authority);
        }

        private void Zarinpal_PaymentVerificationSucceed(Purchase purchase)
        {
            Log("Purchase verification success : refid : " + purchase.OrderId);
        }

        private void Zarinpal_PaymentVerificationFailed(string error)
        {
            LogError("Purchase verification failed . error : "+error);
        }

        public void Initialize()
        {
            Zarinpal.Initialize();
        }

        public void Purchase()
        {
            long price;
            long.TryParse(m_amount.text, out price);
            var desc = m_desc.text;
            var productID = m_productID.text;
            Zarinpal.Purchase(price, desc, productID);
        }


        private void Log(string log)
        {
            m_text.text += "\n" + DateTime.Now.ToLongTimeString() + "  : <color=#FFFFFFFF>" + log + "</color>";
        }

        private void LogError(string error)
        {
            m_text.text += "\n" + DateTime.Now.ToLongTimeString() + "  : <color=#FF0000FF>" + error + "</color>";
        }
    }
}
