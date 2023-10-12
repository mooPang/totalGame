using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingManager : MonoBehaviour
{
    /**********************************
     *  볼링 게임 모든 상호작용 제어
     *  
     *  [볼링 동작]
     *  *** 총 10라운드
     *  *** 각 라운드당 기회 2번
     *  
     *   - 공 던짐
     *   - 핀 몇 개 쓰러졌는지 확인
     *      1. 쓰러졌다면
     *      	1) 스트라이크?
     *      		-> 맞다면 점수 기록
     *      		-> 다음 라운드로
     *      	2) 아니면 몇 개 남았는지 
     *      		-> 2차시도 <스페어 처리>
     *  		    -> 남은 개수 재확인
     *  		    -> 점수 기록
     *  		    -> 다음 라운드로
     *  		    
     *      2. 안쓰러졌다면		    
     *  		1) 회수 차감(2회 -> 1회)	    
     *  		2) 기록 0점	    
     *  		3) 2차시도 <스페어 처리>	    
     *  		4) 회수 차감(1회 -> 0회)	    
     *  		5) 남은 개수 재확인	    
     *  		6) 스페어 여부 확인	    
     *  		7) 맞게 점수 기록	    
     *  		
     *    - 2회 던졌거나 or 스트라이크
     *          ==> go to next Round
     *          
     *  ================
     *  핀 덱의 핀들 중 Y축이 순간적으로 바뀌는 애들을 총 카운트 10에서 뺸다
     *  
     *  
     *  
     **********************************/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
