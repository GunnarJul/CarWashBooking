CarWashBooking.
Projektet er udviklet til asp.net core version 2.2.0 og denne sdk skal være installeret .
Det kan hentes https://dotnet.microsoft.com/download/dotnet-core/2.2

Der har været en del problemer med at få databasen oprettet korrekt på en anden pc, end den løsningen er udviklet på.
Det burder være nok at afvikle disse kommandoer fra Package manager console:
* Add-Migration InitialDbSchema -context CarWashBookingDbContext
* Add-Migration IdentityDb -context CarWashBookingDbContext
* Update-Database -context CarWashBookingDbContext

hvis ikke alle tabeller er oprettet kan vedhæftede DB.sql afvikles fra SQL server Object explore i 
Visual Studio.

Følgende tabeller skal være oprettet:

* AspNetRoleClaims
* AspNetRoles
* AspNetUserClaims
* AspNetUserLogins
* AspNetUserRoles
* AspNetUsers
* AspNetUserTokens
* CarWash
* WashBooking

For at få logging projektet til at køre skal pakkerne installere nuget pakkerne Microsoft.extension.logging og Microsoft.extension.logging.Configuration.

Når projektet kan startes uden fejl, oprettes der automatisk en administrator bruger med credentical 
manager@bilvask.dk VaskDenBil!2#
En ny vaskehal skal have en jpg fil tilknyttet. Filen th.jpg i solution er me det formålet

Før man begynder at logge og oprette og bruger nye bruger, bør man først logge ind som administrator og oprett et par vaskehaller.
Administratorens bruger navn og kode er defineret i appsettings.json, så man kan ændre administratorens brugernavn og kode før start.
Administratoren har adgang til at kunne oprette og redigere vaske haller og har også adgang til statistik og 
antal on line bruger.


I appsettings.json defineres også den mail MailServer, port og credentials til samme.
Bemærk at 

Almindelige bruger, der opretter sig på siden, kan vælge mellem de vaskehaller der er oprettet at administratoren og
book valgte tidspunkter.

Under CarWashBooking\logs skrives løbende logilfiler, med notationen CarWashBooking_<DDMMYYHHMM>.log, der indeholder generelle informationer,
og CarWashBooking_error<DDMMYYHHMM>.log, der indeholder fejl opstået under kørselen.
