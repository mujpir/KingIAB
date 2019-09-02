using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace KingKodeStudio.IAB.Zarinpal
{
	public class ZarinpaliOS : MonoBehaviour, IZarinpalPlatform
	{

		private static ZarinpaliOS _instance;
		
		private bool m_purchaseOpen;

		private string m_productID;

		private string m_authority;

		private string m_refID;

		private Guid _transactionID;


		private ZarinpalPurchase.PurchaseStatus m_status;

		public static ZarinpaliOS CreateInstance()
		{
			if (_instance == null)
			{
				if (_instance == null)
				{
					_instance = new GameObject("ZarinpaliOS").AddComponent<ZarinpaliOS>();
					DontDestroyOnLoad(_instance.gameObject);
				}
			}

			return _instance;
		}

		public string MerchantID { get; private set; }

		/// <summary>
		/// AutoVerifyPurchase is always true on iOS
		/// </summary>

		public bool AutoVerifyPurchase
		{
			get { return true; }
		}

		/// <summary>
		/// Callback is not necessary for iOS
		/// </summary>
		public string Callback
		{
			get { return null; }
		}

		public bool IsInitialized { get; private set; }

		public ZarinpalPurchase PurchaseStatus
		{
			get { return new ZarinpalPurchase(_transactionID.ToString(), m_authority, m_refID, m_productID, m_status); }
		}

		public void Initialize(string merchantID, bool verifyPurchase, string callbackScheme,bool autoStartPurchase)
		{
			MerchantID = merchantID;
			_zu_initialize(merchantID);
		}

		public void Purchase(long amount, string productID, string desc)
		{
			_transactionID = Guid.NewGuid();
			m_productID = productID;
			m_authority = null;
			m_refID = null;
			m_status = ZarinpalPurchase.PurchaseStatus.None;
			_zu_startPurchaseFlow((int) amount, productID, desc);
		}

		public void VerifyPurchase(string authority,int amount)
		{
			throw new NotImplementedException();
		}

		public event Action StoreInitialized;

		public event Action<string,string> PurchaseStarted;
		public event PurchaseFailedToStartDelegate PurchaseFailedToStart;

#pragma warning disable 0067
		/// <summary>
		/// Not supported in iOS . use PaymentVerificationSucceed event instead.
		/// </summary>
		public event PurchaseSucceedDelegate PurchaseSucceed;

		public event PurchaseFailedDelegate PurchaseFailed;

		/// <summary>
		/// Not supported in iOS . use PurchaseFailed event instead.
		/// </summary>
		public event Action PurchaseCanceled;

		/// <summary>
		/// Not supported in iOS .
		/// </summary>
		public event Action<string> PaymentVerificationStarted;

		public event Action<string> PaymentVerificationSucceed;

		/// <summary>
		/// Not supported in iOS .
		/// </summary>
		public event Action<string> PaymentVerificationFailed;

#pragma warning restore 0067 


		#region Callbacks

		private void OnStoreInitialized(string nullMessage)
		{
			IsInitialized = true;
			var handler = StoreInitialized;
			if (handler != null) handler();
		}

		private void OnPurchaseStarted(string productCode,string authority)
		{
			m_status = ZarinpalPurchase.PurchaseStatus.Started;
			m_authority = authority;
			var handler = PurchaseStarted;
			if (handler != null) handler(productCode,authority);
		}

		private void OnPurchaseFailedToStart(string error)
		{
			m_status = ZarinpalPurchase.PurchaseStatus.Failed;
			var handler = PurchaseFailedToStart;
			if (handler != null) handler(error);
		}

		private void OnPurchaseSucceed(string authority)
		{
			/*
		    if (AutoVerifyPurchase)
		    {
		        var handler = PurchaseSucceed;
		        if (handler != null) handler(PurchasingItemID, authority);
		    }
		    else
		    {
		        if (_transactionID.HasValue)
		        {
		            _transactionID = null;
		            var handler = PurchaseSucceed;
		            if (handler != null) handler(PurchasingItemID, authority);
		        }
		    }
		    */
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
			/*
		    if (_transactionID.HasValue)
		    {
		        _transactionID = null;
		        var handler = PurchaseCanceled;
		        if (handler != null) handler();
		    }
		    */
		}

		private void OnPaymentVerificationStarted(string url)
		{
			var handler = PaymentVerificationStarted;
			if (handler != null) handler(url);
		}

		private void OnPaymentVerificationSucceed(string refID)
		{
			m_status = ZarinpalPurchase.PurchaseStatus.Verified;
			m_purchaseOpen = false;
			m_refID = refID;
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



		#region C-Extern

		[DllImport("__Internal")]
		private static extern void _zu_initialize(string merchantID);

		[DllImport("__Internal")]
		private static extern void _zu_startPurchaseFlow(int amount, string productID, string desc);

		#endregion
	}
}
