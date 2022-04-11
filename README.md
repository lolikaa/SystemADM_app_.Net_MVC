# SystemADM_app_.Net_MVC

Opis sytuacji:
Mieszkaniec dzwoni do administracji swojego budynku z informacją o wykrytej usterce – osoba przyjmująca zgłoszenie tworzy nowe zgłoszenie w bazie, uzupełniając przy tym wszystkie wymagane dane. Następnie przypisuje je do odpowiedniego administratora. 
Zarząd nieruchomości może również dokonywać wpisów zgłoszeniowych – jednak tylko dla swojego budynku.
Administratorzy nieruchomości na swoich kontach widzą zgłoszenia powiązane z ich nieruchomościami. Przy realizacji danego zgłoszenia można dodawać notatki z kolejnymi etapami realizacji danego zgłoszenia – dzięki temu osoba pracująca na recepcji może łatwo sprawdzić status realizacji konkretnej sprawy.
Aplikacja zakłada następujące zakresy uprawnień kont:
- konto administratora aplikacji – tworzy bazę nieruchomości oraz użytkowników aplikacji i przypisuje ich do konkretnych nieruchomości.
- konto recepcja – ma możliwość dodawania nowych zgłoszeń do każdej nieruchomości i wybierania osoby odpowiedzialnej. Ma możliwość zmiany statusu zgłoszenia, ale nie może zamykać zgłoszeń.
- konto zarząd – ma możliwość dodawania nowych zgłoszeń do swojej nieruchomości oraz pogląd wszystkich zgłoszeń dla swojego budynku.
- konto administratora nieruchomości – widzi jedynie zgłoszenia powiązane z jego budynkami. Ma możliwość zmiany statusu zgłoszenia oraz na zamykanie zgłoszeń

![image](https://user-images.githubusercontent.com/36272145/162808266-b94dfd1f-6e6c-4b2f-a08b-d22a96b878a3.png)

![gr1_ClassDiagram](https://user-images.githubusercontent.com/36272145/162809988-5e83058b-38f3-4f60-9acd-6907a0752607.png)


![image](https://user-images.githubusercontent.com/36272145/162809236-4ebea4a5-776f-449b-912c-12e5f23f0788.png)

![image](https://user-images.githubusercontent.com/36272145/162809368-e4cca490-80d2-4484-8df8-8d30800d90a6.png)
