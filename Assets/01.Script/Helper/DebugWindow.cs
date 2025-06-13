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

    private void OnGUI()
    {
        GUILayout.Label("이건 커스텀 에디터 창입니다!", EditorStyles.boldLabel);

        GUILayout.Label("정수 값 입력", EditorStyles.boldLabel);


        if (GUILayout.Button("테스트 버튼"))
        {

        }
        if (GUILayout.Button("스폰 버튼"))
        {
            CharacterManager.instance.EditorFunction();
        }

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
                    qm.ClaimReward(qm.dailyQuests[0]);
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

            UIManager.Instance.GetUI<UIBattleMemberViewer>().AddMember();

        }

        if (GUILayout.Button("일퀘 완료 테스트 - 접속"))
        {
            if (Application.isPlaying)
            {
                QuestManager qm = FindObjectOfType<QuestManager>();
                if (qm != null)
                {
                    qm.ClaimReward(qm.dailyQuests[1]);
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
                    qm.ClaimReward(qm.dailyQuests[2]);
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
                    qm.ClaimReward(qm.dailyQuests[3]);
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

        #endregion
    }
}
