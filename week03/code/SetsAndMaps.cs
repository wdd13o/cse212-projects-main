using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Globalization;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<int>();
        var results = new List<string>();

        foreach (var w in words)
        {
            if (string.IsNullOrEmpty(w) || w.Length != 2)
                continue;

            char a = w[0];
            char b = w[1];

            // ignore words with same letters (e.g., "aa") per spec
            if (a == b)
            {
                // still record the key to avoid reprocessing
                int key = (a << 16) | b;
                seen.Add(key);
                continue;
            }

            int keyForward = (a << 16) | b;
            int keyReverse = (b << 16) | a;

            if (seen.Contains(keyReverse))
            {
                // Format as "rev & w" e.g., "ma & am"
                results.Add($"{b}{a} & {w}");
            }

            seen.Add(keyForward);
        }

        return results.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var fields = line.Split(',');
            // education is at index 3 (e.g., Bachelors, HS-grad)
            if (fields.Length > 3)
            {
                var degree = fields[3].Trim();
                if (!string.IsNullOrEmpty(degree))
                {
                    if (!degrees.ContainsKey(degree))
                        degrees[degree] = 0;
                    degrees[degree]++;
                }
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        if (word1 == null || word2 == null)
            return false;

        // Normalize: remove spaces and make lowercase
        string Normalize(string s) => new string(s.Where(c => c != ' ').Select(char.ToLower).ToArray());

        var a = Normalize(word1);
        var b = Normalize(word2);

        if (a.Length != b.Length)
            return false;

        var counts = new Dictionary<char, int>();
        foreach (var c in a)
        {
            if (counts.ContainsKey(c)) counts[c]++;
            else counts[c] = 1;
        }

        foreach (var c in b)
        {
            if (!counts.ContainsKey(c))
                return false;

            counts[c]--;
            if (counts[c] < 0)
                return false;
        }

        return counts.Values.All(v => v == 0);
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // TODO Problem 5:
        // 1. Add code in FeatureCollection.cs to describe the JSON using classes and properties 
        // on those classes so that the call to Deserialize above works properly.
        // 2. Add code below to create a string out each place a earthquake has happened today and its magitude.
        // 3. Return an array of these string descriptions.
        if (featureCollection == null || featureCollection.Features == null)
            return Array.Empty<string>();

        var list = new List<string>();
        foreach (var feature in featureCollection.Features)
        {
            if (feature?.Properties == null)
                continue;

            var place = feature.Properties.Place ?? "(unknown place)";
            var mag = feature.Properties.Mag.HasValue ? feature.Properties.Mag.Value.ToString(CultureInfo.InvariantCulture) : "(unknown)";
            list.Add($"{place} - Mag {mag}");
        }

        return list.ToArray();
    }
}