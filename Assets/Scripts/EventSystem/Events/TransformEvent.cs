using UnityEngine;

// baseEvent -> Transform Event 파생되는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03

[CreateAssetMenu(fileName = "New Transform Event", menuName = "Event/Transform")]
public class TransformEvent : BaseEvent<Transform> { }