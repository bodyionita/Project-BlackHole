using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;
public static class DataTranslator
{


    public static Dictionary<int, Planet> TranslateSlice(BsonDocument slice)
    {
        Dictionary<int, Planet> planets = new Dictionary<int, Planet>();

        foreach (var doc in slice)
        {
            var pIndex = DataStorage.ins.symbolNames.IndexOf(doc.Name);

            Planet p = new Planet();

            float changeLerp = Mathf.InverseLerp(DataStorage.ins.changeMin, DataStorage.ins.changeMax, (float)doc.Value.AsBsonDocument["change"].AsDouble);
            float epsLerp = Mathf.InverseLerp(DataStorage.ins.epsMin, DataStorage.ins.epsMax, (float)doc.Value.AsBsonDocument["latestEPS"].AsDouble);
            float peLerp = Mathf.InverseLerp(DataStorage.ins.peMin, DataStorage.ins.peMax, (float)doc.Value.AsBsonDocument["peRatioLow"].AsDouble);
            float marketcapLerp = Mathf.InverseLerp(DataStorage.ins.marketcapMin, DataStorage.ins.marketcapMax, (float)doc.Value.AsBsonDocument["marketcap"].AsDouble);

            p.color = new Color(Mathf.Lerp(0, 255, changeLerp), Mathf.Lerp(0, 255, 1 - changeLerp), 0);
            p.size = Mathf.Lerp(Planet.sizeLow, Planet.sizeHigh, marketcapLerp);
            p.orbitPeriod = Mathf.Lerp(Planet.orbitPeriodLow, Planet.orbitPeriodHigh, epsLerp);
            p.orbitRadius = Mathf.Lerp(Planet.orbitRadiusLow, Planet.orbitRadiusHigh, peLerp);

            planets.Add(pIndex, p);
        }
        return planets;
    }
}
