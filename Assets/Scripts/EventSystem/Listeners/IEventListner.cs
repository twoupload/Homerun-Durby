using UnityEngine;

// 리스너의 반응에 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03

public interface IEventListener<T>
{
    void OnEventRaised(T data);
}
