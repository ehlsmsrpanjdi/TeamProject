using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class DebugWindow : EditorWindow
{
    [MenuItem("Window/DebugWindow")]
    public static void ShowWindow()
    {
        // 창을 띄운다
        GetWindow<DebugWindow>("MyDebugWindow");
    }
    static int StageValue = 0;
    private void OnGUI()
    {
        GUILayout.Label("이건 커스텀 에디터 창입니다!", EditorStyles.boldLabel);

        GUILayout.Label("정수 값 입력", EditorStyles.boldLabel);


        if (GUILayout.Button("테스트 버튼"))
        {

        }

        StageValue = EditorGUILayout.IntField("StageValue", StageValue);

        if (GUILayout.Button("스테이지 set"))
        {
            Player.Instance.Data.currentStage = StageValue;
        }

        if (GUILayout.Button("뽑기 버튼"))
        {
            GachaManager.Instance.DrawCharacter(GachaType.Normal, 1);
        }
     
        if (GUILayout.Button("프리미엄 뽑기 버튼"))
        {
            GachaManager.Instance.DrawCharacter(GachaType.Premium, 1);
        }

        if (GUILayout.Button("캐릭터 스폰"))
        {
            CharacterManager.Instance.EditorFunction();
        }

        if (GUILayout.Button("바리게이트 체력 전달"))
        {
            Barricade.instance.SetHealth();
        }

        if (GUILayout.Button("캐릭터 강화"))
        {
            CharacterManager.Instance.EditorFunctionEnhance();
        }

        if (GUILayout.Button("캐릭터 랭크업"))
        {
            CharacterManager.Instance.EditorFunctionRankUp();
        }

        if (GUILayout.Button("캐릭터 뽑기"))
        {
            CharacterManager.Instance.EditorFunctionCreat();
        }


        #region //팀장자리
        if (GUILayout.Button("다이아 1000개 획득"))
        {
            Player.Instance.AddDiamond(1000);
        }

        if (GUILayout.Button("battle canvas 열기"))
        {
            UIManager.Instance.OpenBattleCanvas();
        }

        if (GUILayout.Button("battle canvas 닫기"))
        {
            UIManager.Instance.CloseBattleCanvas();
        }

        if (GUILayout.Button("main canvas 열기"))
        {
            UIManager.Instance.OpenMainCanvas();
        }

        if (GUILayout.Button("main canvas 열기"))
        {
            UIManager.Instance.CloseMainCanvas();
        }

        #endregion


        #region QuestTest

        if (GUILayout.Button("공격력과 연결된 버튼"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.OnEnchantButtonPressed();
                }
                else
                {
                    Debug.LogWarning("QuestManager가 씬에서 발견되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning("플레이 모드에서만 실행할 수 있습니다.");
            }
        }

        if (GUILayout.Button("일퀘 완료 테스트 - 공격력"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.ClaimReward(0);
                }
                else
                {
                    Debug.LogWarning("QuestManager가 씬에서 발견되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning("플레이 모드에서만 실행할 수 있습니다.");
            }
        }

        if (GUILayout.Button("일퀘 완료 테스트 - 접속"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.ClaimReward(1);
                }
                else
                {
                    Debug.LogWarning("QuestManager가 씬에서 발견되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning("플레이 모드에서만 실행할 수 있습니다.");
            }
        }

        if (GUILayout.Button("일퀘 완료 테스트 - 10분 접속"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.ClaimReward(2);
                }
                else
                {
                    Debug.LogWarning("QuestManager가 씬에서 발견되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning("플레이 모드에서만 실행할 수 있습니다.");
            }
        }

        if (GUILayout.Button("일퀘 완료 테스트 - 전체"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.ClaimReward(3);
                }
                else
                {
                    Debug.LogWarning("QuestManager가 씬에서 발견되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning("플레이 모드에서만 실행할 수 있습니다.");
            }
        }

        if (GUILayout.Button("퀘스트 강제 리셋"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.ResetDailyQuests();
                    qm.CheckAllDailyQuests();
                }
                else
                {
                    Debug.LogWarning("QuestManager가 씬에서 발견되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning("플레이 모드에서만 실행할 수 있습니다.");
            }
        }


        //if (GUILayout.Button("다음 스테이지로 진입"))
        //{
        //    if (Application.isPlaying)
        //    {
        //        WaveManager wm = FindObjectOfType<WaveManager>();
        //        //wm?.ProceedToNextStage();
        //    }
        //}



        if (GUILayout.Button("모든 좀비 강제 사망"))
        {
            if (Application.isPlaying)
            {
                var allZombies = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (var obj in allZombies)
                {
                    ZombieAI ai = obj.GetComponent<ZombieAI>();
                    if (ai != null && !ai.IsDead)
                    {
                        ai.TakeDamage(9999, obj.transform.position + Vector3.back, 0f);
                    }
                }
            }
        }

        if (GUILayout.Button("모든 좀비 제거"))
        {
            if (Application.isPlaying)
            {
                var allZombies = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (var obj in allZombies)
                {
                    ZombieAI ai = obj.GetComponent<ZombieAI>();
                    if (ai != null)
                    {
                        if (obj.name.Contains("Zombie1"))
                            ai.ResetAndReturnToPool("Zombie1");
                        else if (obj.name.Contains("Zombie2"))
                            ai.ResetAndReturnToPool("Zombie2");
                        else
                            GameObject.Destroy(obj);
                    }
                }
            }
        }

        #endregion
    }
}
