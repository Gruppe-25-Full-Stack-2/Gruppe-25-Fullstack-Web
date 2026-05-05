 ## Innlogging

  En standardbruker opprettes automatisk ved første oppstart:

  | Felt    | Verdi              |
  | ------- | ------------------ |
  | E-post  | `admin@usn.no`     |
  | Passord | `Admin123!`        |

  Brukeren opprettes av `BrukerInitialiserer` i `Data/`. Endre `DefaultEmail` og
  `DefaultPassword` der hvis dere vil ha en annen standardbruker. Nye brukere
  kan også registreres via `/Identity/Account/Register`.

  Sider merket med `[Authorize]` krever innlogging.

