using UnityEngine;

// baseEvent -> vector3 Event 파생되는 스크립트
// 3차원 백터에 관련된 이벤트들을 생성
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03

[CreateAssetMenu(fileName = "New Vector3 Event", menuName = "Event/Vector3")]
public class Vector3Event : BaseEvent<Vector3 > { }