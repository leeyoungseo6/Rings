using System.IO;
using UnityEngine;

public class DataManager
{
    public static DataManager Instance;

    // --- 게임 데이터 파일이름 설정 ("원하는 이름(영문).json") --- //
    private readonly string _gameDataFileName = "gameData.json";

    // --- 저장용 클래스 변수 --- //
    public Data data;

    public DataManager()
    {
        LoadGameData();
    }

    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, _gameDataFileName);

        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            // 저장된 파일 읽어오고 Json을 클래스 형식으로 전환해서 할당
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
        }
        else data = new Data();
    }
    
    public void SaveGameData()
    {
        // 클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, _gameDataFileName);

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(filePath, ToJsonData);
    }
}