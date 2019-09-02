using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZarinpalPurchase
{
    public enum PurchaseStatus
    {
        None,
        Started,
        Canceled,
        Succeed,
        Failed,
        Verified
    }
    public string RequestID { get; private set; }
    public string Authority { get; private set; }
    public string RefID { get; private set; }
    public string ProductID { get; private set; }
    
    public PurchaseStatus Status { get; private set; }

    public ZarinpalPurchase(string reqID,string authority, string refID,string productID,PurchaseStatus status)
    {
        RequestID = reqID;
        Authority = authority;
        RefID = refID;
        ProductID = productID;
        Status = status;
    }
    
}
