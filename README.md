# Tank-Turn-Game

![Image](https://github.com/user-attachments/assets/f3823b9d-86e4-4f1c-820f-037abb2e98b8)
### 당신의 탱크를 조절하여 최대한 적은 턴으로 적들을 격파하세요!

---

## 프로젝트 소개

**턴-탱크-게임은 3D 턴제 전략 시뮬레이션입니다.**

턴-탱크-게임은 3D 턴제 전략 시뮬레이션입니다.
플레이어는 본인의 턴에 이동, 공격을 한 번씩 진행할 수 있으며

w, a, s, d로 포신, 포탑의 각도를 조정하고 q, e등으로 위력을 조절할 수 있습니다.
적들을 맞춰 Hp를 0으로 만들어 클리어 할 수 있습니다. 

---

## 기획 및 제작 의도

기존 턴제 전술 게임의 핵심 재미인  
**전술적 위치 선정과 턴 관리**에 집중하고자 했습니다.

- 데이터 기반 구조로 맵과 적을 쉽게 확장
- FSM을 활용한 명확하고 확장 가능한 AI 구조
- 스테이지 진행형 게임 흐름 설계

이를 통해 **확장성과 유지보수성을 고려한 구조 설계 경험**을 쌓는 것을 목표로 제작했습니다.

---

## 프로젝트 정보

- 장르: 턴제 전략 / 전술 RPG
- 개발 인원: 1명 (게임 클라이언트)
- 플랫폼: PC / Mac
- 개발 엔진: Unity 3D

---

## 참고 자료

- Notion  
  - https://www.notion.so/14b9dfc52f178026b95cf9ace5814eb0?pvs=74

---

## 주요 기능 (Key Features)

- 적 AI
  - FSM 기반 추격 / 공격 / 대기 상태 관리
  - 상태별 행동 분리를 통한 확장성 확보

- 맵 시스템
  - CSV / JSON 기반 맵 데이터 구성
  - 다양한 스테이지를 손쉽게 추가 가능

- 스테이지 진행
  - 스테이지 단위 플레이
  - 클리어 여부를 JSON 데이터로 관리

- 멀티 플랫폼 지원
  - PC / Mac 환경에서 플레이 가능

---


## 개발 환경 (Development Environment)

| 항목 | 내용 |
|------|------|
| Engine | Unity 3D |
| Language | C# |
| Platform | PC / Mac |
| Data | CSV / JSON |

---

## CSV / JSON 기반 맵 시스템 및 FSM 적용

### FSM 구조
![Image](https://github.com/user-attachments/assets/8d89f448-a108-4be2-8484-810bf3a6b6d4)
![Image](https://github.com/user-attachments/assets/ad6ba180-417f-492b-9492-678636cc14f7)

- 플레이어 및 적의 공격 / 이동 상태를 FSM으로 분리
- enum 기반이 아닌
  - **State 클래스를 Dictionary로 관리하는 FSM 구조** 적용
- 새로운 상태 추가 시 코드 수정 최소화

---

### CSV / JSON 데이터 구조
![Image](https://github.com/user-attachments/assets/65f794c3-8f48-4527-9538-e4d5802aadfd)

- 엑셀을 활용해 맵 크기, 적 배치 정보 작성
- CSV → JSON 변환 후 Unity에서 파싱
- 스테이지 단위로 맵 로드
- JSON 데이터를 통해 클리어 여부 관리


## 라이선스 (License)

이 프로젝트는 MIT 라이선스 하에 배포됩니다.  
자세한 내용은 `LICENSE` 파일을 참고하세요.
