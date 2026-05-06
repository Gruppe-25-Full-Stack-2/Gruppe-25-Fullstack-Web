# TSD2491 Gruppe 25 - Fullstack Web
Eksamensprosjekt i Programvareutvikling utviklet i ASP.NET Core MVC.
Denne applikasjonen lar brukere administrere bedrifter og sortere de i kategorier.
Det er lagt til mulighet for å importere bedrifter med Brønnøysundsregisteret sitt API.

## Frontend
Prosjektet benytter Tailwind CSS for styling i stedet for kun standard Bootstrap.

## Krav
Prosjekter krever .NET SDK 10 og Node.js.

## Kjøring
Klon prosjektet til egen mappe og gå inn i dette:
```
git clone https://github.com/Gruppe-25-Full-Stack-2/Gruppe-25-Fullstack-Web.git
cd Gruppe-25-Fullstack-web
```
Kjør prosjektet:
```
dotnet run --project Web
```
Prosjektet vil starte, se terminalvindu for eksakt port, eksempelvis: http://localhost:5292

## Innlogging

En standardbruker opprettes automatisk ved første oppstart:

  | Felt    | Verdi              |
  | ------- | ------------------ |
  | E-post  | `admin@usn.no`     |
  | Passord | `Admin123!`        |

Brukeren opprettes av `BrukerInitialiserer` i `Data/`. Endre `DefaultEmail` og `DefaultPassword` der hvis dere vil ha en annen standardbruker.
Nye brukere kan også registreres via `/Identity/Account/Register`. Sider merket med `[Authorize]` krever innlogging.

## Funksjonalitet
CRUD for både bedrift og kategori.
Bedrifter kan filtreres avhengig av hvilke kategori de tilhører.
API kan brukes for å hente bedrifter fra Brønnøysundregisteret. Maks importeringer per søk er som standard satt til 20.
Ved bruk av API vil bedriften, hvis organisasjonsnummeret allerede finnes, bli oppdatert.
Det er lagt til enkel autentisering, hvor det å legge til bedrifter og kategorier er tillat, mens sletting krever innlogging.

## Gitflow
Prosjektet er utviklet med Gitflow:
- main: stabil versjon
- dev: krav 1–6
- extraFeature: krav 7–10

## Tester
Prosjekter har enhetstester som kan kjøres ved bruk av:
```
dotnet test
```
Testene ligger i /Tests og sjekker CRUD-operasjoner i BedriftController og KategoriController.

## Dokumentasjon
Prosjektet er dokumentert med DocFX. For å se dette:
```
docfx Docs/docfx.json --serve
```
For eksakt portnummer, se i terminalen. Eksempel kan være: http://localhost:8080
