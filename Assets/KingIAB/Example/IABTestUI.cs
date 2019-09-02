using KingKodeStudio.IAB;
using UnityEngine;

public class IABTestUI : MonoBehaviour
{
#if UNITY_ANDROID

    // Enter all the available skus from the CafeBazaar Developer Portal in this array so that item information can be fetched for them
    string[] skus = { "com.fanafzar.bazaarplugin.test1"
                , "com.fanafzar.bazaarplugin.test2"
                , "com.fanafzar.bazaarplugin.test3"
                , "com.fanafzar.bazaarplugin.monthly_subscribtion_test"
                , "com.fanafzar.bazaarplugin.annually_subscribtion_test"};

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, Screen.width - 15f, Screen.height - 15f));
        GUI.skin.button.fixedHeight = 50;
        GUI.skin.button.fontSize = 20;

        if (Button("Initialize IAB"))
        {
            GooglePlayIAB.init();
        }

        if (Button("Query Inventory"))
        {
            GooglePlayIAB.queryInventory(skus);
        }

        if (Button("Query SkuDetails"))
        {
            GooglePlayIAB.querySkuDetails(skus);
        }

        if (Button("Query Purchases"))
        {
            GooglePlayIAB.queryPurchases();
        }

        if (Button("Are subscriptions supported?"))
        {
            Debug.Log("subscriptions supported: " + GooglePlayIAB.areSubscriptionsSupported());
        }

        if (Button("Purchase Product Test1"))
        {
            GooglePlayIAB.purchaseProduct("test_purchase");
        }

        if (Button("Purchase Product Test2"))
        {
            GooglePlayIAB.purchaseProduct("com.fanafzar.bazaarplugin.test2");
        }

        if (Button("Consume Purchase Test1"))
        {
            GooglePlayIAB.consumeProduct("test_purchase");
        }

        if (Button("Consume Purchase Test2"))
        {
            GooglePlayIAB.consumeProduct("com.fanafzar.bazaarplugin.test2");
        }

        if (Button("Consume Multiple Purchases"))
        {
            var skus = new string[] { "test_purchase", "com.fanafzar.bazaarplugin.test2" };
            GooglePlayIAB.consumeProducts(skus);
        }

        if (Button("Test Unavailable Item"))
        {
            GooglePlayIAB.purchaseProduct("com.fanafzar.bazaarplugin.unavailable");
        }

        if (Button("Purchase Monthly Subscription"))
        {
            GooglePlayIAB.purchaseProduct("com.fanafzar.bazaarplugin.monthly_subscribtion_test", "subscription payload");
        }

        if (Button("Purchase Annually Subscription"))
        {
            GooglePlayIAB.purchaseProduct("com.fanafzar.bazaarplugin.annually_subscribtion_test", "subscription payload");
        }

        if (Button("Enable High Details Logs"))
        {
            GooglePlayIAB.enableLogging(true);
        }

        GUILayout.EndArea();
    }

    bool Button(string label)
    {
        GUILayout.Space(5);
        return GUILayout.Button(label);
    }

#endif

}

