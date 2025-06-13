using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> dailyQuests = new();
    private int playTimeSeconds = 0;

    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("QuestManager");
                    instance = obj.AddComponent<QuestManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "DailyQuests.json"); //프리펩 -> JSON 변환 준비(저장 경로)

    [Serializable] //유니티의 JsonUtility는 List 직접 직렬화 불가능. Wrapper 클래스가 필요함.
    private class SaveData
    {
        public List<QuestData> Quests;
        public string lastLoginDate;
    }

    private void SaveQuestsToJson()
    {
        var data = LoadQuestsFromJson();
        data.Quests = dailyQuests;
        data.lastLoginDate = DateTime.Today.ToString("yyyy-MM-dd");

        string json = JsonUtility.ToJson(data,true);
        // string encryptedJson = Encode.Encrypt(json);
        File.WriteAllText(SaveFilePath, json);
    }

    private SaveData CreateDefaultSaveData()
    {
        return new SaveData
        {
            Quests = new List<QuestData>(),
            lastLoginDate = DateTime.Today.ToString("yyyy-MM-dd")
        };
    }

    private SaveData LoadQuestsFromJson()
    {
        if (!File.Exists(SaveFilePath))
        {
            DebugHelper.LogWarrning("파일이 존재하지 않습니다.", this);
            return CreateDefaultSaveData();
        }
        string json = File.ReadAllText(SaveFilePath);
        // string decryptedJson = Encode.Decrypt(json);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null)
        {
            DebugHelper.LogWarrning("SaveData가 존재하지 않습니다.", this);
            return CreateDefaultSaveData();
        }

        return data;
    }

    private void Start()
    {
        InitializeDailyQuests(); // 퀘스트 데이터 정의

         // ResetDailyQuests(); // 테스트용 리셋.
        if (!IsSameDay()) // 오늘 첫 접속 시
        {
            ResetDailyQuests(); // 일퀘 리셋
            // SetLastLoginDate(); // 접속기록 갱신
        }
        else
        {
            LoadQuestsFromJson();
        }

        // 초기화 및 로드 후 퀘스트 상태 점검
        CheckAllDailyQuests();

        // 1분마다 플레이 시간 업데이트 시작
        InvokeRepeating(nameof(UpdatePlayTime), 60f, 60f);
    }

    // 일퀘 목록 생성 (QuestData 생성자 사용)
    private void InitializeDailyQuests()
    {
        dailyQuests.Clear();

        dailyQuests.Add(new QuestData(
            id: 0,
            title: "공격력 강화",
            description: "강화 버튼을 5번 누르세요",
            type: QuestType.IncreaseAttackLevel,
            targetValue: 5
        ));
        dailyQuests.Add(new QuestData(
            id: 1,
            title: "접속하기",
            description: "게임에 접속하세요",
            type: QuestType.DailyLogin,
            targetValue: 0
        ));
        dailyQuests.Add(new QuestData(
            id: 2,
            title: "10분 플레이",
            description: "10분 이상 플레이하세요",
            type: QuestType.Play10Minute,
            targetValue: 600
        ));
        dailyQuests.Add(new QuestData(
            id: 3,
            title: "전체 클리어 보상",
            description: "다른 일반퀘스트를 모두 클리어하세요",
            type: QuestType.AllClear,
            targetValue: 0
        ));
    }

    // 일퀘 클리어 여부 확인.
    private void CheckAllDailyQuests()
    {
        var loginQuest = dailyQuests.Find(q => q.Type == QuestType.DailyLogin);

        if (loginQuest != null && !loginQuest.IsCompleted)
        {
            loginQuest.IsCompleted = true;
            // SaveQuestProgressToPrefs(loginQuest);
            SaveQuestsToJson();
            Debug.Log($"접속 퀘스트 자동 완료");
        }

        CheckAllClearQuest(); // 전체 클리어 퀘스트 상태 갱신
    }

    // 퀘스트 클리어 버튼과 연결, 보상 수령.
    public void ClaimReward(QuestData quest)
    {
        if (quest.IsCompleted && !quest.IsClaimed)
        {
            quest.IsClaimed = true;
            // SaveQuestProgressToPrefs(quest);
            SaveQuestsToJson();
            Debug.Log($"보상 수령: {quest.Title}");

            // 보상 지급 로직 및 UI 업데이트 로직 추가 예정.
        }
        else
        {
            Debug.Log($"{quest.Title} 보상 수령 실패, 조건 여부: {quest.IsCompleted}, 수령 여부: {quest.IsClaimed}");
        }
    }

    // 일일 접속 여부 확인
    private bool IsSameDay()
    {
        SaveData data = LoadQuestsFromJson();
        string last = data?.lastLoginDate ?? "";
        return DateTime.TryParse(last, out var lastDate) && lastDate.Date == DateTime.Today;

        // 문자열 last를 DateTime 형식으로 변환 시도해서 true/false 반환.
        // lastDate.Date과 DateTime.Today는 날자는 해당값, 시간은 00으로 넣어서 가져옴 = 비교가능.
    }

    // 매일 처음 접속 시 접속일 갱신
    // private void SetLastLoginDate()
    // {
    //     PlayerPrefs.SetString("LastLoginDate", DateTime.Today.ToString("yyyy-MM-dd"));
    //     PlayerPrefs.Save();
    // }

    // 매일 처음 접속 시 퀘스트 클리어 조건 리셋
    private void ResetDailyQuests()
    {
        foreach (var quest in dailyQuests)
        {
            quest.CurrentValue = 0;
            quest.IsCompleted = false;
            quest.IsClaimed = false;

            // PlayerPrefs에 저장된 퀘스트 상태 삭제 (초기화)
            // PlayerPrefs.DeleteKey($"Quest_{quest.Id}_IsCompleted");
            // PlayerPrefs.DeleteKey($"Quest_{quest.Id}_IsClaimed");
            // PlayerPrefs.DeleteKey($"Quest_{quest.Id}_CurrentValue");
        }
        // PlayerPrefs.Save();
        SaveQuestsToJson();
        Debug.Log("일일 퀘스트가 초기화되었습니다.");
    }

    // 공격력 강화 버튼 클릭 시 호출
    public void OnEnchantButtonPressed()
    {
        var attackQuest = dailyQuests.Find(q => q.Type == QuestType.IncreaseAttackLevel);
        if (attackQuest == null || attackQuest.IsCompleted)
            return;

        attackQuest.CurrentValue++;
        // SaveQuestProgressToPrefs(attackQuest);
        Debug.Log($"강화 횟수: {attackQuest.CurrentValue} / {attackQuest.TargetValue}");

        if (attackQuest.CurrentValue >= attackQuest.TargetValue)
        {
            attackQuest.IsCompleted = true;
            // SaveQuestProgressToPrefs(attackQuest);
            Debug.Log($"공격력 강화 퀘스트 완료!");
            CheckAllClearQuest();
        }

        SaveQuestsToJson();
    }

    // PlayerPrefs에서 퀘스트 진행 상황 로드
    // private void LoadQuestProgressFromPrefs()
    // {
    //     foreach (var quest in dailyQuests)
    //     {
    //         string completedKey = $"Quest_{quest.Id}_IsCompleted";
    //         string claimedKey = $"Quest_{quest.Id}_IsClaimed";
    //         string currentValueKey = $"Quest_{quest.Id}_CurrentValue";
    //
    //         quest.IsCompleted = PlayerPrefs.GetInt(completedKey, 0) == 1;
    //         quest.IsClaimed = PlayerPrefs.GetInt(claimedKey, 0) == 1;
    //         quest.CurrentValue = PlayerPrefs.GetInt(currentValueKey, 0);
    //
    //         Debug.Log($"퀘스트 '{quest.Title}' 로드됨: 현재 값={quest.CurrentValue}, 완료됨={quest.IsCompleted}, 보상 수령={quest.IsClaimed}");
    //     }
    // }

    // PlayerPrefs에 퀘스트 상태 저장 및 캡슐화
    // private void SaveQuestProgressToPrefs(QuestData quest)
    // {
    //     string completedKey = $"Quest_{quest.Id}_IsCompleted";
    //     string claimedKey = $"Quest_{quest.Id}_IsClaimed";
    //     string currentValueKey = $"Quest_{quest.Id}_CurrentValue";
    //
    //     PlayerPrefs.SetInt(completedKey, quest.IsCompleted ? 1 : 0);
    //     PlayerPrefs.SetInt(claimedKey, quest.IsClaimed ? 1 : 0);
    //     PlayerPrefs.SetInt(currentValueKey, quest.CurrentValue);
    //     PlayerPrefs.Save();
    // }

    // 플레이 시간 업데이트 (1분마다 호출)
    private void UpdatePlayTime()
    {
        var playQuest = dailyQuests.Find(q => q.Type == QuestType.Play10Minute);

        if (playQuest != null && !playQuest.IsCompleted)
        {
            playTimeSeconds += 60; // 1분 증가
            Debug.Log($"플레이 시간 업데이트: {playTimeSeconds}초 / {playQuest.TargetValue}초");

            if (playTimeSeconds >= playQuest.TargetValue)
            {
                playQuest.IsCompleted = true;
                // SaveQuestProgressToPrefs(playQuest);
                CheckAllClearQuest();
                Debug.Log($"10분 이상 플레이 퀘스트 완료!");
            }

            SaveQuestsToJson();
        }
    }

    // '전체 클리어' 퀘스트 확인
    private void CheckAllClearQuest()
    {
        var allClearQuest = dailyQuests.Find(q => q.Type == QuestType.AllClear);
        if (allClearQuest == null || allClearQuest.IsCompleted)
            return;

        // 'AllClear' 퀘스트를 제외한 모든 퀘스트가 완료되었는지 확인
        bool allOthersCompleted = dailyQuests
            .Where(q => q.Type != QuestType.AllClear)
            .All(q => q.IsCompleted);

        if (allOthersCompleted)
        {
            allClearQuest.IsCompleted = true;
            // SaveQuestProgressToPrefs(allClearQuest);
            SaveQuestsToJson();
            Debug.Log("전체 클리어 보상 퀘스트 완료!");
        }
    }
}
