public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // Solution Plan:
        // 1. Create a new array of doubles with the specified length
        // 2. Loop through the array indices (0 to length-1)
        // 3. For each index i, calculate the multiple by multiplying the number by (i + 1)
        //    We use (i + 1) because we want to start with 1x the number, not 0x
        // 4. Store each calculated multiple in the corresponding array position
        // 5. Return the completed array

        // Create the array to store our multiples
        double[] result = new double[length];

        // Calculate and store each multiple
        for (int i = 0; i < length; i++)
        {
            result[i] = number * (i + 1);
        }

        return result;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // Solution Plan:
        // 1. For the rotation, we'll split the list into two parts:
        //    - The part that moves from the end to the beginning (last 'amount' elements)
        //    - The part that moves towards the end (all other elements)
        // 2. Use GetRange to get these two parts as separate lists
        // 3. Clear the original list to prepare for rebuilding
        // 4. Add the parts back in the correct order:
        //    - First add the elements that were at the end (they move to the front)
        //    - Then add the remaining elements
        
        // Get the portion from the end that will move to the front
        var movingToFront = data.GetRange(data.Count - amount, amount);
        
        // Get the portion that will move to the end
        var movingToBack = data.GetRange(0, data.Count - amount);
        
        // Clear the list and rebuild it in the correct order
        data.Clear();
        data.AddRange(movingToFront);
        data.AddRange(movingToBack);
    }
}
