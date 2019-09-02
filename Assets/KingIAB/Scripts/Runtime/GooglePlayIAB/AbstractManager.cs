using UnityEngine;

namespace KingKodeStudio.IAB
{
    public abstract class AbstractManager : MonoBehaviour
    {
        private static GameObject mBazaarGameObject;
        private static MonoBehaviour mBazaarGameObjectMonobehaviourRef;

        private const string BazaarObjectName = "KingIABPlugin";

        public static GameObject getBazaarManagerGameObject()
        {
            if (mBazaarGameObject != null)
                return mBazaarGameObject;

            mBazaarGameObject = GameObject.Find(BazaarObjectName);
            if (mBazaarGameObject == null)
            {
                mBazaarGameObject = new GameObject(BazaarObjectName);
                DontDestroyOnLoad(mBazaarGameObject);
            }

            return mBazaarGameObject;
        }

        public static Object initialize(System.Type type)
        {
            try
            {
                var obj = (FindObjectOfType(type) as MonoBehaviour);
                if (obj != null)
                    return obj;

                GameObject managerGameObject = getBazaarManagerGameObject();
                GameObject gameObject = new GameObject("KingIABPlugin.IABEventManager");
                var component = gameObject.AddComponent(type);
                gameObject.transform.parent = managerGameObject.transform;
                return component;
            }
            catch (UnityException ex)
            {
                string str1 = "It looks like you have the " + type + " on a GameObject in your scene. Our prefab-less manager system does not require the " + type
                    + " to be on a GameObject.\nIt will be added to your scene at runtime automatically for you. Please remove the script from your scene." + ex;

                Debug.LogWarning(str1);
            }

            return null;
        }

        private void Awake()
        {
            gameObject.name = "KingIABPlugin.IABEventManager";
            DontDestroyOnLoad(this);
        }
    }
}