using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool m_bGrounded { get; set; }

    [Tooltip("이전 콜라이더")]
    private Collider m_colPrev;

    [SerializeField]
    [Tooltip("하단에 있는 오브젝트 없애는 용도")]
    private GameObject m_goObjCut;

    [SerializeField]
    [Tooltip("모든 오브젝트 제거")]
    private GameObject m_goAllCut;

    private void OnTriggerStay(Collider other)
    {
        if (m_colPrev != null)
        {
            if (m_colPrev != other && other.gameObject.tag == "Object")
            {
                float fOffsetY = other.transform.position.y - m_colPrev.transform.position.y;

                //더 높은 오브젝트에 위치한 상황이니까 새로운 오브젝트를 생성해야됨
                if(fOffsetY >= 2.0f)
                {
                    GameManagerInUp.gm.CreateObj(GameManagerInUp.gm.m_iCreateNum);
                    StartCoroutine(ActiveCutObj());
                }
            }
        }

        if (other.gameObject.tag == "Ground" ||
            other.gameObject.tag == "Object")
        {
            m_bGrounded                 = true;
            m_colPrev                   = other;
        }

        if (m_colPrev != other && other.gameObject.tag == "DeathGround")
        {
            m_bGrounded                 = true;
            m_colPrev                   = other;

            StartCoroutine(AllCutAndFirstCreateObj());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground" ||
            other.gameObject.tag == "Object")
        {
            m_bGrounded                 = false;
        }
    }

    IEnumerator ActiveCutObj()
    {
        m_goObjCut.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        m_goObjCut.gameObject.SetActive(false);
    }

    IEnumerator AllCutAndFirstCreateObj()
    {
        m_goAllCut.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        m_goAllCut.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        GameManagerInUp.gm.CreateObj(GameManagerInUp.gm.m_iCreateNum);
    }
}
