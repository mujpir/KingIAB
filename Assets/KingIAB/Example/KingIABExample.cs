using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KingKodeStudio.IAB.Example
{
    public class KingIABExample : MonoBehaviour
    {
        [SerializeField] private Text m_text;

        [SerializeField] private InputField m_amount;
        [SerializeField] private InputField m_desc;
        [SerializeField] private InputField m_productID;

        void Start()
        {
            KingIAB.StoreInitialized += KingIAB_StoreInitialized;
            KingIAB.StoreInitializeFailed += KingIAB_StoreInitializeFailed;
            KingIAB.PurchaseStarted += KingIAB_PurchaseStarted;
            KingIAB.PurchaseFailedToStart += KingIAB_PurchaseFailedToStart;
            KingIAB.PurchaseSucceed += KingIAB_PurchaseSucceed;
            KingIAB.PurchaseFailed += KingIAB_PurchaseFailed;
            KingIAB.ConsumeSucceed += KingIAB_ConsumeSucceed;
            KingIAB.ConsumeFailed += KingIAB_ConsumeFailed;
            KingIAB.QueryPurchasesSucceeded += KingIAB_QueryPurchasesSucceeded;
            KingIAB.QueryPurchasesFailed += KingIAB_QueryPurchasesFailed;
            KingIAB.QuerySkuDetailsSucceeded += KingIAB_QuerySkuDetailsSucceeded;
            KingIAB.QuerySkuDetailsFailed += KingIAB_QuerySkuDetailsFailed;
        }

        void OnDestroy()
        {
            KingIAB.StoreInitialized -= KingIAB_StoreInitialized;
            KingIAB.StoreInitializeFailed -= KingIAB_StoreInitializeFailed;
            KingIAB.PurchaseStarted -= KingIAB_PurchaseStarted;
            KingIAB.PurchaseFailedToStart -= KingIAB_PurchaseFailedToStart;
            KingIAB.PurchaseSucceed -= KingIAB_PurchaseSucceed;
            KingIAB.PurchaseFailed -= KingIAB_PurchaseFailed;
            KingIAB.ConsumeSucceed -= KingIAB_ConsumeSucceed;
            KingIAB.ConsumeFailed -= KingIAB_ConsumeFailed;
            KingIAB.QueryPurchasesSucceeded -= KingIAB_QueryPurchasesSucceeded;
            KingIAB.QueryPurchasesFailed -= KingIAB_QueryPurchasesFailed;
            KingIAB.QuerySkuDetailsSucceeded -= KingIAB_QuerySkuDetailsSucceeded;
            KingIAB.QuerySkuDetailsFailed -= KingIAB_QuerySkuDetailsFailed;
        }



        private void KingIAB_StoreInitialized()
        {
            Log("Store initialized");
            QueryPurchases();
        }

        private void KingIAB_StoreInitializeFailed(string error)
        {
            LogError(error);
        }

        private void KingIAB_PurchaseStarted(string productCode, string authority)
        {
            Log("Purchase started . productCode : "+productCode + " , authority : "+authority);
        }

        private void KingIAB_PurchaseFailedToStart(string error)
        {
            LogError("Purchase failed to start : " + error);
        }

        private void KingIAB_PurchaseSucceed(Purchase purchase)
        {
            Log(string.Format("Purchase success : productID : {0} , authority : {1} ", purchase.ProductId, purchase.OrderId));
        }

        private void KingIAB_PurchaseFailed(string error)
        {
            LogError("Purchase failed : "+error);
        }
        
        private void KingIAB_QuerySkuDetailsFailed(string error)
        {
            LogError("Query Sku Details failed : "+error);

        }

        private void KingIAB_QuerySkuDetailsSucceeded(List<SkuInfo> skuinfos)
        {
            var message = "number of Skus : "+skuinfos.Count+"\n";
            foreach (var sku in skuinfos)
            {
                message += sku + "\n";
            }

            Log("Query Sku Details succeed : " + message);
        }

        private void KingIAB_QueryPurchasesFailed(string error)
        {
            LogError("Query Purchases failed : "+error);

        }

        private void KingIAB_QueryPurchasesSucceeded(List<Purchase> purchases)
        {
            var message = "number of purchases : "+purchases.Count+"\n";
            foreach (var purchase in purchases)
            {
                message += purchase + "\n";
            }

            Log("Query Purchases succeed : " + message);
        }

        private void KingIAB_ConsumeFailed(string error)
        {
            LogError("Consume Purchases failed : "+error);

        }

        private void KingIAB_ConsumeSucceed(Purchase purchase)
        {
            Log("Consume Purchases succeed : productID : " + purchase.ProductId + " ,  OrderID : " + purchase.OrderId);

        }

        public void Initialize()
        {
            KingIAB.Initialize();
        }

        public void Purchase()
        {
            long price;
            long.TryParse(m_amount.text, out price);
            var desc = m_desc.text;
            var productID = m_productID.text;
            KingIAB.Purchase(productID,price, desc);
        }
        
        
        public void Consume()
        {
            int price;
            int.TryParse(m_amount.text, out price);
            var productID = m_productID.text;
            //Use productID as both orderid and productID
            KingIAB.Consume(productID,productID,price);
        }


        public void QueryPurchases()
        {
            KingIAB.QueryPurchases();
        }

        public void QuerySkus()
        {
            var productID = m_productID.text;
            KingIAB.QuerySkuDetails(new string[] {productID});
        }


        private void Log(string log)
        {
            KKLog.Log(log);
            m_text.text += "\n" + DateTime.Now.ToLongTimeString() + "  : <color=#FFFFFFFF>" + log + "</color>";
        }

        private void LogError(string error)
        {
            KKLog.LogError(error);
            m_text.text += "\n" + DateTime.Now.ToLongTimeString() + "  : <color=#FF0000FF>" + error + "</color>";
        }
    }
}
