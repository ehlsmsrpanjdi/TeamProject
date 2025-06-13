using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> dailyQuests = new();

    private int playTimeSeconds = 0; // 접속시간 계산.

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

    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "DailyQuests.json"); // JSON 변환 준비(저장 경로)
    // Application.persistentDataPath = 플랫폼 별 안전 저장 장소. pc = C:\Users\Admin\AppData\LocalLow\DefaultCompany\TeamProject.

    [Serializable] // 유니티의 JsonUtility는 List 직접 직렬화 불가능해서 감싸줄 클래스가 필요함.
    private class SaveData
    {
        public List<QuestData> Quests;
        public string lastLoginDate;
    } // Json에 저장할 데이터들

    private void Start()
     {
         InitializeDailyQuests(); // 퀘스트 구조 생성

         if (!IsSameDay()) // 오늘 첫 접속 시
         {
             ResetDailyQuests(); // 이전 일퀘 데이터(현재값, 클리어 여부, 보상 획득 여부) 리셋.
         }
         else // 첫 접속이 아니면
         {
             var loadedData = LoadQuestsFromJson(); // 데이터 불러와서
             ApplySavedProgress(loadedData.Quests); // 불러온 데이터 적용.
         }

         CheckAllDailyQuests(); // 초기화 및 로드 후 퀘스트 상태 점검

         InvokeRepeating(nameof(UpdatePlayTime), 60f, 60f); // 1분 뒤 시작, 1분마다 플레이 시간 업데이트
     }

    public List<QuestDisplayInfo> GetQuestDisplayInfos()
    {
        List<QuestDisplayInfo> displayInfos = new();

        foreach (var quest in dailyQuests)
        {
            displayInfos.Add(new QuestDisplayInfo(
                quest.Id,
                quest.Title,
                quest.Description,
                quest.IsCompleted,
                quest.IsClaimed
            ));
        }

        return displayInfos;
    }
    #region InitializeData

    private void InitializeDailyQuests() // 일퀘 목록 생성
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

    private SaveData CreateDefaultSaveData() // 초기 데이터 없으면 불러오기.
    {
        return new SaveData
        {
            Quests = new List<QuestData>(),
            lastLoginDate = DateTime.Today.ToString("yyyy-MM-dd")
        };
    }

    public void ResetDailyQuests() // 매일 처음 접속 시 퀘스트 클리어 조건 리셋. 현재 test 버튼 연결로 public, 이후 private로 변경 예정.
    {
        foreach (var quest in dailyQuests)
        {
            quest.CurrentValue = 0;
            quest.IsCompleted = false;
            quest.IsClaimed = false;
        }
        SaveQuestsToJson();
        // Debug.Log("일일 퀘스트가 초기화되었습니다.");
    }

    #endregion

    #region DataSaveAndLoad

    private void SaveQuestsToJson() // 데이터 저장
    {
        var data = new SaveData
        {
            Quests = dailyQuests,
            lastLoginDate = DateTime.Today.ToString("yyyy-MM-dd")
        };

        string json = JsonUtility.ToJson(data, true);
        string encryptedJson = Encode.Encrypt(json);
        File.WriteAllText(SaveFilePath, encryptedJson);
    }

    private SaveData LoadQuestsFromJson() // json으로 저장된 데이터 복호화 & 불러오기
    {
        if (!File.Exists(SaveFilePath))
        {
//            DebugHelper.LogWarrning("파일이 존재하지 않습니다.", this);
            return CreateDefaultSaveData();
        }
        string json = File.ReadAllText(SaveFilePath);
        string decryptedJson = Encode.Decrypt(json);
        SaveData data = JsonUtility.FromJson<SaveData>(decryptedJson);

        if (data == null)
        {
//            DebugHelper.LogWarrning("SaveData가 존재하지 않습니다.", this);
            return CreateDefaultSaveData();
        }

        return data;
    }

    private void ApplySavedProgress(List<QuestData> savedQuests) // 불러온 데이터 적용
    {
        foreach (var saved in savedQuests)
        {
            var quest = dailyQuests.Find(q => q.Id == saved.Id && q.Type == saved.Type);
            if (quest != null)
            {
                quest.CurrentValue = saved.CurrentValue;
                quest.IsCompleted = saved.IsCompleted;
                quest.IsClaimed = saved.IsClaimed;
            }
        }
    }

    #endregion

    #region AboutClear

    private void CheckAllDailyQuests() // 일퀘 클리어 여부 확인.
    {
        var loginQuest = dailyQuests.Find(q => q.Type == QuestType.DailyLogin);

        if (loginQuest != null && !loginQuest.IsCompleted)
        {
            loginQuest.IsCompleted = true;
            SaveQuestsToJson();
            // Debug.Log($"접속 퀘스트 자동 완료");
        }

        CheckAllClearQuest(); // 전체 클리어 퀘스트 상태 갱신
    }

    public void ClaimReward(QuestData quest) // 퀘스트 클리어 버튼과 연결, 보상 수령.
    {
        if (quest.IsCompleted && !quest.IsClaimed)
        {
            quest.IsClaimed = true;
            SaveQuestsToJson();
            // Debug.Log($"보상 수령: {quest.Title}");

            // 보상 지급 로직 및 UI 업데이트 로직 추가 예정.
        }
        else
        {
            Debug.Log($"{quest.Title} 보상 수령 실패, 조건 여부: {quest.IsCompleted}, 수령 여부: {quest.IsClaimed}");
        }
    }

    public void OnEnchantButtonPressed() // 공격력 강화 버튼 클릭 시 호출
    {
        var attackQuest = dailyQuests.Find(q => q.Type == QuestType.IncreaseAttackLevel);
        if (attackQuest == null || attackQuest.IsCompleted)
            return;

        attackQuest.CurrentValue++;
        // Debug.Log($"강화 횟수: {attackQuest.CurrentValue} / {attackQuest.TargetValue}");

        if (attackQuest.CurrentValue >= attackQuest.TargetValue)
        {
            attackQuest.IsCompleted = true;
            // Debug.Log($"공격력 강화 퀘스트 완료!");
            CheckAllClearQuest();
        }

        SaveQuestsToJson();
    }

    private void CheckAllClearQuest() // '모든 일퀘 클리어' 퀘스트 확인
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
            SaveQuestsToJson();
//            Debug.Log("전체 클리어 보상 퀘스트 완료!");
        }
    }

    #endregion

    #region AboutTime

    private bool IsSameDay() // 일일 접속 여부 확인
    {
        SaveData data = LoadQuestsFromJson();
        string last = data?.lastLoginDate ?? "";
        return DateTime.TryParse(last, out var lastDate) && lastDate.Date == DateTime.Today;
        // 문자열 last를 DateTime 형식으로 변환 시도해서 true/false 반환.
        // lastDate.Date과 DateTime.Today는 날자는 해당값, 시간은 00으로 넣어서 가져옴 = 비교가능.
    }

    private void UpdatePlayTime() // 플레이 시간 업데이트 (1분마다 호출)
    {
        var playQuest = dailyQuests.Find(q => q.Type == QuestType.Play10Minute);

        if (playQuest != null && !playQuest.IsCompleted)
        {
            playTimeSeconds += 60; // 1분 증가
            // Debug.Log($"플레이 시간 업데이트: {playTimeSeconds}초 / {playQuest.TargetValue}초");

            if (playTimeSeconds >= playQuest.TargetValue)
            {
                playQuest.IsCompleted = true;
                CheckAllClearQuest();
                // Debug.Log($"10분 이상 플레이 퀘스트 완료!");
            }

            SaveQuestsToJson();
        }
    }

    #endregion
}
