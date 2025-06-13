using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> dailyQuests = new();
    private int playTimeSeconds = 0;

    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UIManager");
                    instance = obj.AddComponent<UIManager>();
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

    private void Start()
    {
        InitializeDailyQuests();
        ResetDailyQuests();
        if (!IsSameDay()) // 오늘 첫 접속 시
        {
            ResetDailyQuests(); // 일퀘 리셋
            SetLastLoginDate(); // 접속기록 갱신
        }
        else
        {
            LoadQuestProgressFromPrefs(); // 오늘 데이터 불러오기
        }

        CheckAllDailyQuests();
        InvokeRepeating(nameof(UpdatePlayTime), 60f, 60f);

    }

    // 일퀘 목록 생성
    private void InitializeDailyQuests()
    {
        dailyQuests = new List<QuestData>
        {
            new QuestData
            {
                Id = 0,
                Title = "공격력 강화",
                Description = "강화 버튼을 5번 누르세요",
                Type = QuestType.IncreaseAttackLevel,
                TargetValue = 5,
                CurrentValue = 0,
                IsCompleted = false,
                IsClaimed = false
            },
            new QuestData
            {
                Id = 1,
                Title = "접속하기",
                Description = "게임에 접속하세요",
                Type = QuestType.DailyLogin,
                TargetValue = 0,
                CurrentValue = 0,
                IsCompleted = false,
                IsClaimed = false
            },
            new QuestData
            {
                Id = 2,
                Title = "10분 플레이",
                Description = "10분 이상 플레이하세요",
                Type = QuestType.Play30Minute,
                TargetValue = 540,
                CurrentValue = 0,
                IsCompleted = false,
                IsClaimed = false
            },
            new QuestData
            {
                Id = 3,
                Title = "전체 클리어 보상",
                Description = "다른 일반퀘스트를 모두 클리어하세요",
                Type = QuestType.AllClear,
                TargetValue = 0,
                CurrentValue = 0,
                IsCompleted = false,
                IsClaimed = false
            },
        };
    }

    // 일퀘 클리어 여부 확인. 현재는 foreach 의미 없으나, 퀘스트 추가할 수 있어 놔둠.
    public void CheckAllDailyQuests()
    {
        foreach (var quest in dailyQuests)
        {
            if (!quest.IsCompleted)
            {
                switch (quest.Type)
                {
                    case QuestType.DailyLogin:
                        quest.IsCompleted = true;
                        SaveQuestProgressToPrefs(quest);
                        CheckAllClearQuest();
                        break;
                }
            }
        }

        CheckAllClearQuest();
    }

    // 퀘스트 클리어 버튼과 연결, 보상 수령. 버튼 연결 시 ClaimReward(dailyQuests[0])처럼 연결.
    public void ClaimReward(QuestData quest)
    {
        if (quest.IsCompleted && !quest.IsClaimed)
        {
            quest.IsClaimed = true;
            SaveQuestProgressToPrefs(quest);

            Debug.Log($"보상 수령: {quest.Title}");
        }
        else
        {
            Debug.Log($"{quest.Title}보상 수령 실패, 조건여부: {quest.IsCompleted}, 수령여부 : {quest.IsClaimed}");
        }
    }

    // 일일 접속여부 확인
    private bool IsSameDay()
    {
        string last = PlayerPrefs.GetString("LastLoginDate", "");
        return DateTime.TryParse(last, out var lastDate) && lastDate.Date == DateTime.Today;
    }

    // 매일 처음 접속 시 접속일 갱신
    private void SetLastLoginDate()
    {
        PlayerPrefs.SetString("LastLoginDate", DateTime.Today.ToString("yyyy-MM-dd"));
        PlayerPrefs.Save();
    }

    // 매일 처음 접속 시 퀘스트 클리어 조건 리셋

    private void ResetDailyQuests()
    {
        foreach (var quest in dailyQuests)
        {
            quest.CurrentValue = 0;
            quest.IsCompleted = false;
            quest.IsClaimed = false;

            // PlayerPrefs에 저장된 퀘스트 상태 삭제 (초기화)
            PlayerPrefs.DeleteKey($"Quest_{quest.Id}_IsCompleted");
            PlayerPrefs.DeleteKey($"Quest_{quest.Id}_IsClaimed");
            PlayerPrefs.DeleteKey($"Quest_{quest.Id}_CurrentValue");
        }
        PlayerPrefs.Save();
    }

    // 공격력 강화 버튼 완성 시 연결하여 공격력 강화 퀘스트 클리어
    public void OnEnchantButtonPressed()
    {
        var attackQuest = dailyQuests.Find(q => q.Type == QuestType.IncreaseAttackLevel);
        if (attackQuest == null || attackQuest.IsCompleted)
            return;

        attackQuest.CurrentValue++;
        SaveQuestProgressToPrefs(attackQuest);
        Debug.Log($"강화 횟수: {attackQuest.CurrentValue} / {attackQuest.TargetValue}");

        if (attackQuest.CurrentValue >= attackQuest.TargetValue)
        {
            attackQuest.IsCompleted = true;
            SaveQuestProgressToPrefs(attackQuest);
            Debug.Log($"공격력 강화 퀘스트 완료!");
            CheckAllClearQuest();
        }
    }

    private void LoadQuestProgressFromPrefs()
    {
        foreach (var quest in dailyQuests)
        {
            string completedKey = $"Quest_{quest.Id}_IsCompleted";
            string claimedKey = $"Quest_{quest.Id}_IsClaimed";
            string currentValueKey = $"Quest_{quest.Id}_CurrentValue";

            quest.IsCompleted = PlayerPrefs.GetInt(completedKey, 0) == 1;
            quest.IsClaimed = PlayerPrefs.GetInt(claimedKey, 0) == 1;
            quest.CurrentValue = PlayerPrefs.GetInt(currentValueKey, 0);
        }
    }

    // 퀘스트 상태 저장하기
    private void SaveQuestProgressToPrefs(QuestData quest)
    {
        string completedKey = $"Quest_{quest.Id}_IsCompleted";
        string claimedKey = $"Quest_{quest.Id}_IsClaimed";
        string currentValueKey = $"Quest_{quest.Id}_CurrentValue";

        PlayerPrefs.SetInt(completedKey, quest.IsCompleted ? 1 : 0);
        PlayerPrefs.SetInt(claimedKey, quest.IsClaimed ? 1 : 0);
        PlayerPrefs.SetInt(currentValueKey, quest.CurrentValue);
        PlayerPrefs.Save();
    }

    // 30분 이상 플레이 검사(update에서 프레임마다 시간 더하는 건 낭비같아서 1분마다 검사해서 false면
    private void UpdatePlayTime()
    {
        var playQuest = dailyQuests.Find(q => q.Type == QuestType.Play30Minute);
        if (playQuest != null && !playQuest.IsCompleted)
        {
            playTimeSeconds += 60;

            if (playTimeSeconds >= playQuest.TargetValue)
            {
                playQuest.IsCompleted = true;
                SaveQuestProgressToPrefs(playQuest);
                CheckAllClearQuest();
                Debug.Log($"10분 이상 플레이 퀘스트 완료!");
            }
        }
    }


    private void CheckAllClearQuest()
    {
        var allClearQuest = dailyQuests.Find(q => q.Type == QuestType.AllClear);
        if (allClearQuest == null || allClearQuest.IsCompleted)
            return;

        bool allOthersCompleted = dailyQuests
            .Where(q => q.Type != QuestType.AllClear)
            .All(q => q.IsCompleted);

        if (allOthersCompleted)
        {
            allClearQuest.IsCompleted = true;
            SaveQuestProgressToPrefs(allClearQuest);
            Debug.Log("전체 클리어 보상 퀘스트 완료!");
        }
    }
}


// public class QuestManager : MonoBehaviour
// {
//     [Header("ScriptableObject로 만든 퀘스트들")]
//     public QuestRepeat[] questRepeats;
//
//     [Header("퀘스트 반복 횟수")]
//     public int repeatCount = 50;
//
//     [Header("자동 생성된 퀘스트 목록")]
//     public List<QuestData> allQuests = new();
//
//     //퀘스트 클리어 index.
//     private Dictionary<QuestType, int> questProgress = new();
//
//     void Start()
//     {
//         for (int i = 0; i < repeatCount; i++)
//         {
//             foreach (var pattern in questRepeats)
//             {
//                 QuestData quest = QuestFactory.GenerateQuests(pattern, i);
//                 allQuests.Add(quest);
//             }
//         }
//
//         Debug.Log($"생성된 퀘스트 수: {allQuests.Count}");
//
//
//         if (!IsTodayLoggedIn())
//         {
//             SetTodayLoggedIn();
//             CheckQuestProgress(QuestType.DailyLogin);
//         }
//     }
//
//     public void CheckQuestProgress(QuestType type, int currentValue = 0)
//     {
//         int index = questProgress.ContainsKey(type) ? questProgress[type] : 0;
//         var relevantQuests = allQuests.Where(q => q.Type == type).ToList();
//
//
//         if (index >= relevantQuests.Count)
//             return;
//
//         var currentQuest = relevantQuests[index];
//
//         if (IsQuestCompleted(currentQuest, currentValue))
//         {
//             Debug.Log($"[{type}] 퀘스트 완료: {currentQuest.Title}"); // 퀘스트 완료 시 dia든 뭐든 올려주기.
//             questProgress[type] = index + 1;
//         }
//     }
//
//     private bool IsQuestCompleted(QuestData quest, int currentValue)
//     {
//         switch (quest.Type)
//         {
//             case QuestType.IncreaseAttackLevel:
//                 return currentValue >= quest.TargetValue;
//
//             case QuestType.DailyLogin:
//                 return IsTodayLoggedIn();
//         }
//
//         return false;
//     }
//
//
//     private static bool IsTodayLoggedIn()
//     {
//         string lastLoginStr = PlayerPrefs.GetString("LastLoginDate", "");
//         DateTime today = DateTime.Today;
//
//         if (DateTime.TryParse(lastLoginStr, out var lastLogin))
//         {
//             return lastLogin.Date == today;
//         }
//
//         return false;
//     }
//
//
//     public static void SetTodayLoggedIn()
//     {
//         PlayerPrefs.SetString("LastLoginDate", DateTime.Today.ToString("yyyy-MM-dd"));
//         PlayerPrefs.Save();
//     }
// }
