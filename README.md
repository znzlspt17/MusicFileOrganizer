# 🎵 Music File Organizer

<div align="center">

**메타데이터 기반 음악 파일 자동 정리 프로그램**

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![WinUI3](https://img.shields.io/badge/WinUI3-0078D4?style=for-the-badge&logo=windows&logoColor=white)

</div>

---

## 📋 프로젝트 소개

음악 파일의 메타데이터(아티스트, 앨범, 제목 등)를 읽어 정리해주는 Windows 데스크톱 애플리케이션입니다. 
널부러져 있는 음악 파일들을 일관된 폴더 구조로 정리하여 음악 라이브러리 관리를 쉽게 만들어줍니다.

### ✨ 주요 기능

- **메타데이터 자동 인식**: MP3, FLAC 등 다양한 형식의 음악 파일 메타데이터 자동 추출
- **스마트 폴더 구조**: 아티스트/앨범 기반 자동 폴더 생성 및 분류
- **직관적인 UI**: WinUI 3 기반의 모던하고 깔끔한 사용자 인터페이스
- **빠른 처리**: 비동기 처리로 대용량 파일도 효율적으로 정리
- **미리보기 기능**: 실제 이동 전 정리 결과 미리 확인 가능

---

## 🛠️ 기술 스택

### 핵심 기술
- **언어**: C# 12.0
- **프레임워크**: .NET 8.0 / WinUI 3
- **아키텍처**: MVVM (Model-View-ViewModel)
- **UI 프레임워크**: Windows App SDK 1.7

### 주요 라이브러리
- **TagLibSharp**: 음악 파일 메타데이터 처리
- **CommunityToolkit.Mvvm**: MVVM 패턴 구현

### 프로젝트 구조
```
MusicFileOrganizer/
├── Model/           # 데이터 모델
├── ViewModel/       # 비즈니스 로직
├── Controls/        # 커스텀 UI 컨트롤
├── Services/        # 파일 처리 서비스
├── Factories/       # 객체 생성 팩토리
└── Utils/           # 유틸리티 함수
```

---

## 🚀 시작하기

### 시스템 요구사항

- **OS**: Windows 10 (1809) 이상
- **런타임**: .NET 8.0 Runtime
- **Windows App SDK**: 1.7

### 설치 방법

1. **런타임 설치**
   ```bash
   # .NET 8.0 Runtime 다운로드
   https://dotnet.microsoft.com/download/dotnet/8.0
   ```

2. **프로젝트 클론**
   ```bash
   git clone https://github.com/znzlspt17/MusicFileOrganizer.git
   cd MusicFileOrganizer
   ```

3. **빌드 및 실행**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

---

## 💡 사용 방법

### 기본 사용 흐름

1. **소스 폴더 선택** 
   - 정리할 음악 파일이 있는 폴더를 선택합니다

2. **대상 폴더 선택**
   - 정리된 파일을 저장할 폴더를 선택합니다

3. **미리보기**
   - 정리 결과를 미리 확인합니다

4. **실행**
   - "시작" 버튼을 클릭하여 파일 정리를 시작합니다

### 예시

```
정리 전:
📁 source/
  ├── song1.mp3
  ├── track_02.mp3
  └── music.mp3

정리 후:
📁 destination/
  ├── 📁 아티스트A/
  │   └── 📁 앨범1/
  │       └── 01. 제목1.mp3
  └── 📁 아티스트B/
      └── 📁 앨범2/
          └── 01. 제목2.mp3
```

---

## 📸 스크린샷

### 1. 초기 화면
앱 실행 시 나타나는 첫 화면입니다. 상단의 **+** 버튼을 눌러 소스 폴더를 선택할 수 있습니다.

![1-initial-screen](https://private-user-images.githubusercontent.com/19329533/508972343-ed378dff-9c8a-4d30-9d31-fdda71016df2.png?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NjI2NjI1OTgsIm5iZiI6MTc2MjY2MjI5OCwicGF0aCI6Ii8xOTMyOTUzMy81MDg5NzIzNDMtZWQzNzhkZmYtOWM4YS00ZDMwLTlkMzEtZmRkYTcxMDE2ZGYyLnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTExMDklMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUxMTA5VDA0MjQ1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWE4OTdiNjU5ZDU1ODhlMzM0YzE1ZGRjOWFiNjViNmQyZGY4ZWNkMDcxMmQ0YjM0YmZhMjQwYzk4MjFjYTdiMzUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.6jiYhQi_wfiGXA6tWBCMgle7V5eVKwHGLV2unpdM1cc)

### 2. 소스 폴더 선택 후
소스 폴더를 선택하면 음악 파일의 메타데이터가 자동으로 표시됩니다. 앨범아트, 트랙 번호, 제목, 앨범, 아티스트, 파일명, 경로를 확인할 수 있습니다.

![2-folder-selected](https://private-user-images.githubusercontent.com/19329533/508972346-43541b2f-d0a9-4a12-a93f-dc91a516bcec.png?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NjI2NjI1OTgsIm5iZiI6MTc2MjY2MjI5OCwicGF0aCI6Ii8xOTMyOTUzMy81MDg5NzIzNDYtNDM1NDFiMmYtZDBhOS00YTEyLWE5M2YtZGM5MWE1MTZiY2VjLnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTExMDklMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUxMTA5VDA0MjQ1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWNmNzQxYTZiOGQ4NGMzYmJhNGNhZDUwMmEyNmQxZTdiNTdjOTRkMDIwYTJiN2E1Y2M3MGEwZjc4MzFiZmI2ZGEmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.UDlo7JmHzWTa91Kee-W2I_W-Dv-6LKtz7biZ1VPnYjM)

### 3. 정리 확인 다이얼로그
체크 버튼을 누르면 정리될 폴더 목록을 미리 확인할 수 있는 다이얼로그가 표시됩니다. 아티스트별, 앨범별로 어떻게 정리될지 확인 후 진행할 수 있습니다.

![3-confirmation-dialog](https://private-user-images.githubusercontent.com/19329533/508972344-fab3f06e-3691-489d-9925-28f285d789de.png?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NjI2NjI1OTgsIm5iZiI6MTc2MjY2MjI5OCwicGF0aCI6Ii8xOTMyOTUzMy81MDg5NzIzNDQtZmFiM2YwNmUtMzY5MS00ODlkLTk5MjUtMjhmMjg1ZDc4OWRlLnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTExMDklMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUxMTA5VDA0MjQ1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWIzYWRiNmY2OGM2MDAzZGMwOTBmZGM2ZTMwMTJlMmZkYWIxNTAzOWQzNGJhM2Y3Nzg1NThiMzg3M2NkYzczZGEmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.5Dgmr_nFENqOjKQx7J6ZGO9rwhExofgnhLIELPdI1ss)

### 4. 정리 완료
정리가 완료되면 원본 폴더와 대상 폴더의 속성을 비교하여 결과를 확인할 수 있습니다. 파일 개수의 변화를 통해 정리가 성공적으로 완료되었음을 알 수 있습니다.

![4-completed](https://private-user-images.githubusercontent.com/19329533/508972345-5372a0ec-55b9-4114-8a64-6938c9e014f1.png?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NjI2NjI1OTgsIm5iZiI6MTc2MjY2MjI5OCwicGF0aCI6Ii8xOTMyOTUzMy81MDg5NzIzNDUtNTM3MmEwZWMtNTViOS00MTE0LThhNjQtNjkzOGM5ZTAxNGYxLnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTExMDklMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUxMTA5VDA0MjQ1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTAwY2E3ZTQ1YmRmMDVmMjJmMThkNzMzMGYxYzcwZmZlYTVmNWFlZWE3YjhkOWIxZjMyMzk2MTY3NTdjMGY1YjEmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.HVXQ2pOll3FSeeUV7uetUs_cx3nDVvQsPAJw1seiEXg)
---

## 🎯 프로젝트 하이라이트

### 1. MVVM 아키텍처 적용
- View와 ViewModel의 완전한 분리로 유지보수성 향상
- 데이터 바인딩을 통한 선언적 UI 구현

### 2. 비동기 처리
- async/await 패턴으로 UI 블로킹 없는 파일 처리
- 대용량 파일 처리 시에도 반응성 유지

### 3. 확장 가능한 구조
- Factory 패턴으로 새로운 파일 형식 추가 용이
- 서비스 레이어 분리로 기능 확장 간편

---

## 🔧 개발 환경 설정

### 권장 개발 도구

- **Visual Studio 2022** (17.8 이상)
  - 워크로드: .NET 데스크톱 개발, Windows 앱 개발
- **Visual Studio Code** (선택사항)

### 디버깅

```bash
# Debug 모드로 실행
dotnet run --configuration Debug

# Release 빌드
dotnet build --configuration Release
```

---

## 📝 향후 계획

- [ ] 소스 폴더 선택 및 작업 진행시 시각적으로 진행중임을 알 수 있는 정보 추가
- [ ] 음악 파일 메타데이터 편집 기능

---

## 👨‍💻 개발자

**znzlspt17**

- GitHub: [@znzlspt17](https://github.com/znzlspt17)
- E-mail: [znzlsit@naver.com](znzlsit@naver.com)

---

## 📄 라이선스

이 프로젝트는 개인 사용 목적으로 제작되었습니다.

---

## 🙏 감사의 말

이 프로젝트는 다음 기술들을 사용하여 만들어졌습니다:
- [TagLibSharp](https://github.com/mono/taglib-sharp) - 음악 메타데이터 처리
- [CommunityToolkit](https://github.com/CommunityToolkit) - MVVM 헬퍼
- [WinUI 3](https://github.com/microsoft/microsoft-ui-xaml) - UI 프레임워크

---

<div align="center">

**⭐ 이 프로젝트가 도움이 되었다면 Star를 눌러주세요!**

</div>
