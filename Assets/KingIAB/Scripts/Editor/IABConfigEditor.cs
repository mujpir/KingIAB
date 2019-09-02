using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KingKodeStudio.IAB.Editor
{
    [CustomEditor(typeof(IABConfig))]
    public class IABConfigEditor : UnityEditor.Editor
    {
        private bool m_changed = false;
        private bool _mFoldoutAndroid;
        private bool _mFoldoutIOS;

        public override void OnInspectorGUI()
        {
            var changed = false;
            var color = GUI.color;
            var isNoneProp = serializedObject.FindProperty("m_none");
            var isGooglePlayProp = serializedObject.FindProperty("m_isGooglePlay");
            var googlePlay64KeyProp = serializedObject.FindProperty("m_googlePlay64Key");
            var isBazaarProp = serializedObject.FindProperty("m_isBazaar");
            var bazaar64KeyProp = serializedObject.FindProperty("m_bazaar64Key");
            var isMyketProp = serializedObject.FindProperty("m_isMyket");
            var myket64KeyProp = serializedObject.FindProperty("m_myket64Key");
            var isIranappsProp = serializedObject.FindProperty("m_isIranApps");
            var iranapps64KeyProp = serializedObject.FindProperty("m_iranapps64Key");
            var isAndroidZarinpalProp = serializedObject.FindProperty("m_isAndroidZarinpal");
            var isIOSZarinpalProp = serializedObject.FindProperty("m_isIOSZarinpal");

            
            var merchantIDProp = serializedObject.FindProperty("_merchantID");
            var autoVerifyProp = serializedObject.FindProperty("_autoVerifyPurchase");
            var _schemeProp = serializedObject.FindProperty("_scheme");
            var _hostProp = serializedObject.FindProperty("_host");
            var _useCallbackProp = serializedObject.FindProperty("_useSchemeAndHostAsCallbackUrl");
            var _callbackProp = serializedObject.FindProperty("_calbackUrl");
            var _autoStartPurchaseProp = serializedObject.FindProperty("_autoStartPurchase");
            var logEnabledProp = serializedObject.FindProperty("_logEnabled");


            EditorGUILayout.LabelField("King IAB Setting");
            EditorGUILayout.Space();
            _mFoldoutAndroid = EditorGUILayout.Foldout(_mFoldoutAndroid, "Android setting");
            if (_mFoldoutAndroid)
            {
                EditorGUI.indentLevel++;

                var selectionProps = new List<SerializedProperty>()
                {
                    isNoneProp,isGooglePlayProp, isBazaarProp, isMyketProp, isIranappsProp
                };
                
                //None
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(isNoneProp,new GUIContent("None"));
                changed = EditorGUI.EndChangeCheck();
                if (isNoneProp.boolValue)
                {
                    ApplyRadioButtonBehaviour(isNoneProp, selectionProps);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("I have my own billing service");
                    EditorGUI.indentLevel--;
                }
                
                //GooglePlay
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(isGooglePlayProp,new GUIContent("GooglePlay"));
                changed = changed | EditorGUI.EndChangeCheck();
                if (isGooglePlayProp.boolValue)
                {
                    ApplyRadioButtonBehaviour(isGooglePlayProp, selectionProps);
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(googlePlay64KeyProp,new GUIContent("GooglePlay Base64 Key"));
                    changed = changed | EditorGUI.EndChangeCheck();
                    EditorGUI.indentLevel--;
                }
                
                //Bazaar
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(isBazaarProp,new GUIContent("Bazaar"));
                changed = changed | EditorGUI.EndChangeCheck();
                if (isBazaarProp.boolValue)
                {
                    ApplyRadioButtonBehaviour(isBazaarProp, selectionProps);
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(bazaar64KeyProp,new GUIContent("Bazaar Base64 Key"));
                    changed = changed | EditorGUI.EndChangeCheck();
                    EditorGUI.indentLevel--;
                }
                
                //Myket
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(isMyketProp,new GUIContent("Myket"));
                changed = changed | EditorGUI.EndChangeCheck();
                if (isMyketProp.boolValue)
                {
                    ApplyRadioButtonBehaviour(isMyketProp, selectionProps);
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(myket64KeyProp,new GUIContent("Myket Base64 Key"));
                    changed = changed | EditorGUI.EndChangeCheck();
                    EditorGUI.indentLevel--;
                }
                
                //Iranapps
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(isIranappsProp,new GUIContent("IranApps"));
                changed = changed | EditorGUI.EndChangeCheck();
                if (isIranappsProp.boolValue)
                {
                    ApplyRadioButtonBehaviour(isIranappsProp, selectionProps);
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(iranapps64KeyProp,new GUIContent("Iranapps Base64 Key"));
                    changed = changed | EditorGUI.EndChangeCheck();
                    EditorGUI.indentLevel--;
                }
                
                //Zarinpal
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(isAndroidZarinpalProp,new GUIContent("Zarinpal"));
                changed = changed | EditorGUI.EndChangeCheck();
                if (isAndroidZarinpalProp.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(merchantIDProp);
                    EditorGUILayout.PropertyField(autoVerifyProp);
                    EditorGUILayout.PropertyField(_autoStartPurchaseProp);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(
                        "Provide a unique scheme and host for android\nOthewise your app may conflicts with other apps.\nZarinpal use this scheme and host to identify your app\nand return the purchase result. ",
                        GUILayout.Height(60));
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(_schemeProp);
                    EditorGUILayout.PropertyField(_hostProp);
                    EditorGUILayout.PropertyField(_useCallbackProp);
                    if (_useCallbackProp.boolValue)
                    {
                        _callbackProp.stringValue =
                            string.Format("{0}://{1}", _schemeProp.stringValue, _hostProp.stringValue);
                    }
                    EditorGUILayout.PropertyField(_callbackProp);
                    changed = changed | EditorGUI.EndChangeCheck();
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();



            _mFoldoutIOS = EditorGUILayout.Foldout(_mFoldoutIOS, "IOS setting");
            if (_mFoldoutIOS)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("IOS Sdk version : " + PlayerSettings.iOS.targetOSVersionString);
                try
                {
                    if (Convert.ToSingle(PlayerSettings.iOS.targetOSVersionString) < 8F)
                    {
                        color = GUI.color;
                        GUI.color = Color.red;
                        EditorGUILayout.LabelField("Zarinpal need sdk version 8.0 or higher");

                        GUI.color = Color.green;
                        if (GUILayout.Button("Set IOS SDK to 8.0"))
                        {
                            PlayerSettings.iOS.targetOSVersionString = "8.0";
                        }

                        GUI.color = color;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(logEnabledProp);

            if (changed)
            {
                m_changed = true;
            }

            color = GUI.color;
            if (m_changed)
            {
                EditorGUILayout.Space();
                GUI.color = Color.red;
                EditorGUILayout.LabelField("Hit Update to make changes affected.");
                GUI.color = color;
            }

            color = GUI.color;
            if (m_changed)
            {
                GUI.color = Color.green;
            }

            if (GUILayout.Button("Update Manifest & Files"))
            {
                var pluginDirectoryAndroid = Path.Combine(Application.dataPath, "Plugins/Android");
                if (!Directory.Exists(pluginDirectoryAndroid))
                {
                    Directory.CreateDirectory(pluginDirectoryAndroid);
                }

                var pluginDirectoryIOS = Path.Combine(Application.dataPath, "Plugins/IOS");
                if (!Directory.Exists(pluginDirectoryIOS))
                {
                    Directory.CreateDirectory(pluginDirectoryIOS);
                }

                handleJars(!(isGooglePlayProp.boolValue || isBazaarProp.boolValue || isMyketProp.boolValue ||
                             isIranappsProp.boolValue));
                handleZarinpalJars(!isAndroidZarinpalProp.boolValue);
                handleZarinpalIOS(!isIOSZarinpalProp.boolValue);
                IABManifestTools.GenerateManifest();
                AssetDatabase.Refresh();
                m_changed = false;
            }

            GUI.color = color;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Author : Mojtaba Pirveisi");
            EditorGUILayout.LabelField("@ 2018 KingKode Studio");

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }


        private void ApplyRadioButtonBehaviour(SerializedProperty checkProp,List<SerializedProperty> uncheckProps)
        {
            foreach (var property in uncheckProps)
            {
                if (property != checkProp)
                {
                    property.boolValue = false;
                }
            }
        }


        static void handleZarinpalJars(bool remove)
        {
            try
            {
                if (remove)
                {
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/library-1.0.19.jar");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/library-1.0.19.jar.meta");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/UnityZarinpalPurchase.aar");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/UnityZarinpalPurchase.aar.meta");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/constraint-layout-1.1.2.aar");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/constraint-layout-1.1.2.aar.meta");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/constraint-layout-solver-1.1.2.jar");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/constraint-layout-solver-1.1.2.jar.meta");

                }
                else
                {

                    string bpRootPath = Application.dataPath +
                                        "/KingIAB/Templates/Android/";
                    FileUtil.CopyFileOrDirectory(bpRootPath + "library-1.0.19.jar",
                        Application.dataPath + "/Plugins/Android/library-1.0.19.jar");
                    FileUtil.CopyFileOrDirectory(bpRootPath + "UnityZarinpalPurchase.aar",
                        Application.dataPath + "/Plugins/Android/UnityZarinpalPurchase.aar");
                    FileUtil.CopyFileOrDirectory(bpRootPath + "constraint-layout-1.1.2.aar",
                        Application.dataPath + "/Plugins/Android/constraint-layout-1.1.2.aar");
                    FileUtil.CopyFileOrDirectory(bpRootPath + "constraint-layout-solver-1.1.2.jar",
                        Application.dataPath + "/Plugins/Android/constraint-layout-solver-1.1.2.jar");
                }
            }
            catch
            {
            }
        }



        static void handleJars(bool remove)
        {
            try
            {
                if (remove)
                {
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/KingIABPlugin.jar");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/Android/KingIABPlugin.jar.meta");

                }
                else
                {

                    string bpRootPath = Application.dataPath +
                                        "/KingIAB/Templates/Android/";
                    FileUtil.CopyFileOrDirectory(bpRootPath + "KingIABPlugin.jar",
                        Application.dataPath + "/Plugins/Android/KingIABPlugin.jar");
                }
            }
            catch
            {
            }
        }



        static void handleZarinpalIOS(bool remove)
        {
            try
            {
                if (remove)
                {
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/HttpRequest.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/HttpRequest.swift.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/PaymentViewController.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/PaymentViewController.swift.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/URLs.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/URLs.swift.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinPal.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinPal.swift.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinPalPaymentDelegate.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinPalPaymentDelegate.swift.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinPalSDKPayment.h");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinPalSDKPayment.h.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnity.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnity.swift.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnityBridge.mm");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnityBridge.mm.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnityPlugin-Bridging-Header.h");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnityPlugin-Bridging-Header.h.meta");

                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnityWrapper.swift");
                    FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                                   "/Plugins/IOS/ZarinpalUnityWrapper.swift.meta");


                }
                else
                {

                    string bpRootPath = Application.dataPath +
                                        "/Zarinpal/Templates/IOS/";

                    FileUtil.CopyFileOrDirectory(bpRootPath + "HttpRequest.swift",
                        Application.dataPath + "/Plugins/IOS/HttpRequest.swift");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "PaymentViewController.swift",
                        Application.dataPath + "/Plugins/IOS/PaymentViewController.swift");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "URLs.swift",
                        Application.dataPath + "/Plugins/IOS/URLs.swift");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinPal.swift",
                        Application.dataPath + "/Plugins/IOS/ZarinPal.swift");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinPalPaymentDelegate.swift",
                        Application.dataPath + "/Plugins/IOS/ZarinPalPaymentDelegate.swift");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinPalSDKPayment.h",
                        Application.dataPath + "/Plugins/IOS/ZarinPalSDKPayment.h");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnity.swift",
                        Application.dataPath + "/Plugins/IOS/ZarinpalUnity.swift");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnityBridge.mm",
                        Application.dataPath + "/Plugins/IOS/ZarinpalUnityBridge.mm");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnityPlugin-Bridging-Header.h",
                        Application.dataPath + "/Plugins/IOS/ZarinpalUnityPlugin-Bridging-Header.h");

                    FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnityWrapper.swift",
                        Application.dataPath + "/Plugins/IOS/ZarinpalUnityWrapper.swift");
                }
            }
            catch
            {
            }
        }


        [MenuItem("KingIAB/Setting")]
        static void ShowConfig()
        {
            string path = "Assets/KingIAB/Resources/IABSetting.asset";
            var config = AssetDatabase.LoadAssetAtPath<IABConfig>(path);
            if (config == null)
            {
                config = IABConfig.CreateInstance<IABConfig>();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
            }

            Selection.activeObject = config;
        }
    }
}
