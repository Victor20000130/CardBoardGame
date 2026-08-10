# 작업 규칙

이 저장소에서 작업할 때는 아래 절차를 최우선으로 적용한다. 기존 Git 이력과 사용자의 미완료 변경을 보존한다.

## 작업 시작

1. 이 `AGENTS.md`를 읽는다.
2. `git status`로 로컬 변경을 확인한다.
3. 현재 브랜치와 `git remote -v`를 확인한다.
4. `git fetch --prune`으로 원격 최신 상태를 확인한다.
5. 충돌 위험이 없을 때만 `git pull --ff-only`를 실행한다.

로컬 변경을 폐기하지 않으며, 원격 상태를 확인하지 않은 채 개발하지 않는다.

## 개발 및 검증

- Unity 프로젝트 Root는 `CardBoardGame/`이다.
- `CardBoardGame/ProjectSettings/ProjectVersion.txt`에 지정된 Unity 버전을 사용한다.
- Windows와 macOS에서 함께 동작하도록 절대 경로와 OS 종속 코드를 피한다.
- 생성 파일, 캐시, Secret을 Commit하지 않는다.
- 변경 범위에 맞는 Unity Import, Compile, Run 및 Test를 수행한다.

## 작업 종료

정상 완료된 변경은 다음 순서로 마무리한다.

1. 실행 또는 테스트
2. `git status`
3. `git diff`
4. 예상하지 못한 변경 및 Generated File 검사
5. Secret 검사
6. Commit
7. Push
8. 원격 SHA 반영 확인

변경 없음, 테스트/실행 실패, 충돌, Secret 발견, 데이터 손실 위험, 인증 실패 또는 Push 권한 부재 시 Commit/Push를 강행하지 않는다. 변경사항을 안전하게 보존하고 원인을 보고한다.

## 금지

- Force Push
- `git reset --hard`
- `git clean -fd`
- 사용자 변경 폐기
- Secret Commit
- 실패한 테스트를 성공으로 보고
- 실제 정상 완료 변경을 로컬에만 방치
