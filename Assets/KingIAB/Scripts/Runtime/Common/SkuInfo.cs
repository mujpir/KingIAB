
using System.Collections.Generic;
using SimpleJSON;

#if UNITY_ANDROID

namespace KingKodeStudio.IAB
{
    public class SkuInfo
    {
        public string Title { get; private set; }
        public string Price { get; private set; }
        public string Type { get; private set; }
        public string Description { get; private set; }
        public string ProductId { get; private set; }

        public static List<SkuInfo> fromJsonArray(JSONArray items)
        {
            var skuInfos = new List<SkuInfo>();

            foreach (JSONNode item in items.AsArray)
            {
                SkuInfo bSkuInfo = new SkuInfo();
                bSkuInfo.fromJson(item.AsObject);
                skuInfos.Add(bSkuInfo);
            }

            return skuInfos;
        }

        public SkuInfo() { }

        public void fromJson(JSONClass json)
        {
            Title = json["title"].Value;
            Price = json["price"].Value;
            Type = json["type"].Value;
            Description = json["description"].Value;
            ProductId = json["productId"].Value;
        }

        public override string ToString()
        {
            return string.Format("<BazaarSkuInfo> title: {0}, price: {1}, type: {2}, description: {3}, productId: {4}",
                Title, Price, Type, Description, ProductId);
        }

    }
}
#endif