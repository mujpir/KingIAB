/// Copyright (C) 2012-2014 KingKodeStudio Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Xml;
using System.Collections.Generic;

namespace KingKodeStudio.IAB.Editor
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class StoreManifestTools : IManifestTools
    {
#if UNITY_EDITOR
		static StoreManifestTools instance = new StoreManifestTools();
        private static IABConfig setting;

        static StoreManifestTools()
		{
			IABManifestTools.ManTools.Add(instance);
		}

		public void ClearManifest(){
			RemoveZarinpalBPFromManifest();
			RemoveGooglePlayBPFromManifest();
			RemoveBazaarBPFromManifest();
			RemoveIranappsBPFromManifest();
			RemoveMyketBPFromManifest();
        }
		public void UpdateManifest() {
			HandleZarinpalBPManifest ();
			HandleGooglePlayBPManifest();
			HandleBazaarBPManifest();
			HandleIranappsBPManifest();
			HandleMyketBPManifest();
        }

		public void HandleZarinpalBPManifest()
		{
			if (StoreSettings.IsAndroidZarinpal) {
				AddZarinpalBPToManifest();
			} else {
				RemoveZarinpalBPFromManifest();
			}
		}

		private void AddZarinpalBPToManifest()
		{
			//This is a tag that required to prevent error on android when targeting android 9.0 or above
			//if(PlayerSettings.Android.targetSdkVersion >= AndroidSdkVersions.AndroidApiLevel27)
			IABManifestTools.AppendApplicationElement("uses-library", "org.apache.http.legacy",
				new Dictionary<string, string>()
				{
					{"required", "false"}
				});
			IABManifestTools.SetPermission("android.permission.INTERNET");
			IABManifestTools.AddActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalActivity",new Dictionary<string, string>()
			{
			    {"theme","@android:style/Theme.DeviceDefault.Light.Dialog.NoActionBar.MinWidth" }
			});
		    XmlElement activityElement = IABManifestTools.FindElementWithTagAndName("activity",
		        "com.kingcodestudio.unityzarinpaliab.ZarinpalActivity");
		    XmlElement intentElement = IABManifestTools.AppendElementIfMissing("intent-filter", null, null, activityElement);
		    IABManifestTools.AppendElementIfMissing("action", "android.intent.action.VIEW",
		        new Dictionary<string, string>(),intentElement);
		    IABManifestTools.AppendElementIfMissing("category", "android.intent.category.DEFAULT",
		        new Dictionary<string, string>(), intentElement);


		    IABManifestTools.AddActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalResultActivity", new Dictionary<string, string>()
		    {
		        {"theme","@android:style/Theme.DeviceDefault.Light.Dialog.NoActionBar.MinWidth" }
		    });
            XmlElement activityResultElement = IABManifestTools.FindElementWithTagAndName("activity",
		        "com.kingcodestudio.unityzarinpaliab.ZarinpalResultActivity");
		    XmlElement intentResultElement = IABManifestTools.AppendElementIfMissing("intent-filter", null, null, activityResultElement);
		    IABManifestTools.AppendElementIfMissing("action", "android.intent.action.VIEW",
		        new Dictionary<string, string>(), intentResultElement);
		    IABManifestTools.AppendElementIfMissing("category", "android.intent.category.DEFAULT",
		        new Dictionary<string, string>(), intentResultElement);
		    IABManifestTools.AppendElementIfMissing("category", "android.intent.category.BROWSABLE",
		        new Dictionary<string, string>(), intentResultElement);
		    var scheme = StoreSettings.Scheme;
		    var host = StoreSettings.Host;
		    IABManifestTools.RemoveElement("data", null, intentResultElement);
		    IABManifestTools.AppendElementIfMissing("data", null,
		        new Dictionary<string, string>()
		        {
		            {"scheme",scheme },
		            {"host",host },
		        }, intentResultElement);
        }
		
		
		private void RemoveZarinpalBPFromManifest(){
			// removing Iab Activity
			if (!StoreSettings.IsAndroidZarinpal)
			{
				IABManifestTools.RemoveApplicationElement("uses-library", "org.apache.http.legacy");
				IABManifestTools.RemoveActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalActivity");
				IABManifestTools.RemoveActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalResultActivity");
			}
		}


		public void HandleGooglePlayBPManifest()
		{
			if (StoreSettings.IsGooglePlay) {
				AddGooglePlayBPToManifest();
			} else {
				RemoveGooglePlayBPFromManifest();
			}
		}
		
		private void AddGooglePlayBPToManifest()
		{
			IABManifestTools.SetPermission("com.android.vending.BILLING");
			IABManifestTools.AddActivity("com.bazaar.BazaarIABProxyActivity",
				new Dictionary<string, string>()
				{
					{"theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen"}
				});
			IABManifestTools.AddMetaDataTag("billing.service", "google.GooglePlayIabService");
		}

		private void RemoveGooglePlayBPFromManifest(){
            // removing Iab Activity
            if (!StoreSettings.IsAndroidZarinpal)
            {
	            // removing BILLING permission
	            IABManifestTools.RemovePermission("com.android.vending.BILLING");

	            if (!StoreSettings.IsMyket && !StoreSettings.IsGooglePlay && !StoreSettings.IsIranApps && !StoreSettings.IsBazaar)
	            {
		            // removing Iab Activity
		            IABManifestTools.RemoveActivity("com.bazaar.BazaarIABProxyActivity");
		            IABManifestTools.RemoveApplicationElement("meta-data", "billing.service");
	            }
            }
		}


		public void HandleBazaarBPManifest()
		{
			if (StoreSettings.IsBazaar)
			{
				AddBazaarBPToManifest();
			}
			else
			{
				RemoveBazaarBPFromManifest();
			}
		}

		private void AddBazaarBPToManifest()
        {
            IABManifestTools.SetPermission("com.farsitel.bazaar.permission.PAY_THROUGH_BAZAAR");
            IABManifestTools.AddActivity("com.bazaar.BazaarIABProxyActivity",
                    new Dictionary<string, string>()
                    {
                        {"theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen"},
                    });
            IABManifestTools.AddMetaDataTag("billing.service", "bazaar.BazaarIabService");
        }

        private void RemoveBazaarBPFromManifest()
        {
            // removing BILLING permission
            IABManifestTools.RemovePermission("com.farsitel.bazaar.permission.PAY_THROUGH_BAZAAR");

            if (!StoreSettings.IsMyket && !StoreSettings.IsGooglePlay && !StoreSettings.IsIranApps && !StoreSettings.IsBazaar)
            {
	            // removing Iab Activity
	            IABManifestTools.RemoveActivity("com.bazaar.BazaarIABProxyActivity");
	            IABManifestTools.RemoveApplicationElement("meta-data", "billing.service");
            }
        }


        public void HandleIranappsBPManifest()
        {
            if (StoreSettings.IsIranApps)
            {
                AddIranappsBPToManifest();
            }
            else
            {
                RemoveIranappsBPFromManifest();
            }
        }

        private void AddIranappsBPToManifest()
        {
	        IABManifestTools.SetPermission("ir.tgbs.iranapps.permission.BILLING");
	        IABManifestTools.AddActivity("com.bazaar.BazaarIABProxyActivity",
                new Dictionary<string, string>()
                {
                    {"theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen"},
                });
	        IABManifestTools.AddMetaDataTag("billing.service", "iranapps.IranappsIabService");
        }

        private void RemoveIranappsBPFromManifest()
        {
            // removing BILLING permission
            IABManifestTools.RemovePermission("ir.tgbs.iranapps.permission.BILLING");

            if (!StoreSettings.IsMyket && !StoreSettings.IsGooglePlay && !StoreSettings.IsIranApps && !StoreSettings.IsBazaar)
            {
	            // removing Iab Activity
	            IABManifestTools.RemoveActivity("com.bazaar.BazaarIABProxyActivity");
	            IABManifestTools.RemoveApplicationElement("meta-data", "billing.service");
            }
        }




        public void HandleMyketBPManifest()
        {
            if (StoreSettings.IsMyket)
            {
                AddMyketBPToManifest();
            }
            else
            {
                RemoveMyketBPFromManifest();
            }
        }

        private void AddMyketBPToManifest()
        {
	        IABManifestTools.SetPermission("ir.mservices.market.BILLING");
	        IABManifestTools.AddActivity("com.bazaar.BazaarIABProxyActivity",
                new Dictionary<string, string>() {
                    {"theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen"},
                });
	        IABManifestTools.AddMetaDataTag("billing.service", "myket.MyketIabService");
        }

        private void RemoveMyketBPFromManifest()
        {
            // removing BILLING permission
            IABManifestTools.RemovePermission("ir.mservices.market.BILLING");

            if (!StoreSettings.IsMyket && !StoreSettings.IsGooglePlay && !StoreSettings.IsIranApps && !StoreSettings.IsBazaar)
            {
	            IABManifestTools.RemoveActivity("com.bazaar.BazaarIABProxyActivity");
	            IABManifestTools.RemoveApplicationElement("meta-data", "billing.service");
            }
        }

        public IABConfig StoreSettings
        {
            get
            {
                if(setting==null)
                    setting = AssetDatabase.LoadAssetAtPath<IABConfig>("Assets/KingIAB/Resources/IABSetting.asset");
                return setting;
            }
        }
#endif
    }
}