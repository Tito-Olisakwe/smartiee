using Newtonsoft.Json;
using System.IO;

public class DataService
{
    private const string DataFilePath = "Data/TriviaData.json";

    public TriviaData LoadTriviaData()
    {
        using (StreamReader reader = new StreamReader(DataFilePath))
        {
            string json = reader.ReadToEnd();
            TriviaData triviaData = JsonConvert.DeserializeObject<TriviaData>(json);
            return triviaData;
        }
    }
}
