using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;
public class DataSmoothner : MonoBehaviour
{
    public static DataSmoothner ins;

    public delegate void SmothingCompleteHandler();
    public static event SmothingCompleteHandler OnSmoothingComplete;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else if (ins != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public IEnumerator SmoothUpdate(PlanetsManager pm, Dictionary<int, Planet> data)
    {
        float steps = SimManager.smoothingStepsPerSlice;
        float secondsPerSlice = SimManager.ins.secondsPerSlice;

        Dictionary<int, float[]> IndexToStepIncrease = new Dictionary<int, float[]>();
        Dictionary<int, Planet> dataToGive = new Dictionary<int, Planet>();
        foreach (var planet in pm.IndexToControllerMap)
        {
            var index = planet.Key;
            var pl = planet.Value;

            dataToGive[index] = pm.IndexToControllerMap[index].planet;

            float[] dataSteps = new float[4];
            if (data.ContainsKey(index))
            {
                dataSteps[0] = -(pl.planet.size - data[index].size) / (float)steps;
                dataSteps[1] = -(pl.planet.orbitAngle - data[index].orbitAngle) / (float)steps;
                dataSteps[2] = -(pl.planet.orbitPeriod - data[index].orbitPeriod) / (float)steps;
                dataSteps[3] = -(pl.planet.orbitRadius - data[index].orbitRadius) / (float)steps;
            }
            else
            {
                dataSteps[0] = 0;
                dataSteps[1] = 0;
                dataSteps[2] = 0;
                dataSteps[3] = 0;
            }
            IndexToStepIncrease.Add(index, dataSteps);

        }

        for (var step = 1; step <= steps; step++)
        {
            foreach (var planet in dataToGive)
            {
                planet.Value.size += IndexToStepIncrease[planet.Key][0];
                planet.Value.orbitAngle += IndexToStepIncrease[planet.Key][1];
                planet.Value.orbitPeriod += IndexToStepIncrease[planet.Key][2];
                planet.Value.orbitRadius += IndexToStepIncrease[planet.Key][3];
                planet.Value.color = pm.IndexToControllerMap[planet.Key].planet.color;

                if (data.ContainsKey(planet.Key))
                {
                    planet.Value.color = data[planet.Key].color;
                }
            }
            pm.UpdatePlanets(dataToGive);
            yield return new WaitForSeconds(0.5f / steps);
        }

        if (OnSmoothingComplete != null) OnSmoothingComplete();
        yield break;
    }
}
