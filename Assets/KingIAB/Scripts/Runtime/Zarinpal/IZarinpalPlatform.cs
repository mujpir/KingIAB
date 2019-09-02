using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingKodeStudio.IAB.Zarinpal
{
    public interface IZarinpalPlatform
    {
        string MerchantID { get; }
        bool AutoVerifyPurchase { get; }

        string Callback { get; }
        bool IsInitialized { get; }

        ZarinpalPurchase PurchaseStatus { get; }

        void Initialize(string merchantID, bool verifyPurchase, string callbackScheme,bool autoStartPurchase);

        void Purchase(long amount, string productID, string desc);
        
        void VerifyPurchase(string authority,int amount);


        event Action StoreInitialized;

        event Action<string,string> PurchaseStarted;

        event PurchaseFailedToStartDelegate PurchaseFailedToStart;

        event PurchaseSucceedDelegate PurchaseSucceed;

        event PurchaseFailedDelegate PurchaseFailed;

        event Action PurchaseCanceled;

        event Action<string> PaymentVerificationStarted;

        event Action<string> PaymentVerificationSucceed;

        event Action<string> PaymentVerificationFailed;
    }
}
