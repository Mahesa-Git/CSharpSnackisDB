SLUTRAPPORT CsharpSnackis (Publicerat: Motor-duon Forum) 
Mattias Hermansson Salmi mattias.hermansson.salmi@iths.se
Antonio Santana antonio.santana@iths.se 
Trello board https://trello.com/b/Nega4dSe/csharpsnackis
Gantt-chart finns att se p� Trello
Beskrivning av projektet
Projektet �r ett forum f�r diskussioner om valfria �mnen. Som grund finns st�d f�r just fordon. 
Avvikelser from urspungsplanen: Projektet saknar en tydlig API-gateway, ist�llet sker alla REST-anrop fr�n sidornas code-behind (cshtml.cs). I �vrigt saknas avvikelser.
2.	Tids�versikt
Start f�r projektet var 2021-05-18. Slutdatum var 2021-06-15. Uppskattad tids�tg�ng �r 100 timmar. 
Inget tidsbortfall har intr�ffat under perioden. 
En felbed�mning avseende m�ngden tid f�r att ordna relationer i SQL-databasen och ta bort relationer i ett led. Detta p�verkade dock inte projektet negativt.
Se bifogat Gannt-diagram.
3.	Tekniska krav
Teknisk plattform: Windows
Serverl�sning: IIS
Ramverk: .NET CORE 5
Bibliotek/Nuget-paket: API-projektet (CSharpsnackisDB.sln):
Microsoft.AspNetCore: Diagnostics.EntityFrameworkCore, Identity.EntityFrameworkCore, .Identity.UI, .Authentication.JwtBearer, .SqlServer, EntityFrameworkCore, Microsoft: .EntityFrameWorkCore.SqlServer, .EntityFrameworkCore.Tools, .Extensions.DependencyInjection, .VisualStudio.Web.CodeGeneration.Design .NewtonSoft.Json SwashBucke.AspNetCore
Webb-projektet (CSharpSnackisApp.sln)
Microsoft.AspNetCore.Http NewtonSoft.Json System.Windows.Extension
Datal�sning: MS SQL
Servermilj�: Azure
4.	Applikationsstruktur.
Projekt: CsharpSnackisApp (Site)
Beroenden: SnackisApi.cs (Bas-adress genereras f�r REST-anrop), PostReactionModel.cs, CategoryResponseModel.cs, User.cs (Modell-klasser), SessionCheck.cs (Konvertera token fr�n aktuell session till byte-array). Startup.cs konfigureras cookies, session och m�jligheten att via @inject anv�nda IHttpContextAccessor, f�r att komma �t lagrad information i aktuell session.
Programmeringsm�nster: ASP.NET CORE, Razor Pages.
Projekt: CharpSnackisDB (API/DB) 
Beroenden: Random (Generera en slumpad tr�d fr�n exponerad API-metod). I Startup.cs sker konfiguering av autentisering och JwT-bearer tokens.
API-Struktur: Se SwaggerUI f�r controllerinformation: https://csharpsnackisdb20210614110849.azurewebsites.net/swagger/index.html�
Micro Services: API:et som enhet utg�r Micro service.
Programmeringsm�nster: Webb-API.
5.	Installation.
Projektet som helhet �r publicerat p� Azure: https://motorduonforum.azurewebsites.net/�
https://csharpsnackisdb20210614110849.azurewebsites.net/swagger/index.html�
F�r installation anv�nd Visual Studios publish funtionalitet. Connection-strings finns i App-settings.json filen. API-projektet: Aktuell migration �r genomf�rd vid �verl�mnandet. Vid debugging p� lokal SQL-server kr�vs update-database i package manager console. F�rdefinerade roller finns. �nskas detta �ndras sker detta i Context.cs. Seedning av inneh�ll, v�nligen se SeedController.cs.

6.	Endpoints.
V�nligen se SwaggerUI f�r en beskrivning av Endpoints. Endpoints har namngivits f�r att vara sj�lvf�rklarande. Vid fr�gor v�nligen kontakta utvecklarna.

7.	Funktionalitet.
Meny:
Samtliga bes�kare: L�nk till exponerad API-metod f�r en slumpm�ssig tr�d i forumet. Cookies, anv�ndaren godk�nner anv�ndandet av cookies. Info om GDPR finns.
Icke inloggad: Registrering/Inloggning, Microsoft Identity server som har anpassats med fler egenskaper anv�nds. Auktorisering sker med st�d av JwT-bearer token som lagras i en session hos anv�ndaren. Vid registrering kan uppladdning ske av visningsbild.
Inloggad anv�ndare: Mina sidor, Anv�ndaren kan �ndra profilbild eller radera den, detsamma g�ller all information. Bilder sparas p� webb-appens server. Bilder tas alltid bort fr�n servern vid �nskad radering. Andra anv�ndare som bes�ker profilen kan rapportera en ol�mplig anv�ndare.
Chatt: m�jlighet att vid markering av flera namn (ctrl + click) skapa gruppchatt. Vid val av en mottagare blir det en enkel chatt, dessa syns under �aktiva chattar�. Uppdatering av vald chatt kan ske vid behov f�r kontroll om nytt meddelande mottagits.
Inloggad Administrat�r: Mina sidor, Samma funktionalitet som hos en anv�ndare. En admin kan �ndra information och radera bilder vid bes�k p� en anv�ndares profil.
Admin dashboard, administrat�r kan se statistik �ver antal anv�ndare, poster i forumet, antal tr�dar och rapporterade objekt (poster, svar och anv�ndare). Enkelt att bes�ka rapporterade profiler och inl�gg. Inl�gg som rapporterats markeras upp och kan enkelt identifieras. Det g�r att bannlysa anv�ndare. N�r granskning genomf�rts av objekt kan detta tas bort i menyn.
Forumet:
Icke inloggad anv�ndare kan endast l�sa inl�gg och navigera p� forumet mellan kategorier, �mnen och tr�dar. Inloggning kr�vs f�r att anv�nda annan funktionalitet. 
Inloggade anv�ndare kan ut�ver detta skapa tr�dar, poster och svar. Reaktioner kan l�mnas p� inl�gg och foton kan laddas upp i alla led. Radering �r m�jlig i alla led (endast om anv�ndaren �r upphovsmakare) Rapportering av ol�mpliga inl�gg kan ske.

Inloggad administrat�r kan ut�ver detta skapa nya kategorier och �mnen. �ndring och radering �r m�jlig. Administrat�r �r beh�rig att ta bort och �ndra inl�gg av anv�ndare.
Ett filter finns f�r diverse fula ord som ers�tter den del av ordet som fastnat i filtret med asterisker. 
8.	Brister/ 9. F�rb�ttringsf�rslag
Ordfiltrering (API-metod): Filtret anv�nder Regex och st�d saknas f�r annat �n bokst�ver [a-z, A-Z]. Vid annan inmatning kommer filtret att sl�ppa igenom allt.
Optimering av antalet anrop till API:et kan ses �ver, f�r att undvika on�dig belastning p� databasen.
M�jlighet att radera chattar saknas, detta b�r implementeras. Idag beh�ver vi anv�nda SQL-fr�gor p� Azure f�r att ta bort dessa.
�ters�ndning av formul�r kan vid skickande av chattar och vid reaktioner p� inl�gg optimeras. En l�sning �r att �ven h�r implementera omdirigering och lagra n�dv�ndig data i aktuell session.

10.  Summering
Arbetet har g�tt extremt bra. Vi har haft en bra arbetsdynamik och metodik.
De utmaningarna som har uppst�tt var vid publicering av projektet d� server �r i en annan tidszon (UTC) samt anv�ndandet av statiska f�lt vid flera inloggade anv�ndare (bytte till session ist�llet).
De l�rdomarna som uppst�tt har varit hur man arbetar i en mindre grupp och par programmering �n innan.
Absolut vi trivs bra som kollegor samt v�nner. Detta har resulterat till att vi har LIA p� samma arbetsplats.
