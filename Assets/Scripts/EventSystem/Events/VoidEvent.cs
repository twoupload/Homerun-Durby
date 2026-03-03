using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// baseEvent -> void Event 파생되는 스크립트
// void에 관련된 이벤트들을 생성
// void.cs와는 데이터 타입 정의와 이벤트 리스너 정의에서의 차이 존재
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03

[CreateAssetMenu(fileName="New Void Event", menuName ="Event/void")]
public class VoidEvent : BaseEvent<Void>
{
    public void Raise()
    {
        Raise(new Void());
    }
}
