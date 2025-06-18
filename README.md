# Team Project (가제)

  * [목차](#목차)
  * [🧭프로젝트 목적](#프로젝트-목적)
  * [📗게임 설명](#게임-설명)
  * [📽️주요 기능](#주요-기능)
  * [🛠️기술 스택](#기술-스택)
    + [개발 환경](#개발-환경)
    + [리소스](#리소스)


## 🧭프로젝트 목적

여러 방치형 게임과 내일배움캠프의 강의에서 배운 것을 토대로 </br>
기존 방치형 게임의 로직과 사이클을 배우고 응용하여 </br>
새로운 시스템의 방치형 게임을 만들어 보는 것을 목적으로 합니다.</br>

## 📗게임 설명

- 플레이어는 여러 용병을 고용하여 좀비를 막아내는 사령관입니다.
- 용병들은 자신을 향해 다가오는 좀비들을 처치하고 바리케이트를 지켜내야 합니다.
- 서서히 몰려오는 좀비들을 막고 돈을 벌어 용병들과 바리케이트를 업그레이드 합니다.
- 만약 다음 스테이지를 도전하면, 매우 강한 좀비들이 몰려오며, 이를 막아내면 더 많은 재화를 주는 좀비 웨이브가 시작됩니다.
- 일정한 목표를 달성하여 퀘스트를 깨고 특별한 재화를 통해 용병을 고용합니다. 같은 용병을 합쳐서 용병 랭크를 업그레이드 할 수도 있습니다.
- 용병의 능력치와 랭크를 업그레이드 하고, 더 높은 스테이지로 가서 더 많고 강한 좀비를 처치하는 것이 게임의 목표입니다.


### ◇ 로비

https://github.com/user-attachments/assets/df23862b-412c-486e-a452-d321db1a8f96

### ◇ 가챠

https://github.com/user-attachments/assets/146506e7-d404-4f94-b14f-57bc5e308546

### ◇ 캐릭터 관리

https://github.com/user-attachments/assets/48fa4358-8373-46b9-bc00-f63aecfcbcd4

### ◇ 전투 씬

#### 로비 - 전투 씬 전환

https://github.com/user-attachments/assets/5de271f4-13a9-4886-bb0d-7d7c149e5227

#### 전투 씬 배치
Map
ZombieSpawner
WaveManager
CharacterData
EventSystem

#### 용병(캐릭터) 공격 및 스킬

(추가 필요)

#### 좀비 종류 별 패턴(AI)
- 공격 타입은 근접과 원거리로 나뉘며, 원거리 좀비는 포물선 궤도로 투사체를 발사합니다
- 넉백 중에는 공격과 이동이 중단되며, 일정 시간 후 상태가 복구됩니다
- 사망 시 골드를 지급하고 일정 시간 후 오브젝트 풀로 반환됩니다

</br>


## 📽️주요 기능

### 자동 공격 및 스킬 사용
- (추가 바랍니다.)
  
### 업그레이드 시스템
- (추가 바랍니다.)
  
### 스테이지 사이클
- 웨이브가 시작되면 스테이지에 비례하여 좀비가 생성되고, 전부 처치 시 클리어됩니다. 바리게이트가 사망시 패배합니다.
- 일반 웨이브의 진행이(클리어 & 패배) 끝나면 버튼이 활성화되고, 누를 때 다음 스테이지로 진입합니다. 사망 시 전 웨이브로 후퇴합니다.
- 일반 웨이브를 진행이 끝나면 무한 웨이브가 시작됩니다. 무한 웨이브는 능력치와 습득재화가 1/10 입니다.
  
### 좀비 AI
- 활성화 시 정보값을 초기화하며 NavMesh.SamplePosition을 이용해 NavMesh 위에 자동 보정됩니다
- 상태는 Idle, Chase, Attack, Die 네 가지이며, FSM 방식으로 전환됩니다
- NavMeshAgent를 이용해 플레이어에게 이동하며, AttackRange에 도달 시 공격 상태로 전환됩니다
- 추적 중 플레이어와의 거리에 따라 애니메이션이 전환되고, 일정 거리에서는 회전만 유지합니다
- 공격 시 Animator 트리거로 공격 동작을 실행하며, 타겟이 IDamageable 인터페이스를 구현하고 있어야 대미지를 전달합니다
- 넉백이 발생하면 NavMeshAgent가 꺼지고, Rigidbody에 힘을 받아 물리적으로 밀려난 후 복구됩니다
- 사망 시 에이전트와 콜라이더가 비활성화되고, 일정 시간 후 풀로 반환되며 골드를 지급합니다
  
### 가챠 시스템
- 플레이어는 퀘스트를 깨서 가챠를 위한 재화(다이아)를 획득할 수 있습니다.
- 다이아를 사용하여 가챠를 진행하면, 정해진 확률에 따라 캐릭터를 획득할 수 있습니다.
- S랭크 이상의 캐릭터가 등장하면 특별한 이펙트가 발생합니다.
- 프리미엄 가챠는 더 많은 다이아를 필요로 하는 대신 더 좋은 캐릭터를 획득할 확률이 늘어납니다.

### 퀘스트
- 게임을 진행하며 특정 조건을 만족할 경우, 추가 재화를 획득할 수 있습니다.
- 퀘스트의 자세한 내용은 로비에서 확인할 수 있으며, 완료된 퀘스트는 자정을 기준으로 접속 시 갱신됩니다.
- 퀘스트의 진행사항과 완료여부, 보상 수령 여부를 저장하고 있어 게임을 재실행하더라도 진행사항이 유지됩니다.
  
### 맵
- 현대 도시에서 좀비사태가 발생하여 막아낸다는 컨셉의 일자형 맵을 구성하였습니다.
  
### 저장 및 로드
- 재화를 얻거나 사용하는 시점, 혹은 게임을 종료하는 시점에 플레이어 데이터(재화, 현재 스테이지, 캐릭터 목록, 업그레이드 단계 등)을 저장합니다.
- 게임이 시작할 때 로드하여 저번 종료 시점에 이어서 플레이가 가능합니다.

</br>

## 🛠️기술 스택
### 개발 환경
- Unity 2022.3.17f1
- Windows 10 & 11
- (추가 필요)

### 리소스
- Essential Game Music Pack(https://assetstore.unity.com/packages/audio/music/essential-game-music-pack-313118)
- Free Horror Ambience 2(https://assetstore.unity.com/packages/audio/music/free-horror-ambience-2-215651)
- Sci-fi Sounds(https://kenney.nl/assets/sci-fi-sounds)
- Post Apocalypse Guns Demo(https://assetstore.unity.com/packages/audio/sound-fx/weapons/post-apocalypse-guns-demo-33515)
- Zombie Sound Pack - Free Version(https://assetstore.unity.com/packages/audio/sound-fx/zombie-sound-pack-free-version-124430)
- Free Sound Effects Pack(https://assetstore.unity.com/packages/audio/sound-fx/free-sound-effects-pack-155776)
- Tokyo_Street(https://assetstore.unity.com/packages/3d/environments/urban/tokyo-street-228474)
- Free Quick Effects Vol. 1(https://assetstore.unity.com/packages/vfx/particles/free-quick-effects-vol-1-304424)
- Character Selection Effects FREE(https://assetstore.unity.com/packages/vfx/particles/character-selection-effects-free-285268)
- 100 Best Effects Pack(https://assetstore.unity.com/packages/vfx/particles/spells/100-best-effects-pack-25634)
- 배민 한나는열한살체(https://www.woowahan.com/fonts)
- AI 생성 이미지
- 
