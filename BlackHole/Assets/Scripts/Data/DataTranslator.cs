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
            var pIndex = DataStorage.ins.symbolNames.IndexOf(doc.Name) + 1;

            Planet p = new Planet();

            if (doc.Value.AsBsonDocument["has_data"].AsBoolean)
            {
                float change = (float)doc.Value.AsBsonDocument["change"].ToDouble();
                float eps = (float)doc.Value.AsBsonDocument["eps"].ToDouble();
                float pe = (float)doc.Value.AsBsonDocument["pe"].ToDouble();
                float volume = (float)doc.Value.AsBsonDocument["vtp"].ToDouble();
                float marketcap = (float) (doc.Value.AsBsonDocument["marketcap"].ToDouble() / DataStorage.marketcapDiv);

                float changeLerp = Mathf.InverseLerp(DataStorage.changeMin, DataStorage.changeMax, change);
                float epsLerp = Mathf.InverseLerp(DataStorage.epsMin, DataStorage.epsMax, eps);
                float peLerp = Mathf.InverseLerp(DataStorage.peMin, DataStorage.peMax, pe);
                float volumeLerp = Mathf.InverseLerp(DataStorage.volumeMin, DataStorage.volumeMax, volume);
                float marketcapLerp = Mathf.InverseLerp(DataStorage.marketcapMin, DataStorage.marketcapMax, marketcap );

                p.color = Color.Lerp(Color.red, Color.green, changeLerp);
                p.size = Mathf.Lerp(Planet.sizeLow, Planet.sizeHigh, marketcapLerp);
                p.orbitPeriod = Mathf.Lerp(Planet.orbitPeriodHigh, Planet.orbitPeriodLow, volumeLerp);
                p.orbitRadius = Mathf.Lerp(Planet.orbitRadiusLow, Planet.orbitRadiusHigh, peLerp);
                p.orbitAngle = Mathf.Lerp(Planet.orbitAngleLow, Planet.orbitAngleHigh, epsLerp);

                planets.Add(pIndex, p);
            }           
            
            
        }
        return planets;
    }
}
