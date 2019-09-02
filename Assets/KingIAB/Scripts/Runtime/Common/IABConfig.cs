using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingKodeStudio.IAB
{
    [CreateAssetMenu]
    public class IABConfig : ScriptableObject
    {
        [SerializeField] private bool m_none;
        [SerializeField] private bool m_isGooglePlay;
        [SerializeField] private bool m_isBazaar;
        [SerializeField] private bool m_isMyket;
        [SerializeField] private bool m_isIranApps;
        [SerializeField] private bool m_isAndroidZarinpal;
        [SerializeField] private bool m_isIOSZarinpal;


        [SerializeField] private string m_googlePlay64Key;
        [SerializeField] private string m_bazaar64Key;
        [SerializeField] private string m_iranapps64Key;
        [SerializeField] private string m_myket64Key;
        
        
        //ZarinpalSetting
        [SerializeField] private string _merchantID;
        [SerializeField] private bool _autoStartPurchase = true;
        [SerializeField] private bool _autoVerifyPurchase = true;
        [SerializeField] private string _scheme = "return";
        [SerializeField] private string _host = "zarinpalpayment";
        [SerializeField] private string _calbackUrl;
        [SerializeField] private bool _useSchemeAndHostAsCallbackUrl;
        [SerializeField] private bool _logEnabled = true;


        public bool None
        {
            get { return m_none; }
        }

        public bool IsGooglePlay
        {
            get { return m_isGooglePlay; }
        }

        public bool IsBazaar
        {
            get { return m_isBazaar; }
        }

        public bool IsMyket
        {
            get { return m_isMyket; }
        }

        public bool IsIranApps
        {
            get { return m_isIranApps; }
        }

        public string GooglePlay64Key
        {
            get { return m_googlePlay64Key; }
        }

        public string Bazaar64Key
        {
            get { return m_bazaar64Key; }
        }

        public string Myket64Key
        {
            get { return m_myket64Key; }
        }

        public string Iranapps64Key
        {
            get { return m_iranapps64Key; }
        }

        public bool IsAndroidZarinpal
        {
            get { return m_isAndroidZarinpal; }
        }

        public bool IsIosZarinpal
        {
            get { return m_isIOSZarinpal; }
        }

        public string MerchantID
        {
            get { return _merchantID; }
        }

        public bool AutoVerifyPurchase
        {
            get { return _autoVerifyPurchase; }
        }

        public string Scheme
        {
            get { return _scheme; }
        }

        public string Host
        {
            get { return _host; }
        }

        public bool LogEnabled
        {
            get { return _logEnabled; }
        }

        public string CallbackUrl
        {
            get { return _calbackUrl; }
        }

        public bool AutoStartPurchase
        {
            get { return _autoStartPurchase; }
        }
    }
}

