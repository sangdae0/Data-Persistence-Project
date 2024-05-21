using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
	[SerializeField]
	public class GameRecord
	{
		public string PlayerName;
		public int Score;
	}

	public TMP_Text BestScoreText;

	public TMP_InputField PlayerNameInputField;

	public string CurrentPlayerName;

	public GameRecord BestGameRecord;
	public bool HasNewBestGameRecord = false;

	public static GameManager Instance;

	private string BestGameRecordDataPath => Application.persistentDataPath + "/best_game_record.json";

	void Awake()
	{
		if (Instance)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		LoadBestGameRecord();
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	void LoadBestGameRecord()
	{
		string recordDataPath = BestGameRecordDataPath;
		if (!File.Exists(recordDataPath))
		{
			BestGameRecord = new GameRecord();
			HasNewBestGameRecord = true;
		}
		else
		{
			string json = File.ReadAllText(recordDataPath);
			BestGameRecord = JsonUtility.FromJson<GameRecord>(json);
			HasNewBestGameRecord = false;

			PlayerNameInputField.text = BestGameRecord.PlayerName;
		}

		BestScoreText.text = "Best Score"
							+ " : "
							+ BestGameRecord.PlayerName
							+ " : "
							+ BestGameRecord.Score;
	}

	public void SaveBestGameRecord()
	{
		File.WriteAllText(BestGameRecordDataPath, JsonUtility.ToJson(BestGameRecord));
	}

	// Update is called once per frame
	void Update()
	{

	}

	// transition to main gameplay scene
	public void OnStartButton()
	{
		CurrentPlayerName = PlayerNameInputField.text;
		SceneManager.LoadScene("main", LoadSceneMode.Single);
	}

	public void OnQuitButton()
	{
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}
}
