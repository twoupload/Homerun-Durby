using UnityEngine;

// baseEvent -> pitch type Event 파생되는 스크립트
// 피치 타입별 일어나는 event
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03

[CreateAssetMenu(fileName = "New PitchType Event", menuName = "Event/PitchType")]
public class PitchConfirmEvent : BaseEvent<Pitcher.PitchTypeSO>{}