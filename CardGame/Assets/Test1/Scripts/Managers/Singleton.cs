using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance; // 싱글톤 인스턴스를 저장하는 변수

    public static T Instance
    {
        get
        {
            if (instance == null) // instance가 비어 있다면
            {
                instance = FindObjectOfType<T>(); // 현재 씬에서 T 타입 오브젝트를 찾음

                if (instance == null) // 씬에 없으면 새로 생성
                {
                    GameObject singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return instance; // instance 반환
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T; // instance가 없으면 현재 오브젝트를 싱글톤으로 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 이미 존재하면 중복 방지를 위해 삭제
        }
    }
}