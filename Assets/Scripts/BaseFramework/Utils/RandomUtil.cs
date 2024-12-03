using UnityEngine;

public static class RandomUtil
{
    public static int RandomInteger(int min, int max)
    {
        return Random.Range(min, max);
    }
    public static float RandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }
    public static int RandomWeights(int[] weights)
    {
        int totalWeight = 0;
        foreach (var w in weights)
        {
            totalWeight += w;
        }

        int randomWeight = Random.Range(0, totalWeight);
        int weight = 0;

        for (int index = 0; index < weights.Length; index++)
        {
            weight += weights[index];
            if (weight >= randomWeight)
            {
                return index;
            }
        }

        return -1;
    }
    
}

