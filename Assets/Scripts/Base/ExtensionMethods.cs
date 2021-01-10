using UnityEngine;

/// <summary>
/// A collection of extension methods to simplify simple repeated logic
/// </summary>
public static class ExtensionMethods
{

    /// <summary>
    /// Extension method for calculating a value using a percentage and a given range
    /// </summary>
    /// <param name="x">The float value to modify</param>
    /// <param name="min">The minimum value of the range</param>
    /// <param name="max">The maximum value of the range</param>
    /// <param name="percentage">The percentage to use</param>
    public static void CalculateFromPercentage(this ref float x, float min, float max, float percentage)
    {
        x = (percentage * (max - min)) + min;
    }

    /// <summary>
    /// Extension method for setting an array of <see cref="GameObject"/> active to true or false
    /// </summary>
    /// <param name="objects">The array of <see cref="GameObject"/></param>
    /// <param name="value">Whether active should be set to <see cref="true"/> or <see cref="false"/></param>
    public static void ToggleGameObjects(this GameObject[] objects, bool value)
    {
        //Iterate through every element in the array
        for (int i = 0; i < objects.Length; i++)
        {
            //Set active to the specified boolean value
            objects[i].SetActive(value);
        }
    }
}
