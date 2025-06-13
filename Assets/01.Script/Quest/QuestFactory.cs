// using UnityEngine;
//
// public static class QuestFactory //퀘스트 생성
// {
//     public static QuestData GenerateQuests(QuestRepeat repeat, int index)
//     {
//         int target = repeat.startValue + index * repeat.valueIncrement;
//         string title = $"{repeat.titlePrefix}{index + 1}";
//         string description = string.Format(repeat.descriptionFormat, target);
//
//         return new QuestData
//         {
//             Title = title,
//             Description = description,
//             Type = repeat.questType,
//             TargetValue = target
//         };
//     }
// }
