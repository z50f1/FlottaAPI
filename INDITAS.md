# Flotta API – javított változat

## Indítás
1. Telepítsd a .NET 9 SDK-t (a projekt `net9.0`-t céloz).
2. Indítsd a MySQL/MariaDB szervert (például XAMPP-ban a MySQL-t).
3. Hozd létre/importáld a `flotta` adatbázist a megfelelő táblákkal. **Az eredeti ZIP nem tartalmaz SQL-adatbázismentést.**
4. Ha kell, módosítsd a `Flotta_API/appsettings.json` `FlottaConnection` értékét (felhasználó/jelszó/port). Ha a szerver nem MariaDB 10.4-kompatibilis, a `Program.cs`-ben a `MariaDbServerVersion`-t a tényleges verzióra kell állítani.
5. A `Flotta_API` projektmappából futtasd:
   ```bash
   dotnet restore
   dotnet run --launch-profile http
   ```
6. A végpontok például: `http://localhost:5239/api/Sofor/GetAll`, `http://localhost:5239/api/Gepjarmu/GetAll`.
7. Swagger UI-hoz `dotnet run --launch-profile https`, majd `https://localhost:7118/swagger` (HTTPS tanúsítvány első indításkor jóváhagyást kérhet).

## Login kérés
`POST /api/Login`, JSON body:
```json
{"loginName":"felhasznalo","password":"jelszo"}
```
Csak Argon2 PHC-formátumú hash ellenőrzése után ad sikeres választ. A visszaadott üzenet **nem hozzáférési token**, JWT alapú auth nincs implementálva. A belépés az adatbázisban `Active=true` és `Status=active` mezőket is igényel.

## Fontos
- A ZIP-ben eredetileg nem volt adatbázis SQL dump, így az API adatbázis nélküli futását nem lehet garantálni.
- Ebben a környezetben .NET SDK hiányában a `dotnet build` és a tényleges adatbázis-integrációs teszt nem volt elvégezhető.
- A jelszavas DB hitelesítést lehetőleg környezeti változóval vagy user secrets megoldással add meg, ne éles titokként a Git repóban.
